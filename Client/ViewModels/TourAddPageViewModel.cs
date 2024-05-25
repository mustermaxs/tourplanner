using Client.Components;
using Microsoft.AspNetCore.Components;
using Client.Dao;
using Client.Models;
using Client.Utils;

public class TourAddPageViewModel
{
    private readonly ITourDao _tourDao;
    private PopupViewModel _popupVm;
    private readonly NavigationManager _navigationManager;

    public TourAddPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupVm)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupVm = popupVm;
    }

    public Tour Tour { get; private set; } = new Tour();

    public async Task AddTour()
    {
        try
        {
            var tourSpecification = new AddTourSpecification();

            if (tourSpecification.IsSatisfiedBy(Tour))
            {
                await _tourDao.Create(Tour);
                _popupVm.Open("", "Created new tour.", PopupStyle.Normal);
            }
            else
            {
                throw new Exception();
            }

            Tour = new Tour();
            _navigationManager.NavigateTo("/tours");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding tour: {ex.Message}");
            _popupVm.Open("Error", "Failed to add tour.", PopupStyle.Error);
        }
    }
}