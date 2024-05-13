using Tourplanner.DTOs;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.TourLog
{
    public record GetSingleTourLogRequest(int TourId, int LogId) : IRequest;

    public class GetSingleTourLogRequestHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository,
        IRatingService ratingService)
        : RequestHandler<GetSingleTourLogRequest, TourLogDto>(ctx)
    {
        public override async Task<TourLogDto> Handle(GetSingleTourLogRequest request)
        {
            var log = await tourLogRepository.Get(request.LogId);

            if (log is null)
            {
                throw new ResourceNotFoundException($"Log {request.LogId} doesn't seem to exist");
            }
            
            var tourLogDto = 
                new TourLogDto(
                    id: log!.TourLogId,
                    tourId: log.TourId,
                    dateTime: log.Date,
                    comment: log.Comment,
                    difficulty: log.Difficulty,
                    totalTime: log.Duration,
                    rating: ratingService.Calculate(log.TourLogId)
                );

            return tourLogDto;
        }
    }
}