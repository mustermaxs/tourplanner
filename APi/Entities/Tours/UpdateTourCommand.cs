using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tour
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
        ITourRepository tourRepository)
        : RequestHandler<UpdateTourCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(UpdateTourCommand command)
        {
            var entityToUpdate = await tourRepository.Get(command.Id);

            if (entityToUpdate is null)
            {
                throw new ResourceNotFoundException($"Tour '{command.Name}', #{command.Id} could not be found");
            }

            entityToUpdate.Name = command.Name;
            entityToUpdate.Description = command.Description;
            entityToUpdate.From = command.From;
            entityToUpdate.To = command.To;

            await tourRepository.UpdateAsync(entityToUpdate);
            return Task.CompletedTask;
        }
    }
}

