using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.TextTemplating;
using Tourplanner.DTOs;
using Tourplanner.Entities.Tours;
using Tourplanner.Models;

namespace Tourplanner.Services
{
    public class GetGeoAutoCompleteSuggestionDto
    {
        public Dictionary<string, string> Properties { get; set; }
        
    }

    public interface IOpenRouteService
    {
        public Task<OrsBaseDto> GetAutoCompleteSuggestions(string query);
        public Task<OrsRouteSummary> RouteInfo(Coordinates from, Coordinates to, TransportType transportType);

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


        public async Task<OrsBaseDto> GetAutoCompleteSuggestions(string query)
        {
            return await _httpService.Get<OrsBaseDto>($"{_baseUrl}{_autoCompleteUri}api_key={_apiKey}&text={query}");
        }

        private string CoordinateToString(double coordinate) =>  coordinate.ToString().Replace(",", ".");

        public async Task<OrsRouteSummary> RouteInfo(Coordinates from, Coordinates to, TransportType transportType)
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
            var json = await _httpService.Get<OrsRouteSummary>(
                $"{_baseUrl}{_routeInfoUrl}{profile}?api_key={_apiKey}&start={CoordinateToString(from.Latitude)},{CoordinateToString(from.Longitude)}&end={CoordinateToString(to.Latitude)},{CoordinateToString(to.Longitude)}");

            https://api.openrouteservice.org /v2/directions/driving-car? api_key = your-api-key& start = 8.681495,49.41461& end = 8.687872,49.420318
            return json;
        }
    }
}