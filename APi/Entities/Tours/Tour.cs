using System.ComponentModel.DataAnnotations;
using Tourplanner.Models;

namespace Tourplanner.Entities.Tour;

using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.TourLog;

public class Tour
{
    public Tour()
    {
        TourLogs = new List<TourLog>();
    }
    public int TourId { get; set; } 
    
    [MaxLength((30))]
    public string Name { get; set; }
    
    [MaxLength(150)]
    public string From { get; set; }
    
    [MaxLength(150)]
    public string To { get; set; }
    
    [Precision(10, 3)]
    public float Distance { get; set; }
    
    [MaxLength(500)]
    public string Description { get; set; }
    
    public TimeSpan EstimatedTime { get; set; }
    
    [MaxLength(150)]
    public string ImagePath { get; set; }
    
    [Precision(5,4)]
    public float Popularity { get; set; }
    
    [Precision(5,3)]
    public float ChildFriendliness { get; set; }
    public IEnumerable<TourLog> TourLogs { get; set; }
    public TransportType TransportType { get; set; }
    
}