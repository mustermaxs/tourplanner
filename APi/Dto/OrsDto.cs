using System.Text.Json.Serialization;
using Tourplanner.Services;

namespace Tourplanner.DTOs
{
    public class OrsRouteSummary
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

        private Coordinates? _coordinates;

        public Coordinates TPCoordinates
        {
            get
            {
                if (_coordinates == null && RawCoordinates is { Count: >= 2 })
                {
                    _coordinates = new Coordinates(RawCoordinates[0], RawCoordinates[1]);
                }
                return _coordinates ?? new Coordinates(0, 0); // Return a default value if coordinates are null
            }
        }
    }
}