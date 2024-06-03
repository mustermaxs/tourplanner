using System.Collections;
using System.Text.Json;
using Client.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tourplanner.Entities.Tours;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Exceptions;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tourplanner;

namespace UnitTests;

[TestFixture]
public class ChildFriendlinessServiceTests
{
    protected Mock<Microsoft.Extensions.Configuration.IConfiguration> mockConfiguration;
    protected Microsoft.Extensions.Configuration.IConfiguration _config;
    private Tourplanner.Services.IHttpService _httpService;
    private ServiceProvider? _serviceProvider;
    private TourContext? _context;
    private TourRepository? _repository;
    private IWebHostEnvironment _webHostEnv;

    [SetUp]
    public void Setup()
    {
        mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["OpenRouteService:ApiKey"])
            .Returns("5b3ce3597851110001cf624884fc562312594a7384ea835fc589d433");
        mockConfiguration.Setup(config => config["OpenRouteService:Uri:AutoComplete"]).Returns("geocode/search?");
        mockConfiguration.Setup(config => config["Tiles:BaseDir"]).Returns("Api/Tiles");
        mockConfiguration.Setup(config => config["OpenRouteService:BaseUrl"])
            .Returns("https://api.openrouteservice.org/");
        mockConfiguration.Setup(config => config["OpenRouteService:Uri:RouteInfo"]).Returns("v2/directions/");
        _config = mockConfiguration.Object;

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5280")
        };

        _httpService = new Tourplanner.Services.HttpService(httpClient);
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<TourContext>(options =>
            options.UseInMemoryDatabase("tourplanner_test"));

        serviceCollection.AddScoped<TourRepository>();

        _serviceProvider = serviceCollection.BuildServiceProvider();

        _context = _serviceProvider.GetRequiredService<TourContext>();
        _repository = _serviceProvider.GetRequiredService<TourRepository>();
        
        var _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _mockWebHostEnvironment.Setup(env => env.ContentRootPath).Returns("C:\\Users\\maxsi\\RiderProjects\\tourplanner");
        _webHostEnv = _mockWebHostEnvironment.Object;

    }

    [TearDown]
    public void TearDown()
    {
        _context!.Database.EnsureDeleted();
        _context.Dispose();
        _serviceProvider!.Dispose();
        _repository!.Dispose();
        _httpService.Dispose();
    }

    public void Dispose()
    {
        _context!.Database.EnsureDeleted();
        _context.Dispose();
        _serviceProvider!.Dispose();
    }

    [Test]
    public async Task Calculate_ReturnsNormalizedSum()
    {
        // Arrange
        var tour = new Tour { Id = 1, Distance = 50, EstimatedTime = 120 };
        IEnumerable<TourLog> logs = new[]
        {
            new TourLog { TourLogId = 1, TourId = 1, Difficulty = 3 },
            new TourLog { TourLogId = 2, TourId = 1, Difficulty = 4 }
        };
        var tourLogs = new List<TourLog>
        {
            new TourLog { TourLogId = 1, TourId = 1, Difficulty = 3 },
            new TourLog { TourLogId = 2, TourId = 1, Difficulty = 4 }
        };

        var moqTourRepo = new Mock<ITourRepository>();
        moqTourRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Tour> { tour });

        var moqLogRepo = new Mock<ITourLogRepository>();
        moqLogRepo.Setup(repo => repo.GetTourLogsForTour(1)).ReturnsAsync(tourLogs.AsEnumerable());


        var service = new ChildFriendlinessService(moqTourRepo.Object, moqLogRepo.Object);

        // Act
        var result = await service.Calculate(1);

        // Assert
        Console.WriteLine(result);
        Assert.That(10.0f == result); // Adjust the expected result based on your calculations
    }

    [Test]
    public void Calculate_ThrowsException_WhenTourNotFound()
    {
        // Arrange
        var moqTourRepo = new Mock<ITourRepository>();
        moqTourRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Tour>());

        var moqLogRepo = new Mock<ITourLogRepository>();

        var service = new ChildFriendlinessService(moqTourRepo.Object, moqLogRepo.Object);

        // Act & Assert
        Assert.ThrowsAsync<ResourceNotFoundException>(async () => await service.Calculate(1));
    }

    [Test]
    public async Task OpenRouteService_Gets_RouteInfo_For_Given_Route()
    {
        var from = new Tourplanner.Services.Coordinates(16.3021535, 48.2149546);
        var to = new Tourplanner.Services.Coordinates(16.3526723, 48.1993161);
        var service = new OpenRouteService(_httpService, _config);

        var res = await service.GetRouteInfo(from, to, TransportType.Walking);

        Assert.Multiple(() =>
        {
            Assert.That(res.TransportType == TransportType.Walking);
            Assert.That(res.Distance > 0);
            Assert.That(res.Duration > 0);
        });
    }

    [Test]
    public async Task OpenRouteService_Gets_AutoCompleteSuggestions()
    {
        var service = new OpenRouteService(_httpService, _config);
        var userInput = "wien";

        var suggestions = await service.GetAutoCompleteSuggestions(userInput);

        Assert.That(suggestions != null);
        // var features = suggestions.GetProperty("features");
        // var properties = suggestions.GetProperty("properties").EnumerateArray();
        // var 
    }

    [Test]
    public async Task OpenRouteService_Gets_RouteInfo()
    {
        var service = new OpenRouteService(_httpService, _config);
        var userInput = "wien";
        var from = new Tourplanner.Services.Coordinates(16.3021535, 48.2149546);
        var to = new Tourplanner.Services.Coordinates(16.3526723, 48.1993161);

        var routeInfo = await service.RouteInfo(from, to, TransportType.Walking);

        Assert.That(routeInfo != null);
        // var features = suggestions.GetProperty("features");
        // var properties = suggestions.GetProperty("properties").EnumerateArray();
        // var 
    }

    [Test]
    public async Task TileService_Returns_Expected_List_Of_Tiles()
    {
        var bbox = new List<double> { 16.376941, 48.239177, 16.377488, 48.240183 };
        var zoom = 17;
        var service = new TileService();

        var tiles = service.GetTileConfigs(
            17,
            16.376941, 48.239177, 16.377488, 48.240183);

        var tile1 = tiles.ElementAt(0);
        var tile2 = tiles.ElementAt(1);

        Assert.Multiple(() =>
        {
            Assert.That(tile1.X == 71498);
            Assert.That(tile1.Y == 45431);
            Assert.That(tile2.X == 71498);
            Assert.That(tile2.Y == 45432);
        });
        var tileContext = new TileContext(new FileSystemHandler(_webHostEnv, _httpService), _config, new TileService());
        var t = new Tile("", "", "");
        t.Bbox = ( 16.376941, 48.239177, 16.377488, 48.240183);
        t.Zoom = zoom;
        await tileContext.Save(t);
    }

    // [Test]
    // public async Task Repo_rolls_back()
    // {
    //     var tour = new Tour();
    //     tour.Name = "integration teset";
    //     tour.Distance = 100;
    //     tour.From = "blabla";
    //     tour.To = "blablabla";
    //
    //     _repository!.Create(tour);
    //     var dbSet = _context.Set<Tour>();
    //     var res = await dbSet.ToListAsync();
    //
    //     Assert.That(res != null && res.Any());
    // }
}