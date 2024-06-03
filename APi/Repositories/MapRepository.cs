// using System.Buffers.Text;
// using System.Composition.Convention;
// using Tourplanner.Exceptions;
// using Tourplanner.Services;
// using Microsoft.Extensions.Hosting;
// using Microsoft.AspNetCore.Hosting;
// using Tourplanner.Entities;
//
// namespace Tourplanner.Repositories
// {
//     using System.IO;
//     public class FileSystemHandler
//     {
//         private readonly IWebHostEnvironment _webHostEnvironment;
//         private readonly string _appRootDir;
//         private readonly HttpClient _httpClient;
//
//         public FileSystemHandler(IWebHostEnvironment webHostEnvironment, HttpClient client)
//         {
//             _webHostEnvironment = webHostEnvironment;
//             _appRootDir = _webHostEnvironment.ContentRootPath;
//             _httpClient = client;
//         }
//
//         public async Task DownloadImageFromUrl(string savePath, string imageUrl)
//         {
//             HttpResponseMessage response = await _httpClient.GetAsync(imageUrl);
//             response.EnsureSuccessStatusCode();
//
//             byte[] imageData = await response.Content.ReadAsByteArrayAsync();
//
//             await File.WriteAllBytesAsync(Path.Combine(_appRootDir, savePath), imageData);
//         }
//
//         public string ReadImageAsBase64(string filePath)
//         {
//             if (!FileExists(Path.Combine(_appRootDir, filePath)))
//             {
//                 throw new FileNotFoundException("File not found.", filePath);
//             }
//
//             byte[] imageBytes = File.ReadAllBytes(System.IO.Path.Combine(_appRootDir, filePath));
//             return Convert.ToBase64String(imageBytes);
//         }
//
//         public string[] GetAllFilePaths(string directoryPath, bool includeSubdirectories = false)
//         {
//             if (!DirExists(System.IO.Path.Combine(_appRootDir, directoryPath)))
//             {
//                 throw new DirectoryNotFoundException("Directory not found: " + directoryPath);
//             }
//
//             return Directory.GetFiles(System.IO.Path.Combine(_appRootDir, directoryPath), "*.*",
//                 includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
//         }
//
//         public bool DirExists(string path)
//         {
//             return Directory.Exists(System.IO.Path.Combine(_appRootDir, path));
//         }
//
//         public bool FileExists(string path)
//         {
//             return File.Exists(System.IO.Path.Combine(_appRootDir, path));
//         }
//
//         public void CreateDir(string path)
//         {
//             Directory.CreateDirectory(System.IO.Path.Combine(_appRootDir, path));
//         }
//     }
//
//     public interface ITileContext
//     {
//         public Task<int> Save(Map map);
//         public Task Delete(Map map);
//         public Map Get(int tourId);
//     }
//
//     public class TileContext
//         : ITileContext
//     {
//         private string baseDir;
//         private readonly FileSystemHandler _fsHandler;
//         private readonly ITileService tileService;
//
//         public TileContext(FileSystemHandler fsHandler, IConfiguration configuration, ITileService _tileService)
//         {
//             _fsHandler = fsHandler;
//             tileService = _tileService;
//             baseDir = configuration["Tiles:BaseDir"];
//         }
//
//         public async Task Save(Map map)
//         {
//             try
//             {
//                 var (minX, minY, maxX, maxY) = map.Bbox;
//                 var urls = tileService.GetTileUrls(map.Zoom, minX, minY, maxX, maxY);
//                 var tileDirName = CreateTileDirName(map.TourId);
//
//                 if (_fsHandler.DirExists(tileDirName)) throw new CreateTourException("Tiles already exist");
//
//                 _fsHandler.CreateDir(tileDirName);
//                 var index = 0;
//
//                 foreach (var url in urls)
//                 {
//                     await _fsHandler.DownloadImageFromUrl($"{tileDirName}\\{index}.png", url);
//                     ++index;
//                 }
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e);
//                 throw;
//             }
//         }
//
//         public Task Delete(Map map)
//         {
//             throw new NotImplementedException();
//         }
//
//         public Map Get(int tourId)
//         {
//             if (!_fsHandler.DirExists(CreateTileDirName(tourId)))
//                 throw new ResourceNotFoundException("Tile dir doesn't exist");
//
//             var paths = _fsHandler.GetAllFilePaths($"{CreateTileDirName(tourId)}");
//
//             var tiles = paths.Select((p) =>
//             {
//                 string fileName = Path.GetFileName(p);
//                 string fileNameWithoutExtension = fileName.Replace(".png", "");
//                 var tile = new Tile(p, "", _fsHandler.ReadImageAsBase64(p));
//                 var tile = 
//                 tile.Id = Int32.Parse(fileNameWithoutExtension);
//                 return tile;
//             }).ToList();
//
//             var map = new Map(
//                 0,
//                 
//                 );
//
//         }
//
//         private string CreateTileDirName(int tourId) => Path.Combine(baseDir, tourId.ToString());
//     }
//
//     public interface IMapRepository : IRepository<Map>
//     {
//     }
//
//     // TODO Map besteht ja aus mehreren Tiles, sollte also vlt eher
//     // MapRepository heißen (und MapContext(?))
//     // in Datenbank Pfade und Assoziationen Map -> TileS (1:n) speicher
//     // Map
//     //  List<Tiles>
//     //  Bbox Bbox
//     //  Zoom
//     //
//     // Tile
//     //  Base64Encoded
//     //  
//     //  
//     
//     /*
//          * TODO CREATE TOUR WITH MAP
//          * Handle within CreateTourCommand
//          * CreateTourCommand uses other commands and repositories itself
//      * 
//      * Create Tour  ->  return TourId
//      * Fetch Images  ->  return image names
//      *      store them with the image name they got from the OSM API
//      * Create Map   ->  return MapId
//      * Create Tiles ->  return TileIds
//      *  
//      * 
//      */
//     public class MapRepository(TourContext ctx, ITileContext tileContext) : Repository<Map>(ctx), IMapRepository
//     {
//         public override async Task Create(Map map)
//         {
//             tileContext.Save(map);
//         }
//     }
// }