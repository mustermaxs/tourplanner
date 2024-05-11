public enum TransportType
{
    Car,
    Bicycle,
    Walking,
    PublicTransport
}

public class Tour
{
    public int Id { get; set; }
    public string Name { get; set; } = "Wanderweg";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public float EstimatedTime { get; set; } = 0;
    public string Description { get; set; } = "";
    public float Distance { get; set; } = 0;
    public float Popularity { get; set; } = 0;
    public TransportType TransportType { get; set; } = TransportType.Car;
    public float ChildFriendliness { get; set; } = 0;
    public string ImagePath { get; set; } = "";
}