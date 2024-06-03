using Tourplanner.Entities.TourLogs;
using Tourplanner.Repositories;

namespace Tourplanner.Services
{
    public interface ITileService
    {
        public List<string> GetTileUrls(int zoom, double bbox1, double bbox2, double bbox3, double bbox4);
    }

    public class TileService : ITileService
    {
        public List<string> GetTileUrls(int zoom, double bbox1, double bbox2, double bbox3, double bbox4)
        {
            var tileConfigs = GetTileConfigs(zoom, bbox1, bbox2, bbox3, bbox4);

            return tileConfigs.Select(tile => $"https://tile.openstreetmap.org/{tile.Zoom}/{tile.X}/{tile.Y}.png").ToList();
        }

        public (int xtile, int ytile) LatLonToTileNumbers(double lat, double lon, int zoom)
        {
            double latRad = DegToRad(lat);
            double n = Math.Pow(2.0, zoom);
            int xtile = (int)((lon + 180.0) / 360.0 * n);
            int ytile = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);
            return (xtile, ytile);
        }

        public List<(int Zoom, int X, int Y)> GetTileConfigs(int zoom, double bbox1, double bbox2, double bbox3,
            double bbox4)
        {
            var configs = new List<(int Zoom, int X, int Y)>();
            var topLeft = LatLonToTileNumbers(bbox4, bbox1, zoom);
            var bottomRight = LatLonToTileNumbers(bbox2, bbox3, zoom);

            for (var x = topLeft.xtile; x <= bottomRight.xtile; x++)
            {
                for (var y = topLeft.ytile; y <= bottomRight.ytile; y++)
                {
                    configs.Add((zoom, x, y));
                }
            }

            return configs;
        }


        public List<(int Zoom, int X, int Y)> GetTileConfigs(List<double> bbox, int zoom)
        {
            return GetTileConfigs(
                zoom,
                bbox.ElementAt(0),
                bbox.ElementAt(1),
                bbox.ElementAt(2),
                bbox.ElementAt(3)
            );
        }

        private double DegToRad(double deg)
        {
            return deg * (Math.PI / 180.0);
        }
    }
}