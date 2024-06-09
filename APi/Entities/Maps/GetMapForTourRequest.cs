using Tourplanner;
using Tourplanner.DTOs;
using Tourplanner.Entities;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Maps
{
    public record GetMapForTourRequest(int TourId) : IRequest;

    public class GetMapForTourRequestHandler(
        TourContext ctx,
        IMapRepository mapRepository,
        ITileRepository tileRepository) : RequestHandler<GetMapForTourRequest, MapDto>(ctx)
    {
        public override async Task<MapDto> Handle(GetMapForTourRequest forTourRequest)
        {
            var map = await mapRepository.GetMapWithTilesForTour(forTourRequest.TourId);
            var mapDto = map.ToMapDto();
            return mapDto;
        }
    }
}