using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tour
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
        IChildFriendlinessService childFriendlinessService) : RequestHandler<CreateTourCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(CreateTourCommand request)
        {
            var tour = new Tour();
            tour.Name = request.Name;
            tour.Description = request.Description;
            tour.From = request.From;
            tour.To = request.To;
            tour.TransportType = request.TransportType;
            tour.Popularity = 0.0f;

            var tourId = await tourRepository.CreateReturnId(tour); // TODO return Id
            tour.ChildFriendliness = await childFriendlinessService.Calculate(tourId);
            return Task.CompletedTask;
        }
    }
}
