using Client.Components;
using Client.Dao;
using Client.Models;
using System.Net.Http;

namespace Client.ViewModels;

public class ToursPageViewModel : BaseViewModel
{
    private ITourDao _tourDao;
    private IReportService _reportService;
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();
    private PopupViewModel _popupViewModel;

    public ToursPageViewModel(ITourDao tourDao,IReportService reportService, PopupViewModel popupViewModel)
    {
        _tourDao = tourDao;
        _reportService = reportService;
        _popupViewModel = popupViewModel;
    }

    public async Task DownloadReportAsync()
    {
        try
        {
            await _reportService.GetSummaryReport();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading summary report.");
            throw;
        }
    }

    public async Task GetToursAsync()
    {
        try
        {
            Tours = await _tourDao.ReadMultiple();
            _notifyStateChanged.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tours: {ex.Message}");
            throw;
        }
    }
}
