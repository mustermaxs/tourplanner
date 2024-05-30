namespace Client.Models
{
    public record Coordinates(double Longitude, double Latitude);
    
    public record GeoSuggestion(
        string Label,
        Coordinates Coordinates
    );   
}

