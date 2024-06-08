using Tourplanner;
using Tourplanner.DTOs;
using Tourplanner.Entities;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Maps
{
    public record GetMapRequest(int MapId) : IRequest;

    public class GetMapRequestHandler(
        TourContext ctx,
        IMapRepository mapRepository,
        ITileRepository tileRepository) : RequestHandler<GetMapRequest, MapDto>(ctx)
    {
        public override async Task<MapDto> Handle(GetMapRequest request)
        {
            var map = await mapRepository.GetMapWithTiles(request.MapId);
            return map.ToMapDto();
        }
    }
}