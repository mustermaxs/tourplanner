using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;

namespace Tourplanner.Entities.Tour
{
    public record CreateTourCommand
    (
        string Name,
        string Description,
        string From,
        string To,
        float Distance,
        float EstimatedTime,
        string ImagePath
    ): IRequest;

    public class CreateTourCommandHandler(TourContext ctx) : RequestHandler<CreateTourCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(CreateTourCommand request)
        {
            var tour = new Tour();
            tour.Name = request.Name;
            tour.Description = request.Description;
            tour.Distance = request.Distance;
            tour.From = request.From;
            tour.To = request.To;
            tour.ImagePath = request.ImagePath;
            tour.EstimatedTime = TimeSpan.FromHours(request.EstimatedTime);

            await ctx.AddAsync(tour);
            
            return Task.CompletedTask;
        }
    }
}
