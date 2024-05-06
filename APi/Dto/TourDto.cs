using System;

namespace Tourplanner.DTOs
{
    using Tourplanner.Models;

    public class TourDto
    {
        public TourDto(
            int tourId,
            string name,
            string description,
            string from,
            string to,
            Enum transportType,
            double distance,
            TimeSpan timespan,
            int popularity,
            double childfriendliness,
            string routeImage
        )
        {
            TourId = tourId;
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
        public int TourId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string From { get; protected set; }
        public string To { get; protected set; }
        public Enum TransportType { get; protected set; }
        public double Distance { get; protected set; }
        public TimeSpan EstimatedTime { get; protected set; }
        public int Popularity { get; protected set; }
        public double Childfriendliness { get; protected set; }
        public string RouteImage { get; protected set; }
    }
}
