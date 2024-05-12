using Microsoft.AspNetCore.Components;

public class TourLogPageViewModel
{
    public TourLog TourLog { get; set; }
    private IHttpService _httpService;
    private NavigationManager NavigationManager;

    public TourLogPageViewModel(NavigationManager navigationManager, IHttpService httpServus)
    {
        NavigationManager = navigationManager;
        _httpService = httpServus;
    }

    public async Task InitializeAsync(int logId)
    {
        await Task.Delay(500);
        
        TourLog = new TourLog
        {
            Date = DateTime.Now,
            Comment = "Ich liebe diese Tour!",
            Difficulty = 5,
            Rating = 8,
            Tour = new Tour { Id = 1, Name = "Waldviertel", Description = "Waldviertel Tour", TransportType = TransportType.Bicycle, From = "Wien", To = "Waldviertel", Popularity = 5},
        };
    }

    public async Task DeleteLog()
    {
        Console.WriteLine("Delete Log!");
        // await _httpService.Delete($"logs/{TourLog.Id}");
    }

    public async Task UpdateLog()
    {
        await Task.Delay(500);
        Console.WriteLine("Update successful!");
        NavigationManager.NavigateTo($"/tour/{TourLog.Tour.Id}");
    }
}
