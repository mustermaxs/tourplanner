using Client.Components;
using Client.Dao;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Components;

namespace Client.ViewModels;

public class TourLogAddPageViewModel : BaseViewModel
{
    public TourLog TourLog { get; set; } = new TourLog();
    private readonly NavigationManager _navigationManager;
    private ITourLogDao _tourLogDao;
    private readonly PopupViewModel _popupViewModel;

    public TourLogAddPageViewModel(NavigationManager navigationManager, ITourLogDao tourLogDao, PopupViewModel popupViewModel)
    {
        _navigationManager = navigationManager;
        _tourLogDao = tourLogDao;
        _popupViewModel = popupViewModel;
        TourLog = new TourLog();
    }

    public async Task AddLog()
    {
        var addLogSpec = new TourLogSpecification();

        if (addLogSpec.IsSatisfiedBy(TourLog))
        {
            await _tourLogDao.Create(TourLog);
            _navigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
            TourLog = new TourLog();
        }
        else
        {
            _popupViewModel.Open("Error", "Failed to create tour log.", PopupStyle.Error);
            _notifyStateChanged.Invoke();
        }
    }
};