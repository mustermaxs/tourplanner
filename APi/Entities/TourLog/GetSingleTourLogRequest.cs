using Tourplanner.DTOs;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.TourLogs
{
    public record GetSingleTourLogRequest(int LogId) : IRequest;

    public class GetSingleTourLogRequestHandler(
        TourContext ctx,
        ITourLogRepository tourLogRepository,
        ITourRepository tourRepository,
        IRatingService ratingService)
        : RequestHandler<GetSingleTourLogRequest, TourLogDto>(ctx)
    {
        public override async Task<TourLogDto> Handle(GetSingleTourLogRequest request)
        {
            var log = await tourLogRepository.Get(request.LogId);
            var logs = (await tourRepository.GetTourWithLogs(log.TourId)).TourLogs;
            var tour = await tourRepository.Get(log.TourId);

            if (log is null)
            {
                throw new ResourceNotFoundException($"Log {request.LogId} doesn't seem to exist");
            }

            var tourLogDto =
                new TourLogDto(
                    id: log.TourLogId,
                    tourId: log.TourId,
                    dateTime: log.Date,
                    comment: log.Comment,
                    difficulty: log.Difficulty,
                    totalTime: log.Duration,
                    rating: log.Rating,
                    tourName: tour.Name,
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
                    ));
            return tourLogDto;
        }
    }
}