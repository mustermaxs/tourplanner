using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Services;

namespace Tourplanner.Entities.TourLogs.Commands
{
    public record CreateTourLogCommand(
        int TourId,
        DateTime DateTime,
        string Comment,
        float Difficulty,
        float Rating,
        float Duration,
        float Distance
    ) : IRequest;
    
    public class CreateTourLogCommandHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository,
        ITourRepository tourRepository,
        IChildFriendlinessService childFriendlinessService,
        IRatingService ratingService) : RequestHandler<CreateTourLogCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(CreateTourLogCommand request)
        {
            var tourLog = new TourLog();
            tourLog.TourId = request.TourId;
            tourLog.DateTime = request.DateTime;
            tourLog.Comment = request.Comment;
            tourLog.Difficulty = request.Difficulty;
            tourLog.Rating = request.Rating;
            tourLog.Duration = request.Duration;
            tourLog.Distance=  request.Distance;
            await tourLogRepository.Create(tourLog);
            
            var tour = await tourRepository.Get(request.TourId);
            tour.Popularity = ratingService.Calculate(await tourLogRepository.GetTourLogsForTour(request.TourId));
            tour.ChildFriendliness = await childFriendlinessService.Calculate(tour.Id);
            await tourRepository.UpdateAsync(tour);
            
            return Task.CompletedTask;
        }
    }
}

