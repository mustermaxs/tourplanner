using Client.Models;

namespace Client.Utils;
using Client.Utils.Specifications;

public class AddTourSpecification : CompositeSpecification<Tour>
{
    private readonly Tour _tour;
    private readonly Location? _from;
    private readonly Location? _to;

    public AddTourSpecification(Location? from, Location? to)
    {
        _from = from;
        _to = to;
    }
    public override bool IsSatisfiedBy(Tour tour)
    {
        return (
            (_from != null) &&
            (_to != null) &&
            !string.IsNullOrEmpty(tour.Description) && tour.Description.Length < 500 &&
            !string.IsNullOrEmpty(tour.From) && tour.From.Length < 150 &&
            !string.IsNullOrEmpty(tour.To) && tour.To.Length < 150 &&
            !string.IsNullOrEmpty(tour.Name) && tour.Name.Length < 60);
    }
}