using Client.Models;
using Client.Dtos;

namespace Client.Utils;
using Client.Utils.Specifications;

public class AddTourSpecification : CompositeSpecification<Tour>
{
    private readonly Tour _tour;
    private readonly bool _from;
    private readonly bool _to;

    public AddTourSpecification(bool fromExistsInSuggestionList, bool toExistsInSuggestionList)
    {
        _from = fromExistsInSuggestionList;
        _to = toExistsInSuggestionList;
    }
    public override bool IsSatisfiedBy(Tour tour)
    {
        return (
            (_from ) &&
            (_to) &&
            !string.IsNullOrEmpty(tour.Description) && tour.Description.Length < 500 &&
            !string.IsNullOrEmpty(tour.From) && tour.From.Length < 150 &&
            !string.IsNullOrEmpty(tour.To) && tour.To.Length < 150 &&
            !string.IsNullOrEmpty(tour.Name) && tour.Name.Length < 60);
    }
}