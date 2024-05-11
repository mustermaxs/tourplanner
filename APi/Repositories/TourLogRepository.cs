using Tourplanner.Entities.TourLog;

namespace Tourplanner.Repositories;

public class TourLogRepository : Repository<TourLog>, ITourLogRepository
{
    public TourLogRepository(TourContext tourContext) : base(tourContext)
    {
    }

    public IEnumerable<TourLog> GetTourLogsForTour(int tourid)
    {
        return dbSet.Where(log => log.TourId == tourid).ToList();
    }
}

public interface ITourLogRepository : IRepository<TourLog>
{
    IEnumerable<TourLog> GetTourLogsForTour(int tourid);
}