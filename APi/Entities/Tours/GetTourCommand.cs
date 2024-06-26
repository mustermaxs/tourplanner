using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Exceptions;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Infrastructure;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tours
{
    public record GetTourByIdRequest(int Id) : IRequest;

    public class GetTourByIdRequestHandler(
        ITourRepository tourRepository,
        ITourLogRepository tourLogRepository,
        IRatingService ratingService,
        IChildFriendlinessService childFriendlinessService,
        IMapRepository mapRepository)
        : RequestHandler<GetTourByIdRequest, TourDto>()
    {
        public override async Task<TourDto> Handle(GetTourByIdRequest request)
        {
            var tour = await tourRepository.GetTourWithLogs(request.Id);
            if (tour is null)
            {
                throw new ResourceNotFoundException($"Tour {request.Id} doesn't seem to exist.");
            }

            var map = await mapRepository.GetMapWithTilesForTour(tour.Id);

            if(map is null)
            {
                throw new ResourceNotFoundException($"Map for tour {tour.Id} doesn't seem to exist.");
            }

            tour.Map = map;
            var childFriendliness = await childFriendlinessService.Calculate(tour.Id);
            var popularity = ratingService.Calculate(tour.TourLogs);
            
            return await Task.FromResult(new TourDto(
                tour.Id,
                tour.Name,
                tour.Description,
                tour.From,
                tour.To,
                tour.Coordinates,
                tour.TransportType,
                tour.Distance,
                tour.EstimatedTime,
                popularity,
                childFriendliness));
            
        }
    }
}