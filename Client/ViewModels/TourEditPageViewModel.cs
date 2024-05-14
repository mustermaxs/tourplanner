using Microsoft.AspNetCore.Components;
using Client.Dao;
using Client.Models;

public class TourEditPageViewModel
{
    private readonly ITourDao _tourDao;
    private readonly NavigationManager _navigationManager;

    public TourEditPageViewModel(NavigationManager navigationManager, ITourDao tourDao)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
    }

    public Tour Tour { get; set; } = new Tour();

    public async Task UpdateTour()
    {
        try
        {
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
