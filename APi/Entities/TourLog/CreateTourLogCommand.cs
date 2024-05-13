using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Entities.TourLogs;

namespace Tourplanner.Entities.TourLogs.Commands
{
    public record CreateTourLogCommand(
        int TourId,
        DateTime DateTime,
        string Comment,
        float Difficulty,
        float TotalTime,
        float Rating
    ) : IRequest;
    
    public class CreateTourLogCommandHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository,
        ITourRepository tourRepository) : RequestHandler<CreateTourLogCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(CreateTourLogCommand request)
        {
            var tourLog = new TourLog();
            tourLog.TourId = request.TourId;
            tourLog.DateTime = request.DateTime;
            tourLog.Comment = request.Comment;
            tourLog.Difficulty = request.Difficulty;
            tourLog.Duration = request.TotalTime;
            tourLog.Rating = request.Rating;

            await tourLogRepository.Create(tourLog);
            return Task.CompletedTask;
        }
    }
}

