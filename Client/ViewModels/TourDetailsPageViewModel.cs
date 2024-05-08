using System.Linq;
using System.Threading.Tasks;
using Client.Pages;

public class TourDetailsPageViewModel
{
    public Tour Tour { get; private set; }

    private readonly Tour[] Tours = new Tour[]
       {
        new Tour { Id=1, Name = "Waldviertel", Description = "Waldviertel Tour", TransportType = "Bike", From = "Wien", To = "Waldviertel", Rating = 5},
        new Tour { Id=2, Name = "Donauradweg", Description = "Donauradweg Tour", TransportType = "Bike", From = "Passau", To = "Wien", Rating = 4},
        new Tour { Id=3, Name = "Alpe Adria", Description = "Alpe Adria Tour", TransportType = "Bike", From = "Salzburg", To = "Adria", Rating = 3},
        new Tour { Id=4, Name = "Grossglockner", Description = "Grossglockner Tour", TransportType = "Bike", From = "Salzburg", To = "Grossglockner", Rating = 2},
        new Tour { Id=5, Name = "Neusiedlersee", Description = "Neusiedlersee Tour", TransportType = "Bike", From = "Wien", To = "Neusiedlersee", Rating = 1}
       };

    public async Task InitializeAsync(int id)
    {
        await Task.Delay(500);
        // statt delay api call an backend

        Tour = Tours.FirstOrDefault(t => t.Id == id);

    }
}