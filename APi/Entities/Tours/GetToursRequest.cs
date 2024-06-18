using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.OpenApi.Extensions;
using Tourplanner.DTOs;
using Tourplanner.Models;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tours
{
    public record GetToursRequest : IRequest
    {
    }

    public class GetToursRequestHandler(
        ITourRepository tourRepository,
        IChildFriendlinessService childFriendlinessService)
        : RequestHandler<GetToursRequest, IEnumerable<TourDto>>()
    {
        
        public override async Task<IEnumerable<TourDto>> Handle(GetToursRequest request)
        {
            var tourEntities = await tourRepository.GetAll();
            var tourDtos = new List<TourDto>();
            
            foreach (var tour in tourEntities)
            {
                var childFriendliness = childFriendlinessService.Calculate(tour.Id);

                tourDtos.Add(new TourDto(
                    tour.Id,
                    tour.Name,
                    tour.Description,
                    tour.From,
                    tour.To,
                    tour.Coordinates,
                    tour.TransportType,
                    tour.Distance,
                    tour.EstimatedTime,
                    tour.Popularity,
                    tour.ChildFriendliness
                ));
            }
            return await Task.FromResult(tourDtos);
        }
    }
}