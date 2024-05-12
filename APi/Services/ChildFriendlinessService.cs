using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;
using Tourplanner.Exceptions;

namespace Tourplanner.Services;
using Tourplanner.Repositories;
using Tourplanner.Entities;

public class ChildFriendlinessService
{
    private ITourRepository _tourRepository;
    private ITourLogRepository tourLogRepository;

    public ChildFriendlinessService(ITourRepository tourRepository, ITourLogRepository tourLogRepository)
    {
        _tourRepository = tourRepository;
        this.tourLogRepository = tourLogRepository;
    }

    private float GetAverageDifficulty(IEnumerable<TourLog> logs)
    {
        return logs.Any()
            ? logs.Sum(log => log.Difficulty) / logs.Count()
            : 0.0f; 
    }

    private float GetMaxTourDistance(IEnumerable<Tour> tours)
    {
        return tours.Max(tour => tour.Distance);
    }
    
    public async Task<float> Calculate(int tourId)
    {
        var tours = await _tourRepository.GetAll();
        var tour = tours?.SingleOrDefault(t => t.TourId == tourId) ?? null;
        
        if (tours is null || !tours.Any() || tour is null)
        {
            throw new ResourceNotFoundException("Tour not found");
        }
        
        var maxDistance = tours.Max(tour => tour.Distance);
        var logs = tourLogRepository.GetTourLogsForTour(tourId);
        var avgDifficulty = GetAverageDifficulty(logs);
        var durationInHours = tour.EstimatedTime / 60;
        var normalizedDistance = MathUtils.MapRange(tour.Distance, 0.0f, maxDistance, 0f, 10.0f);
        var normalizedDuration = MathUtils.MapRange(durationInHours, 0f, 60.0f, 0f, 10f);
        var sum = tour.Distance + tour.EstimatedTime + avgDifficulty;
        var normalizedSum = MathUtils.MapRange(sum, 0f, 30.0f, 0.0f, 10.0f);

        return normalizedSum;
    }
}