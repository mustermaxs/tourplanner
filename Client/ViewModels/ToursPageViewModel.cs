using Client.Models;
using Client.Dao;
using System.Net.Http;
using System.IO;

public class ToursPageViewModel
{
    private ITourDao _tourDao;
    private IReportService _reportService;
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();

    public ToursPageViewModel(ITourDao tourDao,IReportService reportService)
    {
        _tourDao = tourDao;
        _reportService = reportService;
    }

    public async Task DownloadReportAsync()
    {
        try
        {
            await _reportService.GetSummaryReport();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading report: {ex.Message}");
            throw;
        }
    }

    public async Task GetToursAsync()
    {
        try
        {
            Tours = await _tourDao.ReadMultiple();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tours: {ex.Message}");
            throw;
        }
    }
}
