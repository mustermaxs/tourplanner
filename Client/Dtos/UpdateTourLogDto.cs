namespace Client.Dto;
public class UpdateTourLogDto(
    int id,
    string comment,
    float difficulty,
    DateTime dateTime,
    float rating
    )
{
    public int Id { get; private set; } = id;
    public string Comment { get; private set; } = comment;
    public float Difficulty { get; private set; } = difficulty;
    public DateTime DateTime { get; private set; } = dateTime;
    public float Rating { get; private set; } = rating;
    // TODO add total time
}