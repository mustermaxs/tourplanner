using Tourplanner.Entities.Tours;
using Tourplanner.Models;

namespace Tourplanner.DTOs;

public class UpdateTourDto
{
    public UpdateTourDto(
        string name,
        string description,
        string from,
        string to,
        Coordinates coordinates,
        TransportType transportType)
    {
        Name = name;
        Description = description;
        From = from;
        To = to;
        Coordinates = coordinates;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public Coordinates Coordinates { get; set; }
    public TransportType TransportType { get; set; }
}