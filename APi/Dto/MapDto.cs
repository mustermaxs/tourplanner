namespace Tourplanner.DTOs
{
    public class TileDto(string image, int x, int y)
    {
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    
    public class MapDto(int id, int tourId, List<TileDto> tileDtos)
    {
        public int Id { get; set; }
        public List<TileDto> Tiles { get; set; }
        public int TourId { get; set; }
    }
}

