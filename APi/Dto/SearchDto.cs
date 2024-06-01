namespace Tourplanner.DTOs;

public class SearchDto(string searchTerm, string category, int threshold)
{
    public string SearchTerm { get; private set; } = searchTerm;
    public string Category { get; private set; } = category;
    public int Threshold { get; private set; } = threshold;
}