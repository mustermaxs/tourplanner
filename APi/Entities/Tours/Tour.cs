using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tourplanner.Models;
using Tourplanner.Entities.TourLogs;

namespace Tourplanner.Entities.Tours;

public class Tour
{
    public Tour()
    {
        TourLogs = new List<TourLog>();
    }

    public int Id { get; set; }

    [MaxLength(60)] public string Name { get; set; }

    [MaxLength(150)] public string From { get; set; }

    [MaxLength(150)] public string To { get; set; }

    public float Distance { get; set; }

    [MaxLength(500)] public string Description { get; set; }

    public float EstimatedTime { get; set; }

    [MaxLength(150)] public string ImagePath { get; set; } = string.Empty;

    public float Popularity { get; set; }

    public float ChildFriendliness { get; set; }

    public ICollection<TourLog> TourLogs { get; set; }

    public TransportType TransportType { get; set; }
}