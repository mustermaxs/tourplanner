namespace Tourplanner.DTOs
{
    public class TourLogDto
    {
        public TourLogDto(
            int id,
            int tourId,
            DateTime dateTime,
            string comment,
            float difficulty,
            double rating,
            TourDto tour,
            float duration,
            float distance
        )
        {
            Id = id;
            TourId = tourId;
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            Rating = rating;
            Tour = tour;
            Duration = duration;
            Distance = distance;
        }

        public int Id { get; private set; }
        public int TourId { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Comment { get; private set; }
        public float Difficulty { get; private set; }
        public double Rating { get; private set; }
        public TourDto Tour {get; set;}
        public float Duration { get; private set;}
        public float Distance { get; private set; }
    }
}
