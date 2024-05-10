using System.Linq;
using System.Threading.Tasks;
using Client.Pages;

public class TourEditPageViewModel
{
    public Tour Tour { get; private set; } = new Tour();

    public void UpdateTour()
    {
        Console.WriteLine("UpdateTour", Tour.Name);
    }

    public async Task InitializeAsync(int id)
    {
        await Task.Delay(500);
        // statt delay api call an backend

        Tour = new Tour
        {
            Id = id,
            Name = "Tour " + id,
            From = "From " + id,
            To = "To " + id,
            EstimatedTime = id * 1.5,
            Description = "Description " + id,
            TransportType = TransportType.Car,
            Difficulty = id % 5,
            Rating = id % 5
        };

    }

}