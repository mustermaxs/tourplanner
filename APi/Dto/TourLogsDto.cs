using System;

namespace Tourplanner.DTOs
{
    public class TourLogDto
    {
        public TourLogDto(
            int tourLogId,
            int tourId,
            DateTime dateTime,
            string comment,
            int difficulty,
            double totalDistance,
            TimeSpan totalTime,
            double rating
        )
        {
            TourLogId = tourLogId;
            TourId = tourId;
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            TotalDistance = totalDistance;
            TotalTime = totalTime;
            Rating = rating;
        }

        public int TourLogId { get; private set; }
        public int TourId { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Comment { get; private set; }
        public int Difficulty { get; private set; }
        public double TotalDistance { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public double Rating { get; private set; }
    }
}
