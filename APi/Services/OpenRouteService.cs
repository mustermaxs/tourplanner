using System.Text.Json;
using System.Text.Json.Nodes;
using Tourplanner.Models;

namespace Tourplanner.Services
{
    public record Coordinates(double Longitude, double Lattitude);

    public record RouteInfo(Coordinates From, Coordinates To, float Duration, float Distance, TransportType TransportType);


    public interface IOpenRouteService
    {
        public Task<JsonObject> GetAutoCompleteSuggestions(string query);
        public Task<RouteInfo> GetRouteInfo(Coordinates from, Coordinates to, TransportType transportType);

    }
    public class OpenRouteService : IOpenRouteService
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string? _routeInfoUrl;


        private string _autoCompleteUri { get; set; }
        private string _apiKey { get; set; }

        public OpenRouteService(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
            _apiKey = _configuration["OpenRouteService:ApiKey"];
            _autoCompleteUri = _configuration["OpenRouteService:Uri:AutoComplete"];
            _baseUrl = _configuration["OpenRouteService:BaseUrl"];
            _routeInfoUrl = _configuration["OpenRouteService:Uri:RouteInfo"];
        }

        public async Task<JsonObject> GetAutoCompleteSuggestions(string query)
        {
            return await _httpService.Get<JsonObject>($"{_baseUrl}{_autoCompleteUri}api_key={_apiKey}&text={query}");
        }

        public async Task<RouteInfo> GetRouteInfo(Coordinates from, Coordinates to, TransportType transportType)
        {
            string profile = transportType switch
            {
                TransportType.Car => "driving-car",
                TransportType.Bicycle => "cycling-regular",
                TransportType.Walking => "foot-walking",
                TransportType.Hiking => "foot-hiking",
                TransportType.Crawling => "wheelchair",  // TODO Updated to Wheelchair
                _ => "driving-car"
            };
            // TODO write helper method to build uri
            var json = await _httpService.Get<JsonElement>($"{_baseUrl}{_routeInfoUrl}{profile}?api_key={_apiKey}&start={from.Longitude},{from.Lattitude}&end={to.Longitude},{to.Lattitude}");

            var feature = json.GetProperty("features").EnumerateArray().ElementAt(0);

                var jsonCoordinates = feature.GetProperty("geometry").GetProperty("coordinates").EnumerateArray();
                var properties = feature.GetProperty("properties");
                var summary = properties.GetProperty("summary");
                var distance = (float)summary.GetProperty("distance").GetDouble();
                var duration = (float)summary.GetProperty("duration").GetDouble();

            var routeInfo = new RouteInfo(
                from,
                to,
                duration,
                distance,
                transportType
            );
        }
    }
}