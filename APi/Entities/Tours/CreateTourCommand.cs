using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tours
{
    public record CreateTourCommand
    (
        string Name,
        string Description,
        string From,
        string To,
        TransportType TransportType,
        Coordinates Start,
        Coordinates Destination
    ): IRequest;

    public class CreateTourCommandHandler(
        ITourRepository tourRepository,
        IChildFriendlinessService childFriendlinessService,
        IOpenRouteService openRouteService) : RequestHandler<CreateTourCommand, int>()
    {
        public override async Task<int> Handle(CreateTourCommand request)
        {
            var tourRouteInfo = await openRouteService.RouteInfo(request.Start, request.Destination, request.TransportType);
            
            var tour = new Tour
            {
                Name = request.Name,
                Description = request.Description,
                From = request.From,
                To = request.To,
                TransportType = request.TransportType,
                Popularity = 0.0f,
                EstimatedTime = tourRouteInfo.Duration,
                Distance = tourRouteInfo.Distance
            };

            var tourId = await tourRepository.CreateReturnId(tour);

            tour.ChildFriendliness = await childFriendlinessService.Calculate(tourId); // TODO unnötig hier, wird eh nicht gespeichert

            return tourId;
        }
    }
}
