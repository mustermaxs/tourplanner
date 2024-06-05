using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tours;

namespace Tourplanner.Entities
{

    public class Tile
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int MapId { get; set; }
        public Map Map { get; set; }
        public int Zoom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Bbox
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        
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
        public string Path { get; set; }
        public int Zoom { get; set; }
        public Bbox Bbox { get; set; }
        public ICollection<Tile> Tiles { get; set; }
    }

}