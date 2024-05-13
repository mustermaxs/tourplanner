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
        TransportType TransportType
    ) : IRequest;

    public class UpdateTourCommandHandler(
        TourContext ctx,
        ITourRepository tourRepository,
        IRatingService ratingService,
        IChildFriendlinessService childFriendlinessService)
        : RequestHandler<UpdateTourCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(UpdateTourCommand command)
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

            await tourRepository.UpdateAsync(entityToUpdate);
            return Task.CompletedTask;
        }
    }
}

