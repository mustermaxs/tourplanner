using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Client.Dao;
using Client.Models;
using Client.Utils;
using Client.Components;
using Microsoft.AspNetCore.Components.Web;

public class TourAddPageViewModel
{
    private readonly ITourDao _tourDao;
    private PopupViewModel _popupVm;
    private readonly IHttpService _httpService;
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

    public List<GeoSuggestion> Suggestions { get; private set; } = new List<GeoSuggestion>{new GeoSuggestion("blabla", new Coordinates(1.9d, 2.0d))};
    private Action _notifyStateChanged;

    public TourAddPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupVm,
        IHttpService httpService)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupVm = popupVm;
        _httpService = httpService;
    }

    public async Task Initialize(Action notifySateChanged)
    {
        _notifyStateChanged = notifySateChanged;
    }

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

    public async Task GetSuggestion(string userInput)
    {
        Console.WriteLine(userInput);
        await Debouncer.Debounce<string>(GetGeoSuggestion, userInput, 1000);
        _notifyStateChanged.Invoke();
    }

    private async Task GetGeoSuggestion(string userInput)
    {
        var json = await _httpService.Get<JsonElement>($"Tours/geosuggestion?location={userInput}");

        var features = json.GetProperty("features");

        foreach (JsonElement feature in features.EnumerateArray())
        {
            var jsonCoordinates = feature.GetProperty("geometry").GetProperty("coordinates").EnumerateArray();
            var longitude = jsonCoordinates.ElementAt(0).GetDouble();
            var lattitute = jsonCoordinates.ElementAt(1).GetDouble();
            var coordinates = new Coordinates(longitude, lattitute);
            var properties = feature.GetProperty("properties");
            try
            {
                Suggestions.Add(
                    new GeoSuggestion(
                        properties.GetProperty("label").GetString(),
                        coordinates
                    )
                );
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}