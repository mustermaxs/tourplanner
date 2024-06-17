using Client.Dao;
using Client.Exceptions;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Components;

namespace Client.ViewModels;

public class TourLogPageViewModel : BaseViewModel
{
    public TourLog TourLog { get; set; }
    private NavigationManager NavigationManager;
    private ITourLogDao _tourLogDao;

    public TourLogPageViewModel(NavigationManager navigationManager, ITourLogDao tourLogDao)
    {
        NavigationManager = navigationManager;
        this._tourLogDao = tourLogDao;
    }


    public async Task Init(int tourId, int logId)
    {
        TourLog = new TourLog();
        TourLog.Id = logId;
        TourLog.Tour.Id = tourId;
        TourLog = await _tourLogDao.Read(TourLog.Id);
        _notifyStateChanged.Invoke();
    }

    public async Task DeleteLog()
    {
        try
        {
            await _tourLogDao.Delete(TourLog);
            NavigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new UserActionException("Failed to delete log.", e);
        }
    }

    public async Task UpdateLog()
    {
        try
        {
            var addLogSpec = new TourLogSpecification();
            if (!addLogSpec.IsSatisfiedBy(TourLog))
            {
                throw new InvalidUserInputException("Please check your input. Some fields are missing or incorrect.");
            }

            await _tourLogDao.Update(TourLog);
            NavigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}