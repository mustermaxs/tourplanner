using Tourplanner.Entities.TourLogs;
using Tourplanner.Repositories;

namespace Tourplanner.Services
{
    public interface ITilesService
    {
        public string GetTileUrl(int xCoord, int yCoord, int zoom);
    }
    public class TilesService : ITilesService
    {
        public string GetTileUrl(int xCoord, int yCoord, int zoom)
        {
            (int xTile, int yTile) = LatLonToTileNumbers(xCoord, yCoord, zoom);

            return $"https://tile.openstreetmap.org/{zoom}/{xTile}/{yTile}.png";
        }

        private (int xtile, int ytile) LatLonToTileNumbers(double lat, double lon, int zoom)
        {
            double latRad = DegToRad(lat);
            double n = Math.Pow(2.0, zoom);
            int xtile = (int)((lon + 180.0) / 360.0 * n);
            int ytile = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);
            return (xtile, ytile);
        }

        private double DegToRad(double deg)
        {
            return deg * (Math.PI / 180.0);
        }
        
    }
}