using Client.Exceptions;
using Client.Utils;
using Microsoft.JSInterop;

namespace Client.Dao
{
    public interface IFileTransferService
    {
        Task UploadFile(string filePath, byte[] bytes);
        Task DownloadFile(string fileUrl, string mimeType, string nameForDownloadedFile);
    }
    
    public class FileTransferService : IFileTransferService
    {
        private string _baseUrl { get; set; }
        private HttpClient _httpClient { get; set; }
        private IJSRuntime _jsRuntime { get; set; }
        
        public FileTransferService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _baseUrl = StaticResService.GetApiBaseUrl;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
        }



        public Task UploadFile(string filePath, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public async Task DownloadFile(string fileUrl, string mimeType, string nameForDownloadedFile)
        {
            try
            {
                var fileAsByteArray = _httpClient.GetByteArrayAsync(fileUrl);
                var fileAsBase64 = Convert.ToBase64String(await fileAsByteArray);
                await _jsRuntime.InvokeVoidAsync("downloadFileFromBase64", mimeType, nameForDownloadedFile, fileAsBase64);
            }
            catch (Exception e)
            {
                throw new DaoException($"Failed to download file {nameForDownloadedFile}.");
            }
        }
    }
}