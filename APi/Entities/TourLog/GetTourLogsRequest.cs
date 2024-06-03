using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Exceptions;

namespace Tourplanner.Entities.TourLogs.Commands
{
    public record GetTourLogsRequest(int TourId) : IRequest;

    public class GetTourLogsRequestHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository,
        ITourRepository tourRepository)
        : RequestHandler<GetTourLogsRequest, IEnumerable<TourLogDto>>(ctx)
    {
        public override async Task<IEnumerable<TourLogDto>> Handle(GetTourLogsRequest request)
        {
            var tourLogs = await tourLogRepository.GetTourLogsForTour(request.TourId);
            var tour = await tourRepository.Get(request.TourId);

            if (tourLogs is null || tour is null)
            {
                throw new ResourceNotFoundException("Cant find tour log / tour");
            }

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
                    rating: log.Rating,
                    tour: new TourDto(
                        id: tour.Id,
                        name: tour.Name,
                        description: tour.Description,
                        from: tour.From,
                        to: tour.To,
                        transportType: tour.TransportType,
                        distance: tour.Distance,
                        timespan: tour.EstimatedTime,
                        popularity: tour.Popularity,
                        childfriendliness: tour.ChildFriendliness,
                        routeImage: tour.ImagePath
                    )
                ));
            }
            return await Task.FromResult<IEnumerable<TourLogDto>>(tourLogDtos.ToList());
        }
    }
}

