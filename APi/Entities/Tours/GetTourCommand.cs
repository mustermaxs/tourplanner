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
        TourContext ctx,
        ITourRepository tourRepository,
        ITourLogRepository tourLogRepository,
        IRatingService ratingService,
        IChildFriendlinessService childFriendlinessService)
        : RequestHandler<GetTourByIdRequest, TourDto>(ctx)
    {
        public override async Task<TourDto> Handle(GetTourByIdRequest request)
        {
            var tour = await tourRepository.GetTourWithLogs(request.Id);

            if (tour is null)
            {
                throw new ResourceNotFoundException($"Tour {request.Id} doesn't seem to exist.");
            }

            var childFriendliness = await childFriendlinessService.Calculate(tour.Id);
            var popularity = ratingService.Calculate(tour.TourLogs);
            
            return await Task.FromResult(new TourDto(
                tour.Id,
                tour.Name,
                tour.Description,
                tour.From,
                tour.To,
                tour.TransportType,
                tour.Distance,
                tour.EstimatedTime,
                popularity,
                childFriendliness,
                tour.ImagePath));
            
        }
    }
}