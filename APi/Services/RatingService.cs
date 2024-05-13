using Tourplanner.Repositories;

namespace Tourplanner.Services
{
    public interface IRatingService
    {
        public float Calculate(int tourId);
    }
    public class RatingService : IRatingService
    {
        private readonly ITourLogRepository tourLogRepository;

        public RatingService(ITourLogRepository tourLogRepository)
        {
            this.tourLogRepository = tourLogRepository;
        }

        public float Calculate(int tourId)
        {
            var tourLogs = tourLogRepository.GetTourLogsForTour(tourId);

            if (tourLogs == null || !tourLogs.Any())
            {
                return 0.0f;
            }

            float totalRating = tourLogs.Sum(t => t.Rating);
            float averageRating = totalRating / tourLogs.Count();

            return averageRating;
        }
    }
}