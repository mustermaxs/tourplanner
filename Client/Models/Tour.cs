using Client.Dao;
using Client.Dtos;

namespace Client.Models
{
    public enum TransportType
    {
        Car,
        Bicycle,
        Walking,
        PublicTransport
    }

    public class Tour
    {
        public Tour()
        {
        }
        public Tour(int id, string name, string from, string to, float estimatedTime, string description, float distance, float popularity, TransportType transportType, float childFriendliness, Coordinates start = null, Coordinates destination = null)
        {
            Id = id;
            Name = name;
            From = from;
            To = to;
            Start = start;
            Destination = destination;
            EstimatedTime = estimatedTime;
            Description = description;
            Distance = distance;
            Popularity = popularity;
            TransportType = transportType;
            ChildFriendliness = childFriendliness;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public Coordinates? Start { get; set; } = null;
        public Coordinates? Destination { get; set; } = null;
        public float EstimatedTime { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public float Distance { get; set; } = 0;
        public float Popularity { get; set; } = 0;
        public TransportType TransportType { get; set; } = TransportType.Car;
        public float ChildFriendliness { get; set; } = 0;
    }

    public static class TourExtensionMethods
    {
        public static CreateTourDto ToCreateTourDto(this Tour tour)
        {
            return new CreateTourDto(
                name: tour.Name,
                description: tour.Description,
                from: tour.From,
                to: tour.To,
                estimatedTime: tour.EstimatedTime,
                transportType: tour.TransportType,
                start: tour.Start,
                destination: tour.Destination
                );
        }
        public static UpdateTourDto ToUpdateTourDto(this Tour tour)
        {
            return new UpdateTourDto(
                name: tour.Name,
                description: tour.Description,
                from: tour.From,
                to: tour.To,
                transportType: tour.TransportType,
                start: tour.Start,
                destination: tour.Destination
            );
        }
    }
}