using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

public class TourAddPageViewModel
{
    private IHttpService _httpService;
    public Tour Tour { get; private set; }

    public TourAddPageViewModel(IHttpService httpService)
    {
        _httpService = httpService;
        Tour = new Tour();
    }

    public void AddTour()
    {
        try
        {
            var createTourDto = new CreateTourDto(
                name: Tour.Name,
                description: Tour.Description,
                from: Tour.From,
                to: Tour.To,
                distance: Tour.Distance,
                estimatedTime: Tour.EstimatedTime
                );
            _httpService.Post<CreateTourDto>(createTourDto, "Tours");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding tour: {ex.Message}");
        }
    }
}
