using Tourplanner.Db;

namespace Tourplanner.Entities.Maps
{
    using Tourplanner.DTOs;
    using Tourplanner.Infrastructure;
    using Tourplanner.Repositories;
    using Tourplanner.Models;
    using Tourplanner.Services;
    using Tourplanner.Entities.Tours;

    public record CreateMapCommand(int TourId,
    Coordinates From,
    Coordinates To,
    TransportType TransportType) : IRequest;

    public class CreateMapCommandHandler(
        IUnitOfWork unitOfWork,
        IImageService imageService,
        ITileCalculator tileCalculator,
        IOpenRouteService openRouteService)
        : RequestHandler<CreateMapCommand, int>()
    {

        public override async Task<int> Handle(CreateMapCommand request)    // TODO tiles Erzeugung etc in service auslagern, wird auch in UpdateMapCommand benötigt
        {
            var routeSummary = await openRouteService.RouteInfo(request.From, request.To, request.TransportType);
            var bboxValues = routeSummary.Bbox;
            var bbox = new Bbox(bboxValues.MinX, bboxValues.MinY, bboxValues.MaxX, bboxValues.MaxY);
            List<TileConfig> tileConfigs = new List<TileConfig>();
            
            int zoomLevel = 17;
            
            do
            {
                tileConfigs = tileCalculator.GetTileConfigs(zoomLevel--, bbox);
            } while (tileConfigs.Count() >= 6);

            string savePath = $"Assets";
            
            var imagePaths = await imageService.FetchImages(savePath, tileConfigs);
            var tiles = new List<Tile>();

            for (var i = 0; i < tileConfigs.Count(); i++)
            {
                var tile = new Tile();
                tile.X = tileConfigs.ElementAt(i).X;
                tile.Y = tileConfigs.ElementAt(i).Y;
                tile.Zoom = zoomLevel;
                tile.Path = imagePaths.ElementAt(i);
                tiles.Add(tile);
            }

            var map = new Map
            {
                TourId = request.TourId,
                Zoom = zoomLevel,
                Tiles = tiles,
                Bbox = bbox
            };

            await unitOfWork.MapRepository.Create(map);
            var createTilesTask = tiles.Select(async t => await unitOfWork.TileRepository.Create(t));
            await Task.WhenAll(createTilesTask);

            return map.Id;
        }
    }
}