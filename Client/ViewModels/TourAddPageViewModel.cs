using Microsoft.AspNetCore.Components;
using Client.Dao;
using Client.Models;

public class TourAddPageViewModel
{
    private readonly ITourDao _tourDao;
    private readonly NavigationManager _navigationManager;

    public TourAddPageViewModel(NavigationManager navigationManager, ITourDao tourDao)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
    }
    public Tour Tour { get; private set; } = new Tour();

    public async Task AddTour()
    {
        try
        {

            await _tourDao.Create(Tour);
            Tour = new Tour();
            _navigationManager.NavigateTo("/tours");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding tour: {ex.Message}");
        }
    }
}
