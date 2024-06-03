using System.Buffers.Text;
using System.Composition.Convention;
using Tourplanner.Exceptions;
using Tourplanner.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Tourplanner.Repositories
{
    using System.IO;

    public class Tile
    {
        public int TourId { get; set; }
        public int Id { get; set; }
        public string FileSystemPath { get; set; }
        public string Base64Encoded { get; set; }
        public (double box1, double box2, double box3, double box4) Bbox { get; set; }
        public int Zoom { get; set; }

        public Tile(string fsPath, string url, string base64)
        {
            FileSystemPath = fsPath;
            Base64Encoded = base64;
        }
    }

    public class FileSystemHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _appRootDir;
        private readonly HttpClient _httpClient;

        public FileSystemHandler(IWebHostEnvironment webHostEnvironment, IHttpService httpService)
        {
            _webHostEnvironment = webHostEnvironment;
            _appRootDir = _webHostEnvironment.ContentRootPath;
            _httpClient = httpService.GetClient();
        }
        public async Task DownloadImageFromUrl(string savePath, string imageUrl)
        {
                HttpResponseMessage response = await _httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                byte[] imageData = await response.Content.ReadAsByteArrayAsync();

                await File.WriteAllBytesAsync(Path.Combine(_appRootDir, savePath), imageData);
        }
        
        public string ReadImageAsBase64(string filePath)
        {
            if (!FileExists(Path.Combine(_appRootDir, filePath)))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }

            byte[] imageBytes = File.ReadAllBytes(System.IO.Path.Combine(_appRootDir, filePath));
            return Convert.ToBase64String(imageBytes);
        }
        
        public string[] GetAllFilePaths(string directoryPath, bool includeSubdirectories = false)
        {
            if (!DirExists(System.IO.Path.Combine(_appRootDir, directoryPath)))
            {
                throw new DirectoryNotFoundException("Directory not found: " + directoryPath);
            }

            return Directory.GetFiles(System.IO.Path.Combine(_appRootDir, directoryPath), "*.*", 
                includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public bool DirExists(string path)
        {
            return Directory.Exists(System.IO.Path.Combine(_appRootDir, path));
        }

        public bool FileExists(string path)
        {
            return File.Exists(System.IO.Path.Combine(_appRootDir, path));
        }

        public void CreateDir(string path)
        {
            // if (!DirExists(path))
            // {
                Directory.CreateDirectory(System.IO.Path.Combine(_appRootDir, path));
            // }
        }
    }

    public interface ITileContext
    {
        public Task Save(Tile tile);
        public Task Delete(Tile tile);
        public List<Tile> Get(int tourId);
    }

    public class TileContext
        : ITileContext
    {
        private string baseDir;
        private readonly FileSystemHandler _fsHandler;
        private readonly ITileService tileService;

        public TileContext(FileSystemHandler _fsHandler, IConfiguration configuration, ITileService _tileService)
        {
            this._fsHandler = _fsHandler;
            tileService = _tileService;
            baseDir = configuration["Tiles:BaseDir"];
        }
        public async Task Save(Tile tile)
        {
            try
            {
                var (box1, box2, box3, box4) = tile.Bbox;
                var urls = tileService.GetTileUrls(tile.Zoom, box1, box2, box3, box4);
                var tileDirName = CreateTileDirName(tile.TourId);

                // if (_fsHandler.DirExists(tileDirName)) throw new CreateTourException("Tiles already exist");

                _fsHandler.CreateDir(tileDirName);
                var index = 0;
                
                foreach (var url in urls)
                {
                    await _fsHandler.DownloadImageFromUrl($"{tileDirName}/{index}.png", url);
                    ++index;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task Delete(Tile tile)
        {
            throw new NotImplementedException();
        }

        public List<Tile> Get(int tourId)
        {
            if (!_fsHandler.DirExists(CreateTileDirName(tourId)))
                throw new ResourceNotFoundException("Tile dir doesn't exist");

            var paths = _fsHandler.GetAllFilePaths($"{CreateTileDirName(tourId)}");

            return paths.Select((p) =>
            {
                string fileName = p.Substring(p.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                string fileNameWithoutExtension = fileName.Replace(".png", "");
                var tile = new Tile(p, "", _fsHandler.ReadImageAsBase64(p));
                tile.Id = Int32.Parse(fileNameWithoutExtension);
                return tile;
            }).ToList();
        }

        private string CreateTileDirName(int tourId) => $"{baseDir}/{tourId}";
    }

    public interface ITileRepository : IRepository<Tile>
    {
    }

    public class TileRepository(TourContext ctx, ITileContext tileContext) : Repository<Tile>(ctx), ITileRepository
    {
        public override async Task Create(Tile tile)
        {
            tileContext.Save(tile);
        }
    }
}