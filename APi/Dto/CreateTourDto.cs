using Tourplanner.Entities.Tours;
using Tourplanner.Models;

namespace Tourplanner.DTOs
{
    public class CreateTourDto
    {
        public CreateTourDto(
            string name,
            string description,
            string from,
            string to,
            Coordinates start,
            Coordinates destination,
            float estimatedTime,
            TransportType transportType
        )
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            Start = start;
            Destination = destination;
            EstimatedTime = estimatedTime;
            TransportType = transportType;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public float EstimatedTime { get; set; }
        public TransportType TransportType { get; set; }
        public Coordinates Start { get; set; }
        public Coordinates Destination {get; set;}
    }
}