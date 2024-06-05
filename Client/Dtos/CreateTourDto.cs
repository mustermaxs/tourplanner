using Client.Dtos;
using Client.Models;

namespace Client.Dtos;

public class CreateTourDto
{
    public CreateTourDto(
        string name,
        string description,
        string from,
        string to,
        float distance,
        float estimatedTime,
        TransportType transportType,
        Coordinates? coordinates
    )
    {
        Name = name;
        Description = description;
        From = from;
        To = to;
        Coordinates = coordinates;
        Distance = distance;
        EstimatedTime = estimatedTime;
        TransportType = transportType;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public float Distance { get; set; }
    public float EstimatedTime { get; set; }
    public TransportType TransportType { get; set; }
    public Coordinates? Coordinates { get; set; }
}