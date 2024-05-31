using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Client.Dao;
using Client.Models;
using Client.Dto;

public class TourDetailsPageViewModel
{
    private ITourDao _tourDao;
    private IJSRuntime _jsRuntime;
    private ITourLogDao _tourLogDao;

    public TourDetailsPageViewModel(ITourDao tourDao, ITourLogDao TourLogDao)
    {
        _tourDao = tourDao;
        _tourLogDao = TourLogDao;
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
            using (var httpClient = new HttpClient())
            {
                //TODO: make not idiot way to get the report
                var report = await httpClient.GetByteArrayAsync($"http://localhost:5161/api/reports/tours/{Tour.Id}");

                var base64Report = Convert.ToBase64String(report);
                await _jsRuntime.InvokeVoidAsync("downloadFileFromBase64", "application/pdf", "TourReport.pdf", base64Report);
            }
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