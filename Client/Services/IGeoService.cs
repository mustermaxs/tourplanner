using System.Text.Json;
using Client.Models;
using Client.Dtos;

namespace Client.Services
{

    public interface IGeoService
    {
        public Task<OrsBaseDto> SearchLocation(string location);
    }

    public class GeoService : IGeoService
    {
        private readonly IHttpService _httpService;

        public GeoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Location? GetLocationFromLabel(string label, IEnumerable<Location> locations)
        {
            return locations.SingleOrDefault(l => l.Label == label);
        }

        public async Task<OrsBaseDto> SearchLocation(string location)
        {
            var locations = new List<Location>();
            var json = await _httpService.Get<OrsBaseDto>($"Tours/geosuggestion?location={location}");
            return json;
        }

    }
}