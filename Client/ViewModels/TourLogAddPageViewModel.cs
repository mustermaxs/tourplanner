using Client.Components;
using Client.Dao;
using Client.Exceptions;
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

    public TourLogAddPageViewModel(NavigationManager navigationManager, ITourLogDao tourLogDao,
        PopupViewModel popupViewModel)
    {
        _navigationManager = navigationManager;
        _tourLogDao = tourLogDao;
        _popupViewModel = popupViewModel;
        TourLog = new TourLog();
    }

    public async Task AddLog()
    {
        try
        {
            var addLogSpec = new TourLogSpecification();

            if (addLogSpec.IsSatisfiedBy(TourLog))
            {
                await _tourLogDao.Create(TourLog);
                _navigationManager.NavigateTo($"/tours/{TourLog.Tour.Id}");
                TourLog = new TourLog();
                _popupViewModel.Open("Success", "Added tour log.", PopupStyle.Normal);
            }
            else
            {
                _notifyStateChanged.Invoke();
                throw new InvalidUserInputException("Tour Log input incorrect. Please check the form.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
};