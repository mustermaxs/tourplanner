using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tour;

namespace Tourplanner.Repositories;

public class TourRepository : Repository<Tour>, ITourRepository
{
    public TourRepository(TourContext tourContext) : base(tourContext)
    {
    }

    public async Task<int> CreateReturnId(Tour tour)
    {
        await dbSet.AddAsync(tour);
        await SaveAsync();
        return tour.TourId;
    }

    public async Task<Tour?> GetTourWithLogs(int tourId)
    {
        return await dbSet.Include(t => t.TourLogs)
            .FirstOrDefaultAsync(to => to.TourId == tourId);
    }
}

public interface ITourRepository : IRepository<Tour>
{
    public Task<int> CreateReturnId(Tour tour);
    public Task<Tour?> GetTourWithLogs(int tourId);
}