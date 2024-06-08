using System.Text.Json.Serialization;
using Client.Models;

namespace Client.Dtos
{
    public class OrsRouteSummary
    {
        
        public OrsDirectionsSummaryDto Summary { get; set; }
    }

    public class OrsDirectionsSummaryDto
    {
        
        public float Distance { get; set; }
        
        public float Duration { get; set; }
    }
    public class OrsBaseDto
    {
        
        public List<OrsFeatureDto> Features { get; set; }
    }

    public class OrsFeatureDto
    {
        
        public OrsPropertiesDto PropertiesDto { get; set; }

        
        public OrsGeometryDto GeometryDto { get; set; }
        
        
        public List<double>? Bbox { get; set; }
    }

    public class OrsPropertiesDto
    {
        
        public string Label { get; set; }
    }

    public class OrsGeometryDto
    {
        
        [JsonPropertyName("coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}