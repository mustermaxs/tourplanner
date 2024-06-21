using Tourplanner.Db;
using Tourplanner.Exceptions;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tours;

using Tourplanner.Infrastructure;

public record DeleteTourCommand(int Id) : IRequest;

public class DeleteTourCommandHandler(
    IUnitOfWork unitOfWork,
    ITourRepository tourRepository)
    : RequestHandler<DeleteTourCommand, Task>()
{
    public override async Task<Task> Handle(DeleteTourCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var tour = await tourRepository.Get(command.Id);

            if (tour is null)
            {
                throw new ResourceNotFoundException($"Tour {command.Id} could not be found");
            }

            await unitOfWork.TourRepository.Delete(tour);
            await unitOfWork.CommitAsync();
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            unitOfWork.RollbackAsync();
            throw;
        }
    }
}