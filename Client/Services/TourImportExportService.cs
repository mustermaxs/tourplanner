namespace Client.Services;
using Client.Utils;
using System.Net.Http;
using Client.Exceptions;
using Microsoft.JSInterop;

public class TourImportExportService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private string baseUrl { get; set; }

    public TourImportExportService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        baseUrl = StaticResService.GetApiBaseUrl;
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
    }



    public async Task ExportToJsonFile(int tourId)
    {
        try
        {
            var tour = _httpClient.GetByteArrayAsync($"{baseUrl}/Tours/{tourId}/export");
            var base64Tour = Convert.ToBase64String(await tour);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromBase64", "application/json", $"tour_{tourId}.json", base64Tour);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new UserActionException($"Failed to export Tour {tourId}");
        }
    }
}