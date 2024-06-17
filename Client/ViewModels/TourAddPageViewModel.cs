using Client.Components;
using Client.Dao;
using Client.Models;
using Client.Services;
using Client.Utils;
using Microsoft.AspNetCore.Components;
using Client.Dtos;
using Client.Exceptions;

namespace Client.ViewModels;

public class TourAddPageViewModel : BaseViewModel
{
    private readonly ITourDao _tourDao;
    private PopupViewModel _popupVm;
    private readonly IHttpService _httpService;
    private readonly IGeoService _geoService;
    private readonly NavigationManager _navigationManager;
    public List<string> ARSCH { get; set; } = new List<string>{"FICK DICH"};
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

    public List<string>? Suggestions { get; private set; } = new List<string>();

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
            var from = (await _geoService.SearchLocation(Tour.From)).Features.FirstOrDefault()?.PropertiesDto;
            var to = (await _geoService.SearchLocation(Tour.To)).Features.FirstOrDefault().PropertiesDto;
            var tourSpecification = new AddTourSpecification(from, to);
            
            if (tourSpecification.IsSatisfiedBy(Tour))
            {
                Tour.Start = (await _geoService.SearchLocation(Tour.From)).Features.FirstOrDefault()?.GeometryDto
                    .Coordinates;
                Tour.Destination = (await _geoService.SearchLocation(Tour.To)).Features.FirstOrDefault()?.GeometryDto
                    .Coordinates;
                await _tourDao.Create(Tour);
                _popupVm.Open("", "Created new tour.", PopupStyle.Normal);
            }
            else
            {
                throw new InvalidUserInputException("Failed to add tour.");
            }

            Tour = new Tour();
            _navigationManager.NavigateTo("/tours");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding tour: {ex.Message}");
            throw new InvalidUserInputException( "Failed to add tour.");
        }
    }

    public async Task GetSuggestion(string userInput)
    {
        Console.WriteLine(userInput);
        var locations = await Debouncer.Debounce<string, OrsBaseDto>(_geoService.SearchLocation, userInput, 1000);
        
        if (locations != null && locations.Features.Any())
        {
            Suggestions = locations.Features.Select(f => f.PropertiesDto.Label).ToList();
            _notifyStateChanged.Invoke();
        }
        else
        {
            
        }
    }
}