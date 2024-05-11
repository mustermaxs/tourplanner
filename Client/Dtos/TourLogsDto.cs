
    public class TourLogDto
    {
        public TourLogDto(
            int id,
            int tourId,
            DateTime dateTime,
            string comment,
            float difficulty,
            float totalTime,
            double rating
        )
        {
            Id = id;
            TourId = tourId;
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Rating = rating;
        }

        public int Id { get; private set; }
        public int TourId { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Comment { get; private set; }
        public float Difficulty { get; private set; }
        public float TotalTime { get; private set; }
        public double Rating { get; private set; }
    }