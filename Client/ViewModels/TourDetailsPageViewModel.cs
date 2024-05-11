using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TourDetailsPageViewModel
{
    public Tour Tour { get; private set; } = new Tour();
    public List<TourLog> TourLogs { get; private set; } = [
        new TourLog { Id = 1, Date = new DateTime(2024, 6, 1), Comment = "Nice tour", Difficulty = 3, Duration = 3f, Rating = 5},
        new TourLog { Id = 3, Date = new DateTime(2024, 6, 3), Comment = "Awesome tour", Difficulty = 5, Duration = 9f, Rating = 5},
        new TourLog { Id = 4, Date = new DateTime(2024, 6, 4), Comment = "Nice tour", Difficulty = 3, Duration = 2f, Rating = 4 },
        new TourLog { Id = 5, Date = new DateTime(2024, 6, 5), Comment = "Great tour", Difficulty = 4, Duration = 1f, Rating = 5},
        new TourLog { Id = 6, Date = new DateTime(2024, 6, 6), Comment = "Awesome tour", Difficulty = 5, Duration = 5f, Rating = 5},
        new TourLog { Id = 7, Date = new DateTime(2024, 6, 7), Comment = "Nice tour", Difficulty = 3, Duration = 4f, Rating = 4},
    ];

    private readonly Tour[] Tours = new Tour[]
       {
        new Tour { Id=1, Name = "Waldviertel", Description = "Waldviertel Tour", TransportType = TransportType.Bicycle, From = "Wien", To = "Waldviertel", Popularity = 5, ChildFriendliness = 2, Distance = 200, EstimatedTime = 60},
        new Tour { Id=2, Name = "Donauradweg", Description = "Donauradweg Tour", TransportType = TransportType.Bicycle, From = "Passau", To = "Wien", Popularity = 4,ChildFriendliness = 3, Distance = 300, EstimatedTime = 90},
        new Tour { Id=3, Name = "Alpe Adria", Description = "Alpe Adria Tour", TransportType = TransportType.Bicycle, From = "Salzburg", To = "Adria", Popularity = 3, ChildFriendliness = 4, Distance = 400, EstimatedTime = 120},
        new Tour { Id=4, Name = "Grossglockner", Description = "Grossglockner Tour", TransportType = TransportType.Bicycle, From = "Salzburg", To = "Grossglockner", Popularity = 2, ChildFriendliness = 5, Distance = 500, EstimatedTime = 150},
        new Tour { Id=5, Name = "Neusiedlersee", Description = "Neusiedlersee Tour", TransportType = TransportType.Bicycle, From = "Wien", To = "Neusiedlersee", Popularity = 1, ChildFriendliness = 1, Distance = 100, EstimatedTime = 30}
       };
    

    public async Task InitializeAsync(int tourId)
    {
        await Task.Delay(500); // Mock delay for async operation
        Tour = Tours.FirstOrDefault(t => t.Id == tourId);
    }
}
