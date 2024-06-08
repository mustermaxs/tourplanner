using System.Text.Json.Serialization;
using Tourplanner.Entities.Tours;
using Tourplanner.Services;

namespace Tourplanner.DTOs
{
    public class OrsRouteSummary
    {
        [JsonPropertyName("summary")]
        public OrsDirectionsSummaryDto Summary { get; set; }
        
        [JsonPropertyName("bbox")]
        public List<double> OrsBbox {get; set;}
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

        private Coordinates? _coordinates;

        public Coordinates TPCoordinates
        {
            get
            {
            //TODO: FIX THIS
                    var c = new Coordinates();
                if (_coordinates == null && RawCoordinates is { Count: >= 2 })
                {
                    c.Longitude = RawCoordinates[0];
                    c.Latitude = RawCoordinates[1];
                    _coordinates = c;
                }
                c.Longitude = 0;
                c.Latitude = 0;
                
                return _coordinates ?? c;
            }
        }
    }
}