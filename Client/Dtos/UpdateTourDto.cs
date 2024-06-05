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
            TransportType transportType,
            Coordinates? coordinates = null)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Coordinates = coordinates;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TransportType TransportType { get; set; }
        public Coordinates? Coordinates { get; set; }
    }
}
