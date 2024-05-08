﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.DTOs;
using Tourplanner.Models;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tour
{
    public record GetToursRequest : IRequest
    {
        public int TourId { get; set; }
    }

    public class GetToursCommandHandler(
        TourContext ctx,
        TourRepository tourRepository)
        : RequestHandler<GetToursRequest, IEnumerable<TourDto>>(ctx)
    {
        private TourRepository _tourRepository;
        
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
                        TransportType.Car,
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