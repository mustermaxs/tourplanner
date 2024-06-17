using Client.Components;
using Client.Dao;
using Client.Exceptions;
using Client.Models;
using Client.Services;

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
    private readonly IHttpService _httpService;
    private readonly TourImportExportService _tourImportExportService;


    public TourDetailsPageViewModel(
        ITourDao tourDao,
        ITourLogDao TourLogDao,
        PopupViewModel popupViewModel,
        IReportService reportService,
        IMapDao mapDao,
        IHttpService httpService,
        TourImportExportService tourImportExportService)
    {
        _tourDao = tourDao;
        _tourLogDao = TourLogDao;
        _popupViewModel = popupViewModel;
        _reportService = reportService;
        _mapDao = mapDao;
        _httpService = httpService;
        _tourImportExportService = tourImportExportService;
    }

    public Tour Tour { get; set; } = new Tour();
    public List<TourLog> TourLogs { get; set; } = new List<TourLog>();
    public Map Map { get; set; } = new Map();

    public override async Task InitializeAsync(Action notifySateChanged)
    {
        await base.InitializeAsync(notifySateChanged);
        TourLogs = (List<TourLog>)await _tourLogDao.ReadMultiple(tourId);
        Tour = new Tour();
        Tour.Id = tourId;
        Tour = await _tourDao.Read(tourId);

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