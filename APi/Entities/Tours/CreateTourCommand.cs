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
        TransportType TransportType
    ): IRequest;

    public class CreateTourCommandHandler(
        TourContext ctx,
        ITourRepository tourRepository,
        IChildFriendlinessService childFriendlinessService) : RequestHandler<CreateTourCommand, int>(ctx)
    {
        public override async Task<int> Handle(CreateTourCommand request)
        {
            var tour = new Tour
            {
                Name = request.Name,
                Description = request.Description,
                From = request.From,
                To = request.To,
                TransportType = request.TransportType,
                Popularity = 0.0f
            };

            var tourId = await tourRepository.CreateReturnId(tour);
            // var createMapCommand = new CreateMapCommand();

            tour.ChildFriendliness = await childFriendlinessService.Calculate(tourId); // TODO unnötig hier, wird eh nicht gespeichert

            return tourId;
        }
    }
}
