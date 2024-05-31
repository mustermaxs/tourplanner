using Microsoft.AspNetCore.Components;
using Client.Dao;
using Client.Models;
using Client.Utils;
using Client.Components;
using Client.Dto;

namespace Client.ViewModels;

public class SearchPageViewModel
{
    private readonly NavigationManager _navigationManager;
    private IHttpService _httpService;
    private readonly PopupViewModel _popupViewModel;
    public readonly string PageTitle = "Search Results";
    private GlobalSearchResultsDto SearchResults { get; set; }
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();
    public IEnumerable<TourLog> TourLogs { get; private set; } = new List<TourLog>();
    public bool IsSearchSuccessful { get; private set; }
    public bool FoundTours { get; private set; } = false;
    public bool FoundTourLogs { get; set; } = false;
    private Action _notifyStateChanged;
    public bool SearchCompleted { get; private set; }
    private string SearchTerm { get; set; }


    public SearchPageViewModel(
        NavigationManager navigationManager,
        IHttpService httpService,
        PopupViewModel popupViewModel)
    {
        _navigationManager = navigationManager;
        _httpService = httpService;
        _popupViewModel = popupViewModel;
    }

    public void Initialize(Action notifySateChanged)
    {
        _notifyStateChanged = notifySateChanged;
    }

    public async Task GetSearchResults(string? searchTerm)
    {
        try
        {
            Reset();
            
            if (String.IsNullOrEmpty(searchTerm)) return;
            
            SearchResults = await _httpService.Get<GlobalSearchResultsDto>($"search?q={searchTerm}");
            FoundTours = SearchResults.Tours.Any();
            FoundTourLogs = SearchResults.TourLogs.Any();

            Tours = FoundTours ? SearchResults.Tours : new List<Tour>();
            TourLogs = FoundTourLogs ? SearchResults.TourLogs : new List<TourLog>();

            SearchCompleted = true;
            _notifyStateChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _popupViewModel.Open("Error", "Something went wrong :(", PopupStyle.Error);
            SearchCompleted = true;
            Reset();
        }
    }

    private void Reset()
    {
        Tours = new List<Tour>();
        TourLogs = new List<TourLog>();
        SearchTerm = string.Empty;
        FoundTours = false;
        FoundTourLogs = false;
        _notifyStateChanged();
    }
}