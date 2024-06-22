using Client.Components;
using Client.Dao;
using Client.Dtos;
using Client.Exceptions;
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
    public HashSet<string> AllSuggestions { get; set; } = new HashSet<string>();

    public bool StartChanged = false;
    public bool DestinationChanged = false;

    public TourEditPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupViewModel,
        IGeoService geoService) : base()
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

            Console.WriteLine("StartChanged: " + StartChanged);
            Console.WriteLine("DestinationChanged: " + DestinationChanged);

            var from = StartChanged ? AllSuggestions.TryGetValue(Tour.From, out string _) : true;
            var to = DestinationChanged ? AllSuggestions.TryGetValue(Tour.To, out string _) : true;

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
            else
            {
                throw new InvalidUserInputException("Failed to update tour. Check your input.");
            }
        }
        catch (DaoException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            throw;
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
            Console.WriteLine($"Failed to get tour {Tour.Id} {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            throw;
        }
    }

    public async Task GetSuggestion(string userInput)
    {
    
        Console.Write("GetSuggestion: " + userInput);

        if (string.IsNullOrEmpty(userInput))
            return;

        if(AllSuggestions.Contains(userInput))
            return;

        var locations = await Debouncer.Debounce<string, OrsBaseDto>(_geoService.SearchLocation, userInput, 1000);

        if (locations != null && locations.Features.Any())
        {
            Suggestions = locations.Features.Select(f => f.PropertiesDto.Label).ToList();
            Suggestions.ForEach(s => AllSuggestions.Add(s));
            _notifyStateChanged.Invoke();
        }
    }
}