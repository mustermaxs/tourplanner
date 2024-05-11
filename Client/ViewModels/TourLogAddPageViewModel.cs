using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

public class TourLogAddPageViewModel
{
    public TourLog TourLog { get; set; } = new TourLog();
    private readonly NavigationManager NavigationManager;

    public TourLogAddPageViewModel(NavigationManager navigationManager)
    {
        NavigationManager = navigationManager;
        TourLog = new TourLog();
    }

    public async Task AddLog()
    {
        await Task.Delay(500);

        Console.WriteLine($"Adding new log for Tour ID {TourLog.Id}");

        NavigationManager.NavigateTo($"/tour/{TourLog.Id}");
    }
};
