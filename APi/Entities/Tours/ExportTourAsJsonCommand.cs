using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tours
{
    public record ExportTourAsJsonCommand(int TourId) : IRequest;

    public class ExportTourAsJsonCommandHandler(ITourRepository tourRepository)
        : RequestHandler<ExportTourAsJsonCommand, FileContentResult>()
    {
        public override async Task<FileContentResult> Handle(ExportTourAsJsonCommand request)
        {
            try
            {
                var tour = await tourRepository.Get(request.TourId);
                
                if (tour is null)
                    throw new ResourceNotFoundException($"Tour with ID {request.TourId} not found.");
                
                string tourAsJson = JsonSerializer.Serialize(tour.ToTourDto());
                byte[] fileBytes = Encoding.UTF8.GetBytes(tourAsJson);
                var fileName = $"tour_{request.TourId}.json";
                return new FileContentResult(fileBytes, "application/json")
                {
                    FileDownloadName = fileName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}