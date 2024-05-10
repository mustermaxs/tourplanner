using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;

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

    public class CreateTourCommandHandler(TourContext ctx, TourRepository tourRepository) : RequestHandler<CreateTourCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(CreateTourCommand request)
        {
            var tour = new Tour();
            tour.Name = request.Name;
            tour.Description = request.Description;
            tour.From = request.From;
            tour.To = request.To;

            await tourRepository.Create(tour);
            return Task.CompletedTask;
        }
    }
}
