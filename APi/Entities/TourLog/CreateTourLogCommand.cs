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
        float TotalTime,
        float Rating
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
            tourLog.Date = request.DateTime;
            tourLog.Comment = request.Comment;
            tourLog.Difficulty = request.Difficulty;
            tourLog.Duration = request.TotalTime;
            tourLog.Rating = request.Rating;
            await tourLogRepository.Create(tourLog);

            var tour = await tourRepository.Get(request.TourId);
            tour.ChildFriendliness = await childFriendlinessService.Calculate(tour.Id);
            await tourRepository.UpdateAsync(tour);
            
            return Task.CompletedTask;
        }
    }
}

