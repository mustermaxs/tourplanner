namespace Tourplanner.Services.FormattingUtils;

public static class Formatting
{
    public static string MetersToKmAndMeters(float distanceInMeters)
    {
        if (distanceInMeters < 1000)
            return $"{distanceInMeters} m";
        else
        {
            float distanceInKm = distanceInMeters / 1000;
            return $"{distanceInKm:0.00} km";
        }
            
    }
    
    public static string SecondsToDaysMinutesHours(double totalSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

        int days = timeSpan.Days;
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;

        string result = "";
        if (days > 0)
            result += $"{days} days, ";
        if (hours > 0)
            result += $"{hours} hours, ";
        if (minutes > 0)
            result += $"{minutes} minutes, ";

        return result.TrimEnd(',', ' ');
    }
}