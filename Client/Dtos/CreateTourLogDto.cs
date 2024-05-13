
public class CreateTourLogDto(
    DateTime dateTime,
    string comment,
    float difficulty,
    float duration,
    float rating)
{
    public DateTime DateTime { get; private set; } = dateTime;
    public string Comment { get; private set; } = comment;
    public float Difficulty { get; private set; } = difficulty;
    public float Duration { get; private set; } = duration;
    public float Rating { get; private set; } = rating;
}