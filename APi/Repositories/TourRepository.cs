using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tours;
using Tourplanner.Exceptions;

namespace Tourplanner.Repositories;

public class TourRepository : Repository<Tour>, ITourRepository
{
    public TourRepository(TourContext tourContext) : base(tourContext)
    {
    }

    public async Task<Tour?> GetTourWithLogs(int tourId)
    {
        try
        {
            return await dbSet.Include(t => t.TourLogs)
                .FirstOrDefaultAsync(to => to.Id == tourId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new DataAccessLayerException("Failed to fetch tour with logs", e);
        }
    }

    public async Task<IEnumerable<Tour>> GetToursWithLogs()
    {
        try
        {
            return await dbSet.Include(t => t.TourLogs).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new DataAccessLayerException("Failed to fetch tours with logs", e);
        }
    }
}

public interface ITourRepository : IRepository<Tour>
{
    public Task<Tour?> GetTourWithLogs(int tourId);
    public Task<IEnumerable<Tour>> GetToursWithLogs();
}