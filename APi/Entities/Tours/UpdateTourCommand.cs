using Api.Entities.Maps;
using Tourplanner.Db;
using Tourplanner.Entities.Maps;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tours
{
    public record UpdateTourCommand(
        int Id,
        string Name,
        string Description,
        string From,
        string To,
        TransportType TransportType,
        Coordinates Start,
        Coordinates Destination
    ) : IRequest;

    public class UpdateTourCommandHandler(
        ITourRepository tourRepository,
        IRatingService ratingService,
        IChildFriendlinessService childFriendlinessService,
        IUnitOfWork unitOfWork,
        IOpenRouteService openRouteService,
        IMediator mediator)
        : RequestHandler<UpdateTourCommand, Task>()
    {
        public override async Task<Task> Handle(UpdateTourCommand command)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {

                var routeSummary = await openRouteService.RouteInfo(command.Start, command.Destination, command.TransportType);

                var entityToUpdate = await tourRepository.GetTourWithLogs(command.Id);

                if (entityToUpdate is null)
                {
                    throw new ResourceNotFoundException($"Tour '{command.Name}', #{command.Id} could not be found");
                }

                entityToUpdate.Name = command.Name;
                entityToUpdate.Description = command.Description;
                entityToUpdate.From = command.From;
                entityToUpdate.To = command.To;
                entityToUpdate.Popularity = ratingService.Calculate(entityToUpdate.TourLogs);
                entityToUpdate.ChildFriendliness = await childFriendlinessService.Calculate(entityToUpdate.Id);
                entityToUpdate.TransportType = command.TransportType;
                entityToUpdate.EstimatedTime = routeSummary.Duration;
                entityToUpdate.Distance = routeSummary.Distance;

                await unitOfWork.TourRepository.UpdateAsync(entityToUpdate);
                var deleteMapCommand = new DeleteMapForTourCommand(command.Id);
                await mediator.Send(deleteMapCommand);
                var createMapCommand = new CreateMapCommand(command.Id, routeSummary, command.TransportType); 
                await mediator.Send(createMapCommand);
                
                await unitOfWork.CommitAsync();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await unitOfWork.RollbackAsync(e);
                throw;
            }
        }
    }
}