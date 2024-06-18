using Tourplanner.DTOs;
using Tourplanner.Exceptions;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tours
{
    using Tourplanner.Infrastructure;
    
    public record ExportTourCommand (int TourId) : IRequest;


    public class ExportTourCommandHandler(
        ITourRepository tourRepository) : RequestHandler<ExportTourCommand, TourDto>()
    {
        public override async Task<TourDto> Handle(ExportTourCommand command)
        {
            var tour = await tourRepository.Get(command.TourId);

            if (tour is null)
            {
                throw new ResourceNotFoundException($"Tour with ID {command.TourId} not found.");
            }
            
            return tour.ToTourDto();
        }
    }
}