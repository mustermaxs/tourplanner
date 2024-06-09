using System.Text.Json.Serialization;
using Tourplanner.Entities.Tours;
using Tourplanner.Entities;
using Tourplanner.Services;

namespace Tourplanner.DTOs
{
    public class Summary
    {
        public Summary(Bbox bbox, double distance, double duration)
        {
            Bbox = bbox;
            Distance = distance;
            Duration = duration;
        }
        public Bbox Bbox {get; set;}
        public double Distance {get; set;}
        public double Duration {get; set;}
    }

    // public class OrsRouteSummary
    // {
    //     [JsonPropertyName("features")]
    //     public List<OrsFeaturesSummary> Features { get; set; }

    //     [JsonPropertyName("bbox")]
    //     public List<double> OrsBbox { get; set; }

    //     // [JsonIgnore]
    //     // public OrsFeaturesSummary Summary {get => Features.ElementAt(0)!.Summary;}
    // }

    public class OrsFeaturesSummary
    {
        [JsonPropertyName("properties")]
        public OrsDirectionsSummaryDto Summary { get; set; }
    }

    public class OrsFeaturesSummaryProperties
    {
        [JsonPropertyName("summary")]
        public OrsDirectionsSummaryDto Summary { get; set; }
    }

    public class OrsDirectionsSummaryDto
    {
        [JsonPropertyName("distance")]
        public float Distance { get; set; }
        [JsonPropertyName("duration")]
        public float Duration { get; set; }
    }
    public class OrsBaseDto
    {
        [JsonPropertyName("features")]
        public List<OrsFeatureDto> Features { get; set; }
    }

    public class OrsFeatureDto
    {
        [JsonPropertyName("properties")]
        public OrsPropertiesDto PropertiesDto { get; set; }

        [JsonPropertyName("geometry")]
        public OrsGeometryDto GeometryDto { get; set; }

        [JsonPropertyName("bbox")]
        public List<double>? Bbox { get; set; }
    }

    public class OrsPropertiesDto
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }


    public class OrsGeometryDto
    {
        [JsonPropertyName("coordinates")]
        public List<double>? RawCoordinates { get; set; }


        [JsonIgnore]
        public Coordinates Coordinates
        {
            get
            {
                if (RawCoordinates == null)
                {
                    return null;
                }
                return new Coordinates(RawCoordinates[1], RawCoordinates[0]);
            }
        }
    }
}