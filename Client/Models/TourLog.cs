using Client.Dao;
using Client.Models;
using Client.Dtos; // TODO fix namespaces
using Client.Dto;  // <- how did this happen, ? sup?

namespace Client.Models
{
    public class TourLog
    {
        public TourLog()
        {
        }
        public TourLog(int id, DateTime dateTime, string comment, float difficulty, float duration, float rating, Tour tour, float distance)
        {
            Id = id;
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            Duration = duration;
            Rating = rating;
            Tour = tour;
            Distance = distance;
        }
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; } = "";
        public float Difficulty { get; set; } = 0;
        public float Duration { get; set; } = 0;
        public float Rating { get; set; } = 0;
        public Tour Tour { get; set; } = new Tour();
        public float Distance { get; set; } = 0f;
    }

    public static class TourLogExtensions
    {
        public static CreateTourLogDto ToCreateTourLogDto(this TourLog tourLog)
        {
            return new CreateTourLogDto(
                tourLog.DateTime,
                tourLog.Comment,
                tourLog.Difficulty,
                tourLog.Duration,
                tourLog.Rating,
                tourLog.Distance            
            );
        }
        public static UpdateTourLogDto ToUpdateTourLogDto(this TourLog tourLog)
        {
            return new UpdateTourLogDto(
                id: tourLog.Id,
                comment: tourLog.Comment,
                difficulty: tourLog.Difficulty,
                dateTime: tourLog.DateTime,
                rating: tourLog.Rating,
                distance: tourLog.Distance,
                duration: tourLog.Duration
            );
        }
    }
}