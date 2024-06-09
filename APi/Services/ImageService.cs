namespace Tourplanner.Services
{
    using Tourplanner.Exceptions;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;

    public interface IImageService
    {
        public Task<string> FetchImageFromUrl(string savePath, TileConfig tileConfig);
        public Task<List<string>> FetchImages(string savePath, List<TileConfig> tileConfigs);
        public string ReadImageAsBase64(string filePath);
    }

    public class ImageService : IImageService
    {
        private HttpClient httpClient;
        private string baseDir;

        public ImageService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            httpClient = httpClientFactory.CreateClient("TourPlannerClient");
            baseDir = configuration["Tiles:BaseDir"];
        }

        public async Task<string> FetchImageFromUrl(string savePath, TileConfig tileConfig)
        {
            var filePath = Path.Combine(baseDir, savePath, CreateImageName(tileConfig));

            HttpResponseMessage response =
                await httpClient.GetAsync(
                    $"https://tile.openstreetmap.org/{tileConfig.Zoom}/{tileConfig.X}/{tileConfig.Y}.png");

            if (!response.IsSuccessStatusCode) throw new ResourceNotFoundException("Failed to fetch image");

            byte[] image = await response.Content.ReadAsByteArrayAsync();


            await File.WriteAllBytesAsync(filePath, image);

            return filePath;
        }

        public async Task<List<string>> FetchImages(string savePath, List<TileConfig> tileConfigs)
        {
            var newTiles = RemoveAlreadyExistingImages(in tileConfigs, savePath);
            
            // if they already exist don't download them again and return the paths to the already existing ones
            if (!newTiles.Any())
                return tileConfigs.Select(t => Path.Combine(baseDir, savePath, CreateImageName(t))).ToList();

            var tasks = newTiles.Select(t => FetchImageFromUrl(savePath, t));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        private List<TileConfig> RemoveAlreadyExistingImages(in List<TileConfig> tileConfigs, string savePath)
        {
            return tileConfigs.Where(t => !File.Exists((Path.Combine(baseDir, savePath, CreateImageName(t))))).ToList();
        }

        public string ReadImageAsBase64(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Tile image not found.", filePath);
            }

            byte[] imageBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageBytes);
        }

        protected string CreateImageName(TileConfig tileconfig) =>
            $"{tileconfig.Zoom}-{tileconfig.X}-{tileconfig.Y}.png";
    }
}