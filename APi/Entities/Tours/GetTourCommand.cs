using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Infrastructure;

namespace Tourplanner.Entities.Tour
{
    public record GetTourByIdRequest(int Id) : IRequest;

    public class GetTourByIdCommandHandler(
        TourContext ctx,
        TourRepository tourRepository)
        : RequestHandler<GetTourByIdRequest, TourDto>(ctx)
    {
        private TourRepository _tourRepository;

        public override async Task<TourDto> Handle(GetTourByIdRequest request)
        {
            var tour = await tourRepository.Get(request.Id);

            return await Task.FromResult(new TourDto(
                tour.TourId,
                tour.Description,
                tour.Name,
                tour.From,
                tour.To,
                TransportType.Car,
                tour.Distance,
                tour.EstimatedTime,
                tour.Popularity,
                tour.ChildFriendliness,
                tour.ImagePath));
        }
    }
}