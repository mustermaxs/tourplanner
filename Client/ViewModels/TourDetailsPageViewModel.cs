using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TourDetailsPageViewModel
{
    public Tour Tour { get; private set; }
    public List<TourLog> TourLogs { get; private set; } = [
        new TourLog { Id = 1, Date = new DateTime(2024, 6, 1), Comment = "Nice tour", Difficulty = 3, TotalDistance = 100, TotalTime = new TimeSpan(1, 30, 0), Rating = 4},
        new TourLog { Id = 2, Date = new DateTime(2024, 6, 2), Comment = "Great tour", Difficulty = 4, TotalDistance = 120, TotalTime = new TimeSpan(2, 0, 0), Rating = 5},
        new TourLog { Id = 3, Date = new DateTime(2024, 6, 3), Comment = "Awesome tour", Difficulty = 5, TotalDistance = 150, TotalTime = new TimeSpan(2, 30, 0), Rating = 5},
        new TourLog { Id = 4, Date = new DateTime(2024, 6, 4), Comment = "Nice tour", Difficulty = 3, TotalDistance = 100, TotalTime = new TimeSpan(1, 30, 0), Rating = 4 },
        new TourLog { Id = 5, Date = new DateTime(2024, 6, 5), Comment = "Great tour", Difficulty = 4, TotalDistance = 120, TotalTime = new TimeSpan(2, 0, 0), Rating = 5},
        new TourLog { Id = 6, Date = new DateTime(2024, 6, 6), Comment = "Awesome tour", Difficulty = 5, TotalDistance = 150, TotalTime = new TimeSpan(2, 30, 0), Rating = 5},
        new TourLog { Id = 7, Date = new DateTime(2024, 6, 7), Comment = "Nice tour", Difficulty = 3, TotalDistance = 100, TotalTime = new TimeSpan(1, 30, 0), Rating = 4},
    ];

    private readonly Tour[] Tours = new Tour[]
       {
        new Tour { Id=1, Name = "Waldviertel", Description = "Waldviertel Tour", TransportType = TransportType.Bicycle, From = "Wien", To = "Waldviertel", Rating = 5},
        new Tour { Id=2, Name = "Donauradweg", Description = "Donauradweg Tour", TransportType = TransportType.Bicycle, From = "Passau", To = "Wien", Rating = 4},
        new Tour { Id=3, Name = "Alpe Adria", Description = "Alpe Adria Tour", TransportType = TransportType.Bicycle, From = "Salzburg", To = "Adria", Rating = 3},
        new Tour { Id=4, Name = "Grossglockner", Description = "Grossglockner Tour", TransportType = TransportType.Bicycle, From = "Salzburg", To = "Grossglockner", Rating = 2},
        new Tour { Id=5, Name = "Neusiedlersee", Description = "Neusiedlersee Tour", TransportType = TransportType.Bicycle, From = "Wien", To = "Neusiedlersee", Rating = 1}
       };
    

    public async Task InitializeAsync(int tourId)
    {
        await Task.Delay(500); // Mock delay for async operation
        Tour = Tours.FirstOrDefault(t => t.Id == tourId);
    }
}
