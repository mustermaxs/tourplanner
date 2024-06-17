using Tourplanner.Entities.TourLogs;
using Tourplanner.Exceptions;

namespace Tourplanner.Repositories;

public class TourLogRepository : Repository<TourLog>, ITourLogRepository
{
    public TourLogRepository(TourContext tourContext) : base(tourContext)
    {
    }

    public async Task<IEnumerable<TourLog>> GetTourLogsForTour(int tourId)
    {
        try
        {
            return dbSet.Where(log => log.TourId == tourId).ToList();
        
            var logs = await GetAll();
            return logs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new DataAccessLayerException("Failed to fetch tour logs for tour", e);
        }

    }


}

public interface ITourLogRepository : IRepository<TourLog>
{
    Task<IEnumerable<TourLog>> GetTourLogsForTour(int tourid);
}