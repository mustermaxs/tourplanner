using Microsoft.AspNetCore.Components;
using Client.Models;
using Client.Dto;
using Client.Dao;
public class TourLogPageViewModel
{
    public TourLog TourLog { get; set; }
    private NavigationManager NavigationManager;
    private ITourLogDao tourLogDao;

    public TourLogPageViewModel(NavigationManager navigationManager, ITourLogDao tourLogDao)
    {
        NavigationManager = navigationManager;
        this.tourLogDao = tourLogDao;
    }

    public async Task InitializeAsync(int logId)
    {
        await Task.Delay(500);

        TourLog = new TourLog();
        TourLog.Id = logId;
        TourLog = await tourLogDao.Read(TourLog);
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
