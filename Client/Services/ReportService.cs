using Client.Utils;
using System.Net.Http;
using Client.Dao;
using Client.Exceptions;
using Microsoft.JSInterop;


public interface IReportService
{
    public Task GetTourReport(int tourId);
    public Task GetSummaryReport();
}

public class ReportService(IFileTransferService fileTransferService) : IReportService
{
    private string _baseUrl = StaticResService.GetApiBaseUrl;
    public async Task GetTourReport(int tourId)
    {
        try
        {
            await fileTransferService.DownloadFile($"{_baseUrl}reports/tours/{tourId}", "application/pdf", $"Report_Tour_{tourId}.pdf");
        }
        catch (Exception ex)
        {
            throw new ServiceLayerException("Failed to download tour report");
        }
    }

    public async Task GetSummaryReport()
    {
        try {
            fileTransferService.DownloadFile($"{_baseUrl}reports/tours/", "application/pdf", "SummaryReport.pdf");
        }   
        catch (Exception ex)
        {
            throw new ServiceLayerException("Failed to download summary report", ex);
        }
    }
}