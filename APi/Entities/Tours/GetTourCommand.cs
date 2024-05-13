using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Exceptions;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Infrastructure;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tour
{
    public record GetTourByIdRequest(int Id) : IRequest;

    public class GetTourByIdCommandHandler(
        TourContext ctx,
        ITourRepository tourRepository,
        IChildFriendlinessService childFriendlinessService)
        : RequestHandler<GetTourByIdRequest, TourDto>(ctx)
    {
        public override async Task<TourDto> Handle(GetTourByIdRequest request)
        {
            var tour = await tourRepository.Get(request.Id);

            if (tour is null)
            {
                throw new ResourceNotFoundException($"Tour {request.Id} doesn't seem to exist.");
            }

            var childFriendliness = await childFriendlinessService.Calculate(tour.TourId);
            
            return await Task.FromResult(new TourDto(
                tour.TourId,
                tour.Description,
                tour.Name,
                tour.From,
                tour.To,
                tour.TransportType,
                tour.Distance,
                tour.EstimatedTime,
                tour.Popularity,
                childFriendliness,
                tour.ImagePath));
            
        }
    }
}