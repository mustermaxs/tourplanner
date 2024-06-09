using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tourplanner.DTOs;
using Tourplanner.Entities.Tours;

namespace Tourplanner.Entities.TourLogs
{
    public class TourLog
    {
        public int TourLogId { get; set; }
        public float Difficulty { get; set; }
        public float Duration { get; set; }
        public float Rating { get; set; }
    
        [MaxLength(500)]
        public string Comment { get; set; } = String.Empty;
        public int TourId { get; set; }

        public Tour Tour { get; set; } = null!;

        public DateTime DateTime { get; set; }
        public float Distance {get; set;}
    }

    public static class TourLogExtensionMethods
    {
        public static TourLogDto ToTourLogDto(this TourLog log)
        {
            return new TourLogDto(
                log.TourLogId,
                log.TourId,
                log.DateTime,
                log.Comment,
                log.Difficulty,
                log.Rating,
                log.Tour.ToTourDto(),
                log.Duration,
                log.Distance);
        }
    }
}