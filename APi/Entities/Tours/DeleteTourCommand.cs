using Tourplanner.Exceptions;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tour;

using Tourplanner.Infrastructure;

public record DeleteTourCommand(int Id) : IRequest;

public class DeleteTourCommandHandler(
    TourContext ctx,
    ITourRepository tourRepository)
    : RequestHandler<DeleteTourCommand, Task>(ctx)
{
    public override async Task<Task> Handle(DeleteTourCommand command)
    {
        var tour = await tourRepository.Get(command.Id);

        if (tour is null)
        {
            throw new ResourceNotFoundException($"Tour {command.Id} could not be found");
        }
        
        await tourRepository.Delete(tour);
        return Task.CompletedTask;
    }
}