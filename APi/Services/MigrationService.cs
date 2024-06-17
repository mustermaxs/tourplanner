using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tourplanner.Entities.Tours;
using Tourplanner.Exceptions;
using Tourplanner.Repositories;

namespace Tourplanner.Services
{

    public interface IMigrationService
    {
        Task<byte[]> ExportTour(int tourId);
        Task ImportTour(byte[] data);
    }

    public class MigrationService : IMigrationService
    {
        private readonly ITourRepository _tourRepository;

        public MigrationService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task<byte[]> ExportTour(int tourId)
        {
            var tour = await _tourRepository.Get(tourId);

            if (tour == null)
            {
                throw new ResourceNotFoundException($"Tour with ID {tourId} not found.");
            }

            var tourDto = tour.ToTourDto();

            if (tourDto == null)
            {
                throw new ResourceNotFoundException($"Tour with ID {tourId} not found.");
            }

            var json = JsonSerializer.Serialize(tourDto);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        public async Task ImportTour(byte[] data)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = System.Text.Encoding.UTF8.GetString(data);
            var tour = JsonSerializer.Deserialize<Tour>(json, options);

            if (tour != null)
            {
                await _tourRepository.CreateReturnId(tour);
            }
        }
    }
}

