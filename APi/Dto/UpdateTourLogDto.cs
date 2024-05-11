namespace Tourplanner.DTOs;

public class UpdateTourLogDto(
    int id,
    string comment,
    float difficulty,
    float totalTime,
    float rating)
{
    public int Id { get; private set; } = id;
    public string Comment { get; private set; } = comment;
    public float Difficulty { get; private set; } = difficulty;
    public float TotalTime { get; private set; } = totalTime;
    public float Rating { get; private set; } = rating;
}