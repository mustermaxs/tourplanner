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
    {}

    public class GetSummaryReportCommandHandler(TourContext ctx, ITourRepository tourRepository, IReportService reportService) : RequestHandler<GetSummaryReportRequest, byte[]>(ctx)
    {
        public override async Task<byte[]> Handle(GetSummaryReportRequest request)
        {
            IEnumerable<Tour> tours = await tourRepository.GetToursWithLogs();

            if (tours == null)
            {
                throw new ResourceNotFoundException($"Failed to get Tours with Logs");
            }

            return reportService.GenerateSummaryReport(tours);
        }
    }


}