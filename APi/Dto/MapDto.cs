namespace Tourplanner.DTOs
{
    public class TileDto
    {
        public TileDto(string image, int x, int y)
        {
            Image = image;
            X = x;
            Y = y;
        }
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    
    public class MapDto
    {
        public MapDto(int id, List<TileDto> tiles, int tourId)
        {
            Id = id;
            Tiles = tiles;
            TourId = tourId;
        }
        public int Id { get; set; }
        public List<TileDto> Tiles { get; set; }
        public int TourId { get; set; }
    }
}

