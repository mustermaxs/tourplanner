namespace Client.Models
{


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


}