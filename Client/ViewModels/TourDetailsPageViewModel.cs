using Client.Components;
using Client.Dao;
using Client.Models;

namespace Client.ViewModels;

public class TourDetailsPageViewModel : BaseViewModel
{
    private ITourDao _tourDao;
    private ITourLogDao _tourLogDao;
    private readonly PopupViewModel _popupViewModel;
    public int tourId;
    private IReportService _reportService;

    public TourDetailsPageViewModel(ITourDao tourDao, ITourLogDao TourLogDao, PopupViewModel popupViewModel, IReportService reportService)
    {
        _tourDao = tourDao;
        _tourLogDao = TourLogDao;
        _popupViewModel = popupViewModel;
        _reportService = reportService;
    }

    public Tour Tour { get; set; } = new Tour();
    public List<TourLog> TourLogs { get; set; } = new List<TourLog>();

    public override async Task InitializeAsync(Action notifySateChanged)
    {
        await base.InitializeAsync(notifySateChanged);
        TourLogs = (List<TourLog>)await _tourLogDao.ReadMultiple(tourId);
        Tour = new Tour();
        Tour.Id = tourId;
        Tour = await _tourDao.Read(Tour);
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
            Console.WriteLine($"Error downloading report: {ex.Message}");
            throw;
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
            Console.WriteLine(e);
            _popupViewModel.Open("Error", "Failed to delete tour", PopupStyle.Error);
            throw;
        }
    }
}