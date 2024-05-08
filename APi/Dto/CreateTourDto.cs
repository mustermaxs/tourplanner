namespace Tourplanner.DTOs
{
    public class CreateTourDto
    {
        public CreateTourDto(
            string name,
            string description,
            string from,
            string to,
            float distance,
            float estimatedTime,
            string imagePath
        )
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            Distance = distance;
            EstimatedTime = estimatedTime;
            ImagePath = imagePath;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public float Distance { get; set; }
        public float EstimatedTime { get; set; }
        public string ImagePath { get; set; }
    }
}