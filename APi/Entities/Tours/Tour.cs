using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tourplanner.DTOs;
using Tourplanner.Models;
using Tourplanner.Entities.TourLogs;

namespace Tourplanner.Entities.Tours
{

    public class Coordinates
    {
        public double Longitude {get; set;}
        public double Latitude {get; set;}

        public Coordinates(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }

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
        public Coordinates Coordinates {get; set;}
        public float Distance { get; set; }
        [MaxLength(500)] public string Description { get; set; }
        public float EstimatedTime { get; set; }
        public float Popularity { get; set; }
        public float ChildFriendliness { get; set; }
        public ICollection<TourLog> TourLogs { get; set; }
        public TransportType TransportType { get; set; }
        public int? MapId { get; set; }
        public Map Map { get; set; }
    }

    public static class TourExtensionMethods
    {
        public static TourDto ToTourDto(this Tour tour)
        {
            return new TourDto(
                tour.Id,
                tour.Name,
                tour.Description,
                tour.From,
                tour.To,
                tour.Coordinates,
                tour.TransportType,
                tour.Distance,
                tour.EstimatedTime,
                tour.Popularity,
                tour.ChildFriendliness
            );
        }
    }
}