using Client.Dao;

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
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public float EstimatedTime { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public float Distance { get; set; } = 0;
        public float Popularity { get; set; } = 0;
        public TransportType TransportType { get; set; } = TransportType.Car;
        public float ChildFriendliness { get; set; } = 0;
        public string ImagePath { get; set; } = string.Empty;
    }

}