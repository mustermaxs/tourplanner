using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Exceptions;

namespace Tourplanner.Entities.TourLogs.Commands
{
    public record GetTourLogsRequest(int TourId) : IRequest;

    public class GetTourLogsRequestHandler(
        ITourLogRepository tourLogRepository,
        ITourRepository tourRepository)
        : RequestHandler<GetTourLogsRequest, IEnumerable<TourLogDto>>()
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
                tourLogDtos.Add(new TourLogDto(     // TODO extension method verwenden
                    id: log.TourLogId,
                    tourId: log.TourId,
                    dateTime: log.DateTime,
                    comment: log.Comment,
                    distance: log.Distance,
                    difficulty: log.Difficulty,
                    duration: log.Duration,
                    rating: log.Rating,
                    tour: new TourDto(              // TODO extension method verwenden
                        id: tour.Id,
                        name: tour.Name,
                        description: tour.Description,
                        from: tour.From,
                        to: tour.To,
                        coordinates: tour.Coordinates,
                        transportType: tour.TransportType,
                        distance: tour.Distance,
                        timespan: tour.EstimatedTime,
                        popularity: tour.Popularity,
                        childfriendliness: tour.ChildFriendliness
                        )
                ));
            }
            return await Task.FromResult<IEnumerable<TourLogDto>>(tourLogDtos.ToList());
        }
    }
}