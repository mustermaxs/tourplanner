using System.Text.Json;
using System.Text.Json.Nodes;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Services;

namespace Tourplanner.Entities
{
    public record GetGeoAutoCompleteQuery(string Location) : IRequest;


    public class GetGeoAutoCompleteQueryHandler(
        TourContext ctx,
        IOpenRouteService openRouteService) : RequestHandler<GetGeoAutoCompleteQuery, OrsBaseDto>(ctx)
    {
        public override async Task<OrsBaseDto> Handle(GetGeoAutoCompleteQuery request)
        {
            return await openRouteService.GetAutoCompleteSuggestions(request.Location);
        }
    }
}