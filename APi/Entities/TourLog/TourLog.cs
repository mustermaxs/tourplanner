using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tourplanner.Entities.TourLog;

using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tour;
public class TourLog
{
    public int TourLogId { get; set; }
    public float Difficulty { get; set; }
    public float Duration { get; set; }
    public float Rating { get; set; }
    
    [MaxLength(500)]
    public string Comment { get; set; } = String.Empty;
    
    [Required]
    public int TourId { get; set; }
    public Tour Tour { get; set; }
    public DateTime Date { get; set; }
}