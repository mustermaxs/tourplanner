using Tourplanner.Db;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;

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
        IUnitOfWork unitOfWork,
        IRatingService ratingService)
    : RequestHandler<UpdateTourLogCommand, Task>()
    {
        public override async Task<Task> Handle(UpdateTourLogCommand request)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var tourLog = await unitOfWork.TourLogRepository.Get(request.TourLogId);

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
                await unitOfWork.TourLogRepository.UpdateAsync(tourLog);
                
                var tour = await unitOfWork.TourRepository.Get(tourLog.TourId);
                var logs = await unitOfWork.TourLogRepository.GetTourLogsForTour(tourLog.TourId);
                tour.Popularity = ratingService.Calculate(logs);
                
                await unitOfWork.TourRepository.UpdateAsync(tour);
                await unitOfWork.CommitAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}

