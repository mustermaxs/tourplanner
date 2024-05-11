using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

public class TourAddPageViewModel
{
    public Tour Tour { get; private set; }

    public TourAddPageViewModel()
    {
        Tour = new Tour();
    }

    public void AddTour()
    {
        try
        {
            Console.WriteLine($"Adding new tour: {JsonSerializer.Serialize(Tour)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding tour: {ex.Message}");
        }
    }
}
