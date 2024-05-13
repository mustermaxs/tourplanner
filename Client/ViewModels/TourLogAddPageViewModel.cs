using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Client.Dao;

public class TourLogAddPageViewModel
{
    public TourLog TourLog { get; set; } = new TourLog();
    private readonly NavigationManager NavigationManager;
    private ITourLogDao _tourLogDao;

    public TourLogAddPageViewModel(NavigationManager navigationManager, ITourLogDao tourLogDao)
    {
        NavigationManager = navigationManager;
        _tourLogDao = tourLogDao;
        TourLog = new TourLog();
        
    }

    public async Task InitializeAsync(int tourId)
    {
        TourLog.Tour.Id = tourId;
    }


    public async Task AddLog()
    {
        await _tourLogDao.Create(TourLog);
        NavigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
        TourLog = new TourLog();
    }
};
