using Client.Components;
using Microsoft.AspNetCore.Components;
using Client.Dao;
using Client.Models;
using Client.Utils;
public class TourEditPageViewModel
{
    private readonly ITourDao _tourDao;
    private IPopupService _popupService;
    private readonly NavigationManager _navigationManager;

    public TourEditPageViewModel(NavigationManager navigationManager, ITourDao tourDao, IPopupService popupService)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupService = popupService;
    }

    public Tour Tour { get; set; } = new Tour();

    public async Task UpdateTour()
    {
        try
        {
            // TODO validation
            await _tourDao.Update(Tour);
            _navigationManager.NavigateTo($"/tours/{Tour.Id}");
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

    public async Task InitializeAsync(int id)
    {
        try
        {
            _popupService.Show();
            Tour = new Tour { Id = id };
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
