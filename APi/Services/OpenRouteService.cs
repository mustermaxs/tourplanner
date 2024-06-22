using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Api.Services.Logging;
using Microsoft.VisualStudio.TextTemplating;
using Tourplanner.DTOs;
using Tourplanner.Entities.Tours;
using Tourplanner.Entities;
using Tourplanner.Models;
using Api.Services.Logging;
using Tourplanner.Exceptions;
using LoggerFactory = Api.Services.Logging.LoggerFactory;

namespace Tourplanner.Services
{
    public class GetGeoAutoCompleteSuggestionDto
    {
        public Dictionary<string, string> Properties { get; set; }
    }

    public interface IOpenRouteService
    {
        public Task<OrsBaseDto> GetAutoCompleteSuggestions(string query);
        public Task<Summary> RouteInfo(Coordinates from, Coordinates to, TransportType transportType);
    }

    public class OpenRouteService : IOpenRouteService
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string? _routeInfoUrl;
        protected ILoggerWrapper Logger = LoggerFactory.GetLogger();


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


        public async Task<OrsBaseDto> GetAutoCompleteSuggestions(string query)
        {
            return await _httpService.Get<OrsBaseDto>($"{_baseUrl}{_autoCompleteUri}api_key={_apiKey}&text={query}");
        }

        private string CoordinateToString(double coordinate) => coordinate.ToString().Replace(",", ".");

        private string BuildRouteInfoUrl(
            string profile,
            Coordinates from,
            Coordinates to,
            TransportType transportType) =>
            $"{_baseUrl}{_routeInfoUrl}{profile}?api_key={_apiKey}&start={CoordinateToString(from.Latitude)},{CoordinateToString(from.Longitude)}&end={CoordinateToString(to.Latitude)},{CoordinateToString(to.Longitude)}";

        public async Task<Summary> RouteInfo(Coordinates from, Coordinates to, TransportType transportType)
        {
            string profile = transportType switch
            {
                TransportType.Car => "driving-car",
                TransportType.Bicycle => "cycling-regular",
                TransportType.Walking => "foot-walking",
                TransportType.Hiking => "foot-hiking",
                TransportType.Wheelchair => "wheelchair",
                _ => "driving-car"
            };

            try
            {
                var json = await _httpService.Get<string>(
                    $"{_baseUrl}{_routeInfoUrl}{profile}?api_key={_apiKey}&start={CoordinateToString(from.Latitude)},{CoordinateToString(from.Longitude)}&end={CoordinateToString(to.Latitude)},{CoordinateToString(to.Longitude)}");

                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                // Extract the bounding box
                JsonElement bboxElement = root.GetProperty("bbox");
                double[] bbox = new double[bboxElement.GetArrayLength()];
                for (int i = 0; i < bboxElement.GetArrayLength(); i++)
                {
                    bbox[i] = bboxElement[i].GetDouble();
                }

                Console.WriteLine("Bounding Box: " + string.Join(", ", bbox));

                // Get the first feature
                JsonElement feature = root.GetProperty("features")[0];

                // Extract properties and segments
                JsonElement properties = feature.GetProperty("properties");
                JsonElement segments = properties.GetProperty("segments")[0];

                // Extract distance and duration
                float distance = (float)segments.GetProperty("distance").GetDouble();
                float duration = (float)segments.GetProperty("duration").GetDouble();

                Console.WriteLine("Distance: " + distance);
                Console.WriteLine("Duration: " + duration);

                return new Summary(
                    new Bbox(bbox[0], bbox[1], bbox[2], bbox[3]),
                    distance,
                    duration
                );
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while getting route info from OpenRouteService. {ex}");
                throw new DataAccessLayerException(ex.Message, ex);
            }
        }
    }
}