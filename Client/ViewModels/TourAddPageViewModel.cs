using Client.Components;
using Client.Dao;
using Client.Models;
using Client.Services;
using Client.Utils;
using Microsoft.AspNetCore.Components;
using Client.Dtos;
using Client.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace Client.ViewModels;

public class TourAddPageViewModel : BaseViewModel
{
    private readonly ITourDao _tourDao;
    private PopupViewModel _popupVm;
    private readonly IHttpService _httpService;
    private readonly IGeoService _geoService;
    private readonly NavigationManager _navigationManager;
    public IBrowserFile? ImportTourFile { get; set; }
    public Tour Tour { get; set; } = new Tour();
    private string test { get; set; }

    public HashSet<string> AllSuggestions { get; set; } = new HashSet<string>();
    private bool IsFileUpload { get; set; } = false;


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
    public bool IsImportFromFileModalOpen { get; set; } = false;

    public TourAddPageViewModel(NavigationManager navigationManager, ITourDao tourDao, PopupViewModel popupVm,
        IHttpService httpService, IGeoService geoService)
    {
        _navigationManager = navigationManager;
        _tourDao = tourDao;
        _popupVm = popupVm;
        _httpService = httpService;
        _geoService = geoService;
    }

    public override async Task InitializeAsync(Action notifySateChanged)
    {
        Tour = new Tour();
        IsFileUpload = false;
        base.InitializeAsync(notifySateChanged);
    }

    public async Task AddTour()
    {
        try
        {
            _notifyStateChanged.Invoke();
            
            if (!IsFileUpload)
            {
                var from = AllSuggestions.TryGetValue(Tour.From, out string _);
                var to = AllSuggestions.TryGetValue(Tour.To, out string _);
                var tourSpecification = new AddTourSpecification(from, to);

                if (!tourSpecification.IsSatisfiedBy(Tour))
                {
                    Console.WriteLine($"Error adding tour. Input doesn't satisfy specification.");
                    throw new InvalidUserInputException("Failed to add tour.");
                }
            }

            Tour.Start = (await _geoService.SearchLocation(Tour.From)).Features.FirstOrDefault()?.GeometryDto
                .Coordinates;
            Tour.Destination = (await _geoService.SearchLocation(Tour.To)).Features.FirstOrDefault()?.GeometryDto
                .Coordinates;


            await _tourDao.Create(Tour);

            _popupVm.Open("", "Created new tour.", PopupStyle.Normal);
            _navigationManager.NavigateTo("/tours");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding tour: {ex.Message}");
            throw new InvalidUserInputException("Failed to add tour.");
        }
    }

    public async Task GetSuggestion(string userInput)
    {
        if (string.IsNullOrEmpty(userInput))
            return;
        var locations = await Debouncer.Debounce<string, OrsBaseDto>(_geoService.SearchLocation, userInput, 1000);

        if (locations != null && locations.Features.Any())
        {
            Suggestions = locations.Features.Select(f => f.PropertiesDto.Label).ToList();
            Suggestions.ForEach(s => AllSuggestions.Add(s));
            _notifyStateChanged.Invoke();
        }
    }

    public async Task LoadAndValidateFile(InputFileChangeEventArgs ev)
    {
        ImportTourFile = ev.File;

        using (var stream = ImportTourFile.OpenReadStream())
        using (var streamContent = new StreamContent(stream))
        {
            var fileContent = await streamContent.ReadAsStringAsync();
            try
            {
                var tour = (Tour)JsonSerializer.Deserialize<Tour>(fileContent,
                    options: new JsonSerializerOptions(JsonSerializerDefaults.Web));
                Tour = tour;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid tour file provided. {e.Message}");
                throw new InvalidUserInputException($"The file you selected is not a valid tour file.");
            }
        }
    }

    public async Task UploadTour()
    {
        IsImportFromFileModalOpen = false;
        IsFileUpload = true;
        await AddTour();
        _popupVm.Open("Success", "Uploaded Tour", PopupStyle.Normal);
    }
}