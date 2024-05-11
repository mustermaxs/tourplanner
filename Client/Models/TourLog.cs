public class TourLog
{
    public int Id { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Comment { get; set; } = "";
    public float Difficulty { get; set; } = 0;
    public float Duration { get; set; } = 0;
    public float Rating { get; set; } = 0;
    public Tour Tour { get; set; } = new Tour();
}
