using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tour;

using Tourplanner.Infrastructure;

public record DeleteTourCommand(int Id) : IRequest;

public class DeleteTourCommandHandler(TourContext ctx, TourRepository tourRepository)
    : RequestHandler<DeleteTourCommand, Task>(ctx)
{
    public override async Task<Task> Handle(DeleteTourCommand command)
    {
        await tourRepository.DeleteById(command.Id);
        return Task.CompletedTask;
    }
}