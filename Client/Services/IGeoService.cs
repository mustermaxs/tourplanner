using System.Text.Json;
using Client.Models;
using Client.DTOs;

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
            // var features = json.GetProperty("features");
            //
            // foreach (JsonElement feature in features.EnumerateArray())
            // {
            //     var jsonCoordinates = feature.GetProperty("geometry").GetProperty("coordinates").EnumerateArray();
            //     var longitude = jsonCoordinates.ElementAt(0).GetDouble();
            //     var lattitute = jsonCoordinates.ElementAt(1).GetDouble();
            //     var coordinates = new Coordinates(longitude, lattitute);
            //     var properties = feature.GetProperty("properties");
            //     try
            //     {
            //         locations.Add(
            //             new Location(
            //                 properties.GetProperty("label").GetString(),
            //                 coordinates
            //             )
            //         );
            //     }
            //     catch (KeyNotFoundException e)
            //     {
            //         Console.WriteLine(e);
            //         throw;
            //     }
            // }

            // return locations;
        }

    }
}