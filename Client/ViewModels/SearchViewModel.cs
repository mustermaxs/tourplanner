using Client.Pages;
public class SearchViewModel
{
    public List<Tour> TourResults { get; set; } = new List<Tour>();
    public List<TourLog> TourLogResults { get; set; } = new List<TourLog>();
    

    public async Task SearchTours(string query)
    {

        //add delay
        await Task.Delay(2000);
        Console.WriteLine($"Searching for tours with query: {query}");

        TourResults = new List<Tour>();
        TourLogResults = new List<TourLog>();


        TourResults.Add(new Tour());
        TourResults.Add(new Tour());
        TourResults.Add(new Tour());

        TourLogResults.Add(new TourLog());
        TourLogResults.Add(new TourLog());


    }

}
