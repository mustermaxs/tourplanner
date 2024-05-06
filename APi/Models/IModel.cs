namespace Tourplanner.Models;
using Tourplanner.DTOs;

// OBSOLETE ?
public abstract class Model
{
}

// TODO in jedes Model file extension method schreiben
// um DTO für Model generieren zu können
// z.B.:

/* 
public static class TourExtension
{
    public static TourDto ToDto(this Tour)
    {
        return new TourDto(
            tourId,
            name,
            description,
            ...
            );
    }
}
*/