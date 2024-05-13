using Tourplanner.Entities.TourLogs;

namespace Tourplanner.Repositories;

public class TourLogRepository : Repository<TourLog>, ITourLogRepository
{
    public TourLogRepository(TourContext tourContext) : base(tourContext)
    {
    }

    public async Task<IEnumerable<TourLog>> GetTourLogsForTour(int tourId)
    {
        var logs = await GetAll();
        return logs;
    }


}

public interface ITourLogRepository : IRepository<TourLog>
{
    Task<IEnumerable<TourLog>> GetTourLogsForTour(int tourid);
}