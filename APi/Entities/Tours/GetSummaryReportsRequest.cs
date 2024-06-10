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

    public record GetSummaryReportRequest() : IRequest
    { }

    public class GetSummaryReportRequestHandler(TourContext ctx, ITourRepository tourRepository, IReportService reportService, IMapRepository mapRepository) : RequestHandler<GetSummaryReportRequest, byte[]>(ctx)
    {
        public override async Task<byte[]> Handle(GetSummaryReportRequest request)
        {
            IEnumerable<Tour> tours = await tourRepository.GetToursWithLogs();

            if (tours is null)
            {
                throw new ResourceNotFoundException($"Failed to get Tours with Logs");
            }

            foreach (var tour in tours)
            {
                var mapEntity = await mapRepository.GetMapWithTilesForTour(tour.Id);


                if (mapEntity is null)
                {
                    throw new ResourceNotFoundException($"Failed to get Map for Tour {tour.Id}");
                }
                tour.Map = mapEntity;
            }

            return reportService.GenerateSummaryReport(tours);
        }
    }


}
