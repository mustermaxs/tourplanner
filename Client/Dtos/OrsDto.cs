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