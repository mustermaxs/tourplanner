using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.OpenApi.Extensions;
using Tourplanner.DTOs;
using Tourplanner.Models;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;
using Tourplanner.Services;
using Tourplanner.Exceptions;

namespace Tourplanner.Entities.Tours.Commands
{

    public record GetTourReportRequest(int TourId) : IRequest
    {}
    
    public class GetTourReportRequestHandler(TourContext ctx, ITourRepository tourRepository, IReportService reportService, IMapRepository mapRepository) : RequestHandler<GetTourReportRequest, byte[]>(ctx)
    {
        public override async Task<byte[]> Handle(GetTourReportRequest request)
        {
            var tourEntity = await tourRepository.GetTourWithLogs(request.TourId);

            if (tourEntity is null)
            {
                throw new ResourceNotFoundException($"Failed to get Tour {request.TourId}");
            }

            var mapEntity = await mapRepository.GetMapWithTilesForTour(request.TourId);

            if (mapEntity is null)
            {
                throw new ResourceNotFoundException($"Failed to get Map for Tour {request.TourId}");
            }

            tourEntity.Map = mapEntity;

            return reportService.GenerateTourReport(tourEntity);
        }
    }


}
