using Tourplanner.Entities.Tour;

namespace Tourplanner.Repositories;

public class TourRepository : Repository<Tour>
{
    public TourRepository(TourContext tourContext) : base(tourContext)
    {
    }
}