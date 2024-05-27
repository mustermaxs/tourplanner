using Client.Dao;
using Client.Models;

namespace Client.Models
{
    public class TourLog
    {
        public TourLog()
        {
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; } = "";
        public float Difficulty { get; set; } = 0;
        public float Duration { get; set; } = 0;
        public float Rating { get; set; } = 0;
        public Tour Tour { get; set; } = new Tour();
    }
}