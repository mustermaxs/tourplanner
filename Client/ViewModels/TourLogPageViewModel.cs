using Microsoft.AspNetCore.Components;
using Client.Models;
using Client.Dto;
using Client.Dao;
public class TourLogPageViewModel
{
    public TourLog TourLog { get; set; }
    private NavigationManager NavigationManager;
    private ITourLogDao _tourLogDao;

    public TourLogPageViewModel(NavigationManager navigationManager, ITourLogDao tourLogDao)
    {
        NavigationManager = navigationManager;
        this._tourLogDao = tourLogDao;
    }

    public async Task InitializeAsync(int tourId, int logId)
    {
        TourLog = new TourLog();
        TourLog.Id = logId;
        TourLog.Tour.Id = tourId;
        TourLog = await _tourLogDao.Read(TourLog);
    }

    public async Task DeleteLog()
    {
    try {
        
        await _tourLogDao.Delete(TourLog);
        NavigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
        } catch (Exception e) {
            Console.WriteLine(e);
        }
    }

    public async Task UpdateLog()
    {
        await _tourLogDao.Update(TourLog);
        NavigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
    }
}
