using System.Text.Json;
using System.Text.Json.Nodes;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Services;

namespace Tourplanner.Entities
{
    public record GetGeoAutoCompleteQuery(string Location) : IRequest;


    public class GetGeoAutoCompleteQueryHandler(
        IOpenRouteService openRouteService) : RequestHandler<GetGeoAutoCompleteQuery, OrsBaseDto>()
    {
        public override async Task<OrsBaseDto> Handle(GetGeoAutoCompleteQuery request)
        {
            return await openRouteService.GetAutoCompleteSuggestions(request.Location);
        }
    }
}