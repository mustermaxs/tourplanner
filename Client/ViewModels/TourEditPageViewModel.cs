using Client.Components;
using Client.Dao;
using Client.Models;
using Microsoft.AspNetCore.Components;

namespace Client.ViewModels;

public class TourEditPageViewModel : BaseViewModel
{
    private readonly ITourDao _tourDao;
    private readonly PopupViewModel _popupViewModel;
    private readonly NavigationManager _navigationManager;

    public TourEditPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupViewModel) : base()
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupViewModel = popupViewModel;
    }

    public Tour Tour { get; set; } = new Tour();

    public async Task UpdateTour()
    {
        try
        {
            await _tourDao.Update(Tour);
            _navigationManager.NavigateTo($"/tours/{Tour.Id}");
            _popupViewModel.Open("Success", "Updated tour!", PopupStyle.Normal);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            _popupViewModel.Open("Error", "Failed to update tour.", PopupStyle.Error);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            _popupViewModel.Open("Error", "Failed to update tour.", PopupStyle.Error);
        }
    }

    public void SetTourId(int id)
    {
        Tour = new Tour { Id = id };
    }

    public async Task InitializeAsync(Action notifySateChanged)
    {
        await base.InitializeAsync(notifySateChanged);
        
        try
        {
            Tour = await _tourDao.Read(Tour);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}