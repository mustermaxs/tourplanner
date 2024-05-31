using Client.Models;
using Client.Dao;
using Microsoft.JSInterop;
using System.Net.Http;
using System.IO;

public class ToursPageViewModel
{
    private ITourDao _tourDao;
    private IJSRuntime _jsRuntime;
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();

    public ToursPageViewModel(ITourDao tourDao, IJSRuntime jsRuntime)
    {
        _tourDao = tourDao;
        _jsRuntime = jsRuntime;
    }

    public async Task DownloadReportAsync()
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                //TODO: make not idiot way to get the report
                var report = await httpClient.GetByteArrayAsync("http://localhost:5161/api/reports/tours");

                var base64Report = Convert.ToBase64String(report);
                await _jsRuntime.InvokeVoidAsync("downloadFileFromBase64", "application/pdf", "ToursReport.pdf", base64Report);
            }
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
