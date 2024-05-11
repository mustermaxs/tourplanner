using Tourplanner.Entities.Tour;

namespace Tourplanner.Repositories;

public class TourRepository : Repository<Tour>, ITourRepository
{
    public TourRepository(TourContext tourContext) : base(tourContext)
    {
    }
}

public interface ITourRepository : IRepository<Tour> {}