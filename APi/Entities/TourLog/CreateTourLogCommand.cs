using Tourplanner.Db;
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
        IUnitOfWork unitOfWork,
        IChildFriendlinessService childFriendlinessService,
        IRatingService ratingService) : RequestHandler<CreateTourLogCommand, Task>()
    {
        public override async Task<Task> Handle(CreateTourLogCommand request)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var tourLog = new TourLog();
                tourLog.TourId = request.TourId;
                tourLog.DateTime = request.DateTime;
                tourLog.Comment = request.Comment;
                tourLog.Difficulty = request.Difficulty;
                tourLog.Rating = request.Rating;
                tourLog.Duration = request.Duration;
                tourLog.Distance = request.Distance;
                await unitOfWork.TourLogRepository.Create(tourLog);

                await unitOfWork.CommitAsync();

                await unitOfWork.BeginTransactionAsync();

                var tour = await unitOfWork.TourRepository.Get(request.TourId);

                tour.Popularity = ratingService.Calculate(await unitOfWork.TourLogRepository.GetTourLogsForTour(request.TourId));

                tour.ChildFriendliness = await childFriendlinessService.Calculate(tour.Id);

                await unitOfWork.TourRepository.UpdateAsync(tour);

                await unitOfWork.CommitAsync();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}