using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Client.Dao;
using Client.Models;
using Client.Dto;

public class TourDetailsPageViewModel
{
    private ITourDao _tourDao;
    private ITourLogDao _tourLogDao;
    private IReportService _reportService;

    public TourDetailsPageViewModel(ITourDao tourDao, ITourLogDao TourLogDao, IReportService reportService)
    {
        _tourDao = tourDao;
        _tourLogDao = TourLogDao;
        _reportService = reportService;
    }

    public Tour Tour { get; set; } = new Tour();
    public List<TourLog> TourLogs { get; set; } = new List<TourLog>();

    public async Task InitializeAsync(int tourId)
    {
        TourLogs = (List<TourLog>) await _tourLogDao.ReadMultiple(tourId);
        Tour = new Tour();
        Tour.Id = tourId;
        Tour = await _tourDao.Read(Tour);
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
        await _tourDao.Delete(Tour);

        foreach (var log in TourLogs)
        {
            await _tourLogDao.Delete(log);
        }
    }
}