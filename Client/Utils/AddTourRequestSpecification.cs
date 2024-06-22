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

    //log the values of the fields
    Console.WriteLine($"_from: {_from}");
    Console.WriteLine($"_to: {_to}");
    Console.WriteLine($"Description: {tour.Description}");
    Console.WriteLine($"From: {tour.From}");
    Console.WriteLine($"To: {tour.To}");
    Console.WriteLine($"Name: {tour.Name}");

        return (
            (_from ) &&
            (_to) &&
            !string.IsNullOrEmpty(tour.Description) && tour.Description.Length < 500 &&
            !string.IsNullOrEmpty(tour.From) && tour.From.Length < 150 &&
            !string.IsNullOrEmpty(tour.To) && tour.To.Length < 150 &&
            !string.IsNullOrEmpty(tour.Name) && tour.Name.Length < 60);
    }
}