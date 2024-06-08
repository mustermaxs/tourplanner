

namespace Tourplanner.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Tourplanner.Entities.Tours;
    using Tourplanner.DTOs;

    public class Tile
    {
        public int Id { get; set; }
        public string Base64Encoded { get; set; }
        public int Order { get; set; }
        public int MapId { get; set; }
        public Map Map { get; set; }
        public int Zoom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Path { get; set; }
    }
    
    public static class TileExtensions
    {
        public static TileDto ToTileDto(this Tile tile)
        {
            return new TileDto(
                image: tile.Base64Encoded,
                x: tile.X,
                y: tile.Y
                );
        }
    }

    public class Bbox
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }

        public Bbox(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public Bbox()
        {
            
        }
        
        public void Deconstruct(out double minX, out double minY, out double maxX, out double maxY)
        {
            minX = MinX;
            minY = MinY;
            maxX = MaxX;
            maxY = MaxY;
        }
    }
    
    public class Map
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public int Zoom { get; set; }
        public Bbox Bbox { get; set; }
        public ICollection<Tile> Tiles { get; set; }
    }

    public static class MapExtensions
    {
        public static MapDto ToMapDto(this Map map)
        {
            return new MapDto(
                id: map.Id,
                tileDtos: map.Tiles.Select(t => t.ToTileDto()).ToList(),
                tourId: map.TourId
                );
        }
    }

}