using Client.Models;

namespace Client.Utils;
using Client.Utils.Specifications;

public class AddTourSpecification : CompositeSpecification<Tour>
{
    public override bool IsSatisfiedBy(Tour tour)
    {
        return !(
            string.IsNullOrEmpty(tour.Description) &&
            string.IsNullOrEmpty(tour.From) &&
            string.IsNullOrEmpty(tour.TourName));
    }
}