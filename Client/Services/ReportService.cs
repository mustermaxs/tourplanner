using Client.Utils;
using System.Net.Http;
using Microsoft.JSInterop;


public interface IReportService
{
    public Task GetTourReport(int tourId);
    public Task GetSummaryReport();
}

public class ReportService : IReportService
{
    private String baseUrl;
    private HttpClient client;

    private IJSRuntime _jsRuntime;

    public ReportService(HttpClient http, IJSRuntime jsRuntime)
    {
        baseUrl = StaticResService.GetApiBaseUrl;
        client = http;
        client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
        _jsRuntime = jsRuntime;
    }

    public async Task GetTourReport(int tourId)
    {
        try {
        
            var report = await client.GetByteArrayAsync($"{baseUrl}reports/tours/{tourId}");

            var base64Report = Convert.ToBase64String(report);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromBase64", "application/pdf", "TourReport.pdf", base64Report);
        }   
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading report: {ex.Message}");
            throw;
        }
    }

    public async Task GetSummaryReport()
    {
        try {
            var report = await client.GetByteArrayAsync($"{baseUrl}reports/tours/");

            var base64Report = Convert.ToBase64String(report);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromBase64", "application/pdf", "SummaryReport.pdf", base64Report);
        }   
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading report: {ex.Message}");
            throw;
        }
    }
}