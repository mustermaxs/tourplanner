using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.TourLogs
{
    public record GetTourLogsRequest(int TourId) : IRequest;

    public class GetTourLogsRequestHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository)
        : RequestHandler<GetTourLogsRequest, IEnumerable<TourLogDto>>(ctx)
    {
        public override async Task<IEnumerable<TourLogDto>> Handle(GetTourLogsRequest request)
        {
            var tourLogs = await tourLogRepository.GetTourLogsForTour(request.TourId);
            var tourLogDtos = new List<TourLogDto>();
            
            foreach (var log in tourLogs)
            {
                tourLogDtos.Add(new TourLogDto(
                    id: log.TourLogId,
                    tourId: log.TourId,
                    dateTime: log.Date,
                    comment: log.Comment,
                    difficulty: log.Difficulty,
                    totalTime: log.Duration,
                    rating: log.Rating
                ));
            }

            return await Task.FromResult<IEnumerable<TourLogDto>>(tourLogDtos.ToList());
        }
    }
}

