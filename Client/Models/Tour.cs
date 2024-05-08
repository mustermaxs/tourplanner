public class Tour
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public String From { get; set; } = "";
    public String To { get; set; } = "";
    public double EstimatedTime { get; set; } = 0;
    public string Description { get; set; } = "";
    public string TransportType { get; set; } = "";
    public int Difficulty { get; set; } = 0;
    public int Rating { get; set; } = 0;
}