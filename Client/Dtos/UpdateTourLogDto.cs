namespace Client.Dto;
public class UpdateTourLogDto(
    int id,
    string comment,
    float difficulty,
    DateTime dateTime,
    float rating,
    float distance,
    float duration
    )
{
    public int Id { get; private set; } = id;
    public string Comment { get; private set; } = comment;
    public float Difficulty { get; private set; } = difficulty;
    public DateTime DateTime { get; private set; } = dateTime;
    public float Rating { get; private set; } = rating;
    public float Distance { get; private set; } = distance;
    public float Duration { get; private set; } = duration;
    
    // TODO add total time
}