using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.TourLog
{
    public record GetTourLogsRequest(int TourId) : IRequest;

    public class GetTourLogsRequestHandler(TourContext ctx, TourLogRepository tourLogRepository)
        : RequestHandler<GetTourLogsRequest, IEnumerable<TourLogDto>>(ctx)
    {
        public override async Task<IEnumerable<TourLogDto>> Handle(GetTourLogsRequest request)
        {
            var tourLogs = tourLogRepository.GetTourLogsForTour(request.TourId);
            var tourLogDtos = tourLogs.Select(log =>
                new TourLogDto(
                    id: log.TourLogId,
                    tourId: log.TourId,
                    dateTime: log.Date,
                    comment: log.Comment,
                    difficulty: log.Difficulty,
                    totalTime: log.Duration,
                    rating: log.Rating
                ));

            return await Task.FromResult<IEnumerable<TourLogDto>>(tourLogDtos.ToList());
        }
    }
}

