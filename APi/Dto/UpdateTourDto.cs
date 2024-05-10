using Tourplanner.Models;

namespace Tourplanner.DTOs;

public class UpdateTourDto
{
    public UpdateTourDto(
        string name,
        string description,
        string from,
        string to,
        TransportType transportType)
    {
        Name = name;
        Description = description;
        From = from;
        To = to;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public TransportType TransportType { get; set; }
}