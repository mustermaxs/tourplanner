using Client.Components;
using Client.Dao;
using Client.Dtos;
using Client.Models;
using Client.Services;
using Client.Utils;
using Microsoft.AspNetCore.Components;

namespace Client.ViewModels;

public class TourEditPageViewModel : BaseViewModel
{
    private readonly ITourDao _tourDao;
    private readonly PopupViewModel _popupViewModel;
    private readonly NavigationManager _navigationManager;
    private IGeoService _geoService;
    public List<string> Suggestions { get; private set; } = new List<string>();

    public TourEditPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupViewModel, IGeoService geoService) : base()
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupViewModel = popupViewModel;
        _geoService = geoService;
    }

    public Tour Tour { get; set; } = new Tour();

    public async Task UpdateTour()
    {
        try
        {
            _notifyStateChanged.Invoke();
            var from = (await _geoService.SearchLocation(Tour.From)).Features.FirstOrDefault()?.PropertiesDto;
            var to = (await _geoService.SearchLocation(Tour.To)).Features.FirstOrDefault().PropertiesDto;
            var tourSpecification = new AddTourSpecification(from, to);
            
            if (tourSpecification.IsSatisfiedBy(Tour))
            {
                Tour.Start = (await _geoService.SearchLocation(Tour.From)).Features.FirstOrDefault()?.GeometryDto
                    .Coordinates;
                Tour.Destination = (await _geoService.SearchLocation(Tour.To)).Features.FirstOrDefault()?.GeometryDto
                    .Coordinates;
                await _tourDao.Update(Tour);
            _navigationManager.NavigateTo($"/tours/{Tour.Id}");
            _popupViewModel.Open("Success", "Updated tour!", PopupStyle.Normal);
            }
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
            Tour = await _tourDao.Read(Tour.Id);
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

    public async Task GetSuggestions(string userInput)
    {
        if (string.IsNullOrEmpty(userInput))
        {
             Suggestions = new List<string>();
        }
        try
        {
            var locations = await Debouncer.Debounce<string, OrsBaseDto>(_geoService.SearchLocation, userInput, 1000);

            if (locations is not null && locations.Features is not null)
            {
                Suggestions = locations.Features.Select(f => f.PropertiesDto.Label).ToList();
                _notifyStateChanged.Invoke();
            }
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