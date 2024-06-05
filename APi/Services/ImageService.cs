namespace Tourplanner.Services
{
    using Tourplanner.Exceptions;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;

    public interface IImageService
    {
        public Task<string> FetchImageFromUrl(string savePath, TileConfig tileConfig);
        public  Task<List<string>> FetchImages(string savePath, List<TileConfig> tileConfigs);
        public string ReadImageAsBase64(string filePath);
    }

    public class ImageService : IImageService
    {
        private HttpClient httpClient;
        private string baseDir;

        public ImageService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
            baseDir = AppContext.BaseDirectory;
        }
        public async Task<string> FetchImageFromUrl(string savePath, TileConfig tileConfig)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://tile.openstreetmap.org/{tileConfig.Zoom}/{tileConfig.X}/{tileConfig.Y}.png");
            
            if (!response.IsSuccessStatusCode) throw new ResourceNotFoundException("Failed to fetch image");

            byte[] image = await response.Content.ReadAsByteArrayAsync();

            var filePath = Path.Combine(baseDir, savePath, CreateImageName(tileConfig));

            await File.WriteAllBytesAsync(filePath, image);

            return filePath;
        }

        public async Task<List<string>> FetchImages(string savePath, List<TileConfig> tileConfigs)
        {
            var tasks = tileConfigs.Select(t => FetchImageFromUrl(savePath, t));
            var results = await Task.WhenAll(tasks);
            return results.ToList();  
        }

        public string ReadImageAsBase64(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Tile not found.", filePath);
            }

            byte[] imageBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageBytes);
        }

        protected string CreateImageName(TileConfig tileconfig) => $"{tileconfig.Zoom}-{tileconfig.X}-{tileconfig.Y}.png";
    }
}