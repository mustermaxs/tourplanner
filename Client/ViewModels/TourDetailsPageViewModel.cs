using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TourDetailsPageViewModel
{
    private IHttpService _httpService;
    public TourDetailsPageViewModel(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Tour Tour { get; private set; } 
    public List<TourLog> TourLogs { get; private set; } = [
        new TourLog { Id = 1, Date = new DateTime(2024, 6, 1), Comment = "Nice tour", Difficulty = 3, Duration = 3f, Rating = 5},
        new TourLog { Id = 3, Date = new DateTime(2024, 6, 3), Comment = "Awesome tour", Difficulty = 5, Duration = 9f, Rating = 5},
        new TourLog { Id = 4, Date = new DateTime(2024, 6, 4), Comment = "Nice tour", Difficulty = 3, Duration = 2f, Rating = 4 },
        new TourLog { Id = 5, Date = new DateTime(2024, 6, 5), Comment = "Great tour", Difficulty = 4, Duration = 1f, Rating = 5},
        new TourLog { Id = 6, Date = new DateTime(2024, 6, 6), Comment = "Awesome tour", Difficulty = 5, Duration = 5f, Rating = 5},
        new TourLog { Id = 7, Date = new DateTime(2024, 6, 7), Comment = "Nice tour", Difficulty = 3, Duration = 4f, Rating = 4},
    ];

    public async Task GetTour(int tourId)
    {
        try
        {
            Tour = await _httpService.Get<Tour>($"Tours/{tourId}");
            Console.WriteLine(Tour.Name);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tours: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteTour()
    {
        await _httpService.Delete($"Tours/{Tour.Id}");
    }
}
