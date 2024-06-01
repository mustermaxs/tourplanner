using System.Text.Json;
using System.Text.Json.Nodes;
using Tourplanner.Infrastructure;
using Tourplanner.Services;

namespace Tourplanner.Entities
{
    public record GetGeoAutoCompleteQuery(string Location) : IRequest;


    public class GetGeoAutoCompleteQueryHandler(
        TourContext ctx,
        IOpenRouteService openRouteService) : RequestHandler<GetGeoAutoCompleteQuery, JsonObject>(ctx)
    {
        public override async Task<JsonObject> Handle(GetGeoAutoCompleteQuery request)
        {
            return await openRouteService.GetAutoCompleteSuggestions(request.Location);
        }
    }
}