using Client.Dao;

namespace Client.Services;

using Client.Utils;
using System.Net.Http;
using Client.Exceptions;
using Microsoft.JSInterop;

public class TourImportExportService
{
    private IFileTransferService _fileTransferService;
    private string baseUrl = StaticResService.GetApiBaseUrl;
    
    public TourImportExportService(IFileTransferService fileTransferService)
    {
        _fileTransferService = fileTransferService;
    }

    public async Task ExportToJsonFile(int tourId)
    {
        try
        {
            await _fileTransferService.DownloadFile($"{baseUrl}Tours/{tourId}/export", "application/json",
                $"Tour_{tourId}.json");
        }
        catch (Exception e)
        {
            throw new UserActionException($"Failed to export Tour {tourId}");
        }
    }
}