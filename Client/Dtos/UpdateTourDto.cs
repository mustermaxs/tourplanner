using Client.Models;

namespace Client.Dtos
{

    public class UpdateTourDto
    {
        public UpdateTourDto(
            string name,
            string description,
            string from,
            string to,
            Coordinates start,
            Coordinates destination,
            TransportType transportType)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Start = start;
            Destination = destination;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TransportType TransportType { get; set; }
        public Coordinates? Start { get; set; }
        public Coordinates? Destination { get; set; }
    }
}
