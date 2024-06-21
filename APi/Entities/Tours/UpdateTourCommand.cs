using Tourplanner.Db;
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
        IUnitOfWork unitOfWork)
        : RequestHandler<UpdateTourCommand, Task>()
    {
        public override async Task<Task> Handle(UpdateTourCommand command)
        {
            unitOfWork.BeginTransactionAsync();
            try
            {
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

                await unitOfWork.TourRepository.UpdateAsync(entityToUpdate);
                unitOfWork.CommitAsync();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}