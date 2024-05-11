using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.OpenApi.Extensions;
using Tourplanner.DTOs;
using Tourplanner.Models;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tour
{
    public record GetToursRequest : IRequest
    {
    }

    public class GetToursCommandHandler(
        TourContext ctx,
        ITourRepository tourRepository)
        : RequestHandler<GetToursRequest, IEnumerable<TourDto>>(ctx)
    {
        
        public override async Task<IEnumerable<TourDto>> Handle(GetToursRequest request)
        {
            var tourEntities = await tourRepository.GetAll();
            var tourDtos = tourEntities.Select(
                tour =>
                    new TourDto(
                        tour.TourId,
                        tour.Description,
                        tour.Name,
                        tour.From,
                        tour.To,
                        tour.TransportType,
                        tour.Distance,
                        tour.EstimatedTime,
                        tour.Popularity,
                        tour.ChildFriendliness,
                        tour.ImagePath
                    ));
            return await Task.FromResult(tourDtos);
        }
    }
}