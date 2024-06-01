using Client.Components;
using Client.Dao;
using Client.Models;
using Client.Services;
using Client.Utils;
using Microsoft.AspNetCore.Components;

namespace Client.ViewModels;

public class TourAddPageViewModel : BaseViewModel
{
    private readonly ITourDao _tourDao;
    private PopupViewModel _popupVm;
    private readonly IHttpService _httpService;
    private readonly IGeoService _geoService;
    private readonly NavigationManager _navigationManager;
    public Tour Tour { get; set; } = new Tour();
    private string test { get; set; }
    public string Test
    {
        get => test;
        set
        {
            test = value;
            _notifyStateChanged.Invoke();
        }
    }

    public List<Location> Suggestions { get; private set; } = new List<Location>{new Location("blabla", new Coordinates(1.9d, 2.0d))};

    public TourAddPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupVm,
        IHttpService httpService, IGeoService geoService)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupVm = popupVm;
        _httpService = httpService;
        _geoService = geoService;
    }

    public async Task AddTour()
    {
        try
        {
            _notifyStateChanged.Invoke();
            var from = (await _geoService.SearchLocation(Tour.From)).FirstOrDefault();
            var to = (await _geoService.SearchLocation(Tour.To)).FirstOrDefault();
            var tourSpecification = new AddTourSpecification(from, to);
            
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

    private async Task<Location> GetLocationFromLabel(string label)
    {
        var location = (await _geoService.SearchLocation(label)).SingleOrDefault();

        if (location is null)
        {
            throw new Exception($"Couldn't find location {label}.");
            _popupVm.Open("Error", $"Couldn't find location {label}", PopupStyle.Error);
        }

        return location;
    }

    public async Task GetSuggestion(string userInput)
    {
        Console.WriteLine(userInput);
        var locations = await Debouncer.Debounce<string, List<Location>>(_geoService.SearchLocation, userInput, 1000);
        Suggestions = locations.Any() ? locations : new List<Location>();
        
        _notifyStateChanged.Invoke();
    }
}