using Client.Pages;
public class SearchViewModel
{
    public List<Tour> SearchResults { get; set; }

    public async Task SearchTours(string query)
    {

        //add delay
        await Task.Delay(2000);
        Console.WriteLine($"Searching for tours with query: {query}");

        SearchResults = new List<Tour>();

        SearchResults.Add(new Tour());
        SearchResults.Add(new Tour());
        SearchResults.Add(new Tour());
    }

    public SearchViewModel()
    {
        SearchResults = new List<Tour>();
    }

}
