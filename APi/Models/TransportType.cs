using System.Runtime.Serialization;

namespace Tourplanner.Models;

public enum TransportType 
{
    // [EnumMember(Value = "Bicycle")]
    Bicycle = 0,
    // [EnumMember(Value = "Car")]
    Car,
    // [EnumMember(Value = "Walking")]
    Walking,
    // [EnumMember(Value = "Hiking")]
    Hiking,
    // [EnumMember(Value = "Wheelchair")]
    Wheelchair,
}