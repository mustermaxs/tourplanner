using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.TourLogs.Commands
{
    public record UpdateTourLogCommand(
        int TourLogId,
        DateTime DateTime,
        string Comment,
        float Difficulty,
        float Rating,
        float Duration,
        float Distance
    ) : IRequest;
    
    public class UpdateTourLogCommandHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository)
    : RequestHandler<UpdateTourLogCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(UpdateTourLogCommand request)
        {
            var tourLog = await tourLogRepository.Get(request.TourLogId);

            if (tourLog is null)
            {
                throw new ResourceNotFoundException($"Tour log entry {request.TourLogId} doesn't seem to exist");
            }
            
            tourLog.DateTime = request.DateTime;
            tourLog.Comment = request.Comment;
            tourLog.Difficulty = request.Difficulty;
            tourLog.Rating = request.Rating;
            tourLog.TourLogId = request.TourLogId;
            tourLog.Duration = request.Duration;
            tourLog.Distance = request.Distance;

            await tourLogRepository.UpdateAsync(tourLog);

            return Task.CompletedTask;
        }
    }
}

