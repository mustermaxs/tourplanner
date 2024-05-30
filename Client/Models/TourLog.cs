using Client.Dao;
using Client.Models;

namespace Client.Models
{
    public class TourLog
    {
        public TourLog()
        {
        }
        public TourLog(int id, DateTime dateTime, string comment, float difficulty, float duration, float rating, Tour tour)
        {
            Id = id;
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            Duration = duration;
            Rating = rating;
            Tour = tour;
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