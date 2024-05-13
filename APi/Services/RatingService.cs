using Tourplanner.Entities.TourLog;
using Tourplanner.Repositories;

namespace Tourplanner.Services
{
    public interface IRatingService
    {
        public float Calculate(IEnumerable<TourLog> tourLogs);
    }
    public class RatingService : IRatingService
    {

        public RatingService()
        {
        }

        public float Calculate(IEnumerable<TourLog> tourLogs)
        {
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