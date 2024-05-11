using System;

namespace Tourplanner.DTOs
{
    using Tourplanner.Models;

    public class TourDto
    {
        public TourDto(
            int id,
            string name,
            string description,
            string from,
            string to,
            TransportType transportType,
            double distance,
            float timespan,
            float popularity,
            double childfriendliness,
            string routeImage
        )
        {
            Id = id;
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = timespan;
            Popularity = popularity;
            Childfriendliness = childfriendliness;
            RouteImage = routeImage;
        }
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string From { get; protected set; }
        public string To { get; protected set; }
        public TransportType TransportType { get; protected set; }
        public double Distance { get; protected set; }
        public float EstimatedTime { get; protected set; }
        public float Popularity { get; protected set; }
        public double Childfriendliness { get; protected set; }
        public string RouteImage { get; protected set; }
    }
}
