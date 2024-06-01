namespace Client.Models
{
    public record Coordinates(double Longitude, double Latitude);
    
    public record Location(
        string Label,
        Coordinates Coordinates
    );   
}

