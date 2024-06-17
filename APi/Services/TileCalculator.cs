using Tourplanner.Entities;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Repositories;

namespace Tourplanner.Services
{

    public record TileConfig(int Zoom, int X, int Y);
    public interface ITileCalculator
    {
        public (int xtile, int ytile) LatLonToTileNumbers(double lat, double lon, int zoom);
        public List<TileConfig> GetTileConfigs(int zoom, Bbox bbox);
    }

    /**
     * TileService bekommt Koordinaten "start" & "destination"
     * 
    */

    public class TileCalculator : ITileCalculator
    {

        public (int xtile, int ytile) LatLonToTileNumbers(double lat, double lon, int zoom)
        {
            double latRad = DegToRad(lat);
            double n = Math.Pow(2.0, zoom);
            int xtile = (int)((lon + 180.0) / 360.0 * n);
            int ytile = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);
            return (xtile, ytile);
        }

        public List<TileConfig> GetTileConfigs(int zoom, Bbox bbox)
        {
            var (bbox1, bbox2, bbox3, bbox4) = bbox;
            var configs = new List<TileConfig>();
            var topLeft = LatLonToTileNumbers(bbox4, bbox1, zoom);
            var bottomRight = LatLonToTileNumbers(bbox2, bbox3, zoom);

            for (var x = topLeft.xtile; x <= bottomRight.xtile; x++)
            {
                for (var y = topLeft.ytile; y <= bottomRight.ytile; y++)
                {
                    TileConfig config = new TileConfig(zoom, x, y);
                    configs.Add(config);
                }
            }

            return configs;
        }

        private double DegToRad(double deg)
        {
            return deg * (Math.PI / 180.0);
        }


    }
}