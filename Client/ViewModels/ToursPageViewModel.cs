using System.Linq;
using System.Threading.Tasks;

public class ToursViewModel
{
    public Tour[] Tours { get; private set; } = [];

    public async Task InitializeAsync()
    {
        await Task.Delay(500);
        // statt delay api call an backend

        Tours = new Tour[]
        {
            new Tour { Id=1, Name = "Waldviertel", Description = "Waldviertel Tour", TransportType = "Bike", From = "Wien", To = "Waldviertel", Rating = 5},
            new Tour { Id=2, Name = "Donauradweg", Description = "Donauradweg Tour", TransportType = "Bike", From = "Passau", To = "Wien", Rating = 4},
            new Tour { Id=3, Name = "Alpe Adria", Description = "Alpe Adria Tour", TransportType = "Bike", From = "Salzburg", To = "Adria", Rating = 3},
            new Tour { Id=4, Name = "Grossglockner", Description = "Grossglockner Tour", TransportType = "Bike", From = "Salzburg", To = "Grossglockner", Rating = 2},
            new Tour { Id=5, Name = "Neusiedlersee", Description = "Neusiedlersee Tour", TransportType = "Bike", From = "Wien", To = "Neusiedlersee", Rating = 1}
        };
    }

    public async Task GetToursAsync()
    {
        await Task.Delay(500);
        // statt delay api call an backend

        Tours = Enumerable.Range(1, 5).Select(index => new Tour
        {
            Name = $"Tour {index}",
            Description = $"Description {index}",
            TransportType = $"Transport Type {index}",
            From = $"From {index}",
            To = $"To {index}",
            Rating = index
        }).ToArray();
    }

    public async Task GetTourAsync(int id)
    {
        await Task.Delay(500);
        // statt delay api call an backend

        var tour = new Tour
        {
            Name = $"Tour {id}",
            Description = $"Description {id}",
            TransportType = $"Transport Type {id}",
            From = $"From {id}",
            To = $"To {id}",
            Rating = id
        };

        // return tour;
    }

}

