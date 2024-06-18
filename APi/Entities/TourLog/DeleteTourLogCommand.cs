using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.TourLogs
{
    public record DeleteTourLogCommand(int TourLogId) : IRequest;
    
    public class DeleteTourLogCommandHandler(
        ITourLogRepository tourLogRepository)
    : RequestHandler<DeleteTourLogCommand, Task>()
    {
        public override async Task<Task> Handle(DeleteTourLogCommand request)
        {
            var tourLog = await tourLogRepository.Get(request.TourLogId);


            if (tourLog is null)
            {
                throw new ResourceNotFoundException($"Tour log {request.TourLogId} could not be found");
            }
            
            await tourLogRepository.Delete(tourLog);

            return Task.CompletedTask;
        }
    }
}

