using Client.Components;
using Client.Dao;
using Client.Exceptions;
using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Components;

namespace Client.ViewModels;

public class TourDetailsPageViewModel : BaseViewModel
{
    private ITourDao _tourDao;
    private ITourLogDao _tourLogDao;
    private readonly PopupViewModel _popupViewModel;
    public int tourId;
    private IReportService _reportService;
    public MapViewModel MapVM { get; set; }
    private IMapDao _mapDao;
    private NavigationManager _navigationManager;
    private readonly TourImportExportService _tourImportExportService;


    public TourDetailsPageViewModel(
        ITourDao tourDao,
        ITourLogDao TourLogDao,
        PopupViewModel popupViewModel,
        IReportService reportService,
        IMapDao mapDao,
        IHttpService httpService,
        TourImportExportService tourImportExportService,
        NavigationManager navigationManager)
    {
        _tourDao = tourDao;
        _tourLogDao = TourLogDao;
        _popupViewModel = popupViewModel;
        _reportService = reportService;
        _mapDao = mapDao;
        _tourImportExportService = tourImportExportService;
        _navigationManager = navigationManager;
    }

    public Tour Tour { get; set; } = new Tour();
    public List<TourLog> TourLogs { get; set; } = new List<TourLog>();
    public Map Map { get; set; } = new Map();

    public override async Task InitializeAsync(Action notifySateChanged)
    {
        await base.InitializeAsync(notifySateChanged);
        Tour = new Tour();
        Tour.Id = tourId;

    try {
        Tour = await _tourDao.Read(tourId);
    } catch (Exception) {
        _navigationManager.NavigateTo("/notfound");
    }

        TourLogs = (List<TourLog>)await _tourLogDao.ReadMultiple(tourId);

        MapVM = new MapViewModel(_mapDao, tourId);

        _notifyStateChanged.Invoke();
    }


    public async Task DownloadReportAsync()
    {
        try
        {
            await _reportService.GetTourReport(Tour.Id);
        }
        catch (Exception ex)
        {
            throw new UserActionException("Failed to download report.");
        }
    }

    public async Task DeleteTour()
    {
        try
        {
            await _tourDao.Delete(Tour);

            foreach (var log in TourLogs)
            {
                await _tourLogDao.Delete(log);
            }

            _popupViewModel.Open("Success", "Deleted tour and tour logs!", PopupStyle.Normal);
        }
        catch (Exception e)
        {
            throw new UserActionException("Failed to delete tour.");
        }
    }

    public async Task ExportTourAsJson()
    {
        await _tourImportExportService.ExportToJsonFile(Tour.Id);
        _popupViewModel.Open("Success", "Exported tour!", PopupStyle.Normal);
    }
}