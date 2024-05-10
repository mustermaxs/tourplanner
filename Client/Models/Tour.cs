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
    public string Name { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public double EstimatedTime { get; set; } = 0;
    public string Description { get; set; } = "";
    public float Difficulty { get; set; } = 0;
    public float Rating { get; set; } = 0;
    public TransportType TransportType { get; set; } = TransportType.Car;
}