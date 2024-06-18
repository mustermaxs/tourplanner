using Tourplanner.Exceptions;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tours;

using Tourplanner.Infrastructure;

public record DeleteTourCommand(int Id) : IRequest;

public class DeleteTourCommandHandler(
    ITourRepository tourRepository)
    : RequestHandler<DeleteTourCommand, Task>()
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