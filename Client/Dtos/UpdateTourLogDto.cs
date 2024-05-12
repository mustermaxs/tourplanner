namespace Client.Dto;
public class UpdateTourLogDto(
    int id,
    string comment,
    float difficulty,
    float duration,
    float rating)
{
    public int Id { get; private set; } = id;
    public string Comment { get; private set; } = comment;
    public float Difficulty { get; private set; } = difficulty;
    public float Duration { get; private set; } = duration;
    public float Rating { get; private set; } = rating;
}