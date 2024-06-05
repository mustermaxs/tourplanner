using Client.Dtos;
using Client.Models;

    public class TourDto
    {
        public TourDto(
            int tourId,
            string name,
            string description,
            string from,
            string to,
            Coordinates coordinates,
            TransportType transportType,
            double distance,
            float timespan,
            float popularity,
            double childfriendliness
        )
        {
            TourId = tourId;
            Name = name;
            Description = description;
            From = from;
            To = to;
            Coordinates = coordinates;
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = timespan;
            Popularity = popularity;
            Childfriendliness = childfriendliness;
        }
        public int TourId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string From { get; protected set; }
        public string To { get; protected set; }
        public Coordinates Coordinates { get; protected set; }
        public TransportType TransportType { get; protected set; }
        public double Distance { get; protected set; }
        public float EstimatedTime { get; protected set; }
        public float Popularity { get; protected set; }
        public double Childfriendliness { get; protected set; }
    }
