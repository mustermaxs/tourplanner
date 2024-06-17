namespace Client.Models
{
    public class Tile
    {
        public Tile(string image, int x, int y)
        {
            Image = image;
            X = x;
            Y = y;
        }
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Map
    {
        public Map(int id, List<Tile> tiles, int tourId)
        {
            Id = id;
            Tiles = tiles;
            TourId = tourId;
        }
        
        public Map() {}

        public int Id { get; set; }
        public List<Tile> Tiles { get; set; }
        public int TourId { get; set; }
    }
}