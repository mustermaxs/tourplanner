public class TourLog
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; } = "";
    public float Difficulty { get; set; } = 0;
    public double TotalDistance { get; set; } = 0;
    public TimeSpan TotalTime { get; set; }
    public float Rating { get; set; } = 0;
}
