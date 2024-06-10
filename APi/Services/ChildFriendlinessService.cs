using Tourplanner.Entities.Tours;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Exceptions;
using Tourplanner.Repositories;
using Tourplanner.Entities;

namespace Tourplanner.Services
{
    public interface IChildFriendlinessService
    {
        public float GetAverageDifficulty(IEnumerable<TourLog> logs);
        public float GetMaxTourDistance(IEnumerable<Tour> tours);
        public Task<float> Calculate(int tourId);

    }
    public class ChildFriendlinessService : IChildFriendlinessService
    {
        private ITourRepository _tourRepository;
        private ITourLogRepository tourLogRepository;

        public ChildFriendlinessService(ITourRepository tourRepository, ITourLogRepository tourLogRepository)
        {
            _tourRepository = tourRepository;
            this.tourLogRepository = tourLogRepository;
        }

        public float GetAverageDifficulty(IEnumerable<TourLog> logs)
        {
            return logs.Any()
                ? logs.Sum(log => log.Difficulty) / logs.Count()
                : 0.0f;
        }

        public float GetMaxTourDistance(IEnumerable<Tour> tours)
        {
            return tours.Max(tour => tour.Distance);
        }

        public async Task<float> Calculate(int tourId)
        {
            var tours = await _tourRepository.GetAll();
            var tour = tours?.SingleOrDefault(t => t.Id == tourId) ?? null;

            if (tours is null || !tours.Any() || tour is null)
            {
                throw new ResourceNotFoundException("Tour not found");
            }

            var maxDistance = 10000f;
            var logs = tour.TourLogs;
            var avgDifficulty = GetAverageDifficulty(logs);
            var durationInHours = tour.EstimatedTime / 3600;
            var normalizedDistance = MathUtils.MapRange(tour.Distance, 0.0f, maxDistance, 0f, 10.0f);
            var normalizedDuration = MathUtils.MapRange(durationInHours, 0f, 2.0f, 0f, 10f);
            var sum = -normalizedDistance - normalizedDuration - avgDifficulty + 10;
            var normalizedSum = MathUtils.MapRange(sum, -20f, 30.0f, 0.0f, 10.0f);

            return normalizedSum;
        }
    }
}