using Tourplanner.Db;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.TourLogs
{
    public record DeleteTourLogCommand(int TourLogId) : IRequest;
    
    public class DeleteTourLogCommandHandler(
        IUnitOfWork unitOfWork,
        ITourLogRepository tourLogRepository,
        IRatingService ratingService)
    : RequestHandler<DeleteTourLogCommand, Task>()
    {
        public override async Task<Task> Handle(DeleteTourLogCommand request)
        {
            try
            {
                var tourLog = await tourLogRepository.Get(request.TourLogId);
                await unitOfWork.BeginTransactionAsync();

                if (tourLog is null)
                {
                    throw new ResourceNotFoundException($"Tour log {request.TourLogId} could not be found");
                }
            
                await unitOfWork.TourLogRepository.Delete(tourLog);
                var tour = await unitOfWork.TourRepository.Get(tourLog.TourId);
                var tourLogs = await unitOfWork.TourLogRepository.GetTourLogsForTour(tourLog.TourId);
                tour.Popularity = ratingService.Calculate(tourLogs);
                unitOfWork.TourRepository.UpdateAsync(tour);
                await unitOfWork.CommitAsync();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                await unitOfWork.RollbackAsync(e);
                throw;
            }

        }
    }
}

