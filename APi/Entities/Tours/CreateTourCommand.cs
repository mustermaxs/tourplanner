using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tourplanner.Db;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities.Tours
{
    public record CreateTourCommand
    (
        string Name,
        string Description,
        string From,
        string To,
        TransportType TransportType,
        Coordinates Start,
        Coordinates Destination
    ): IRequest;

    public class CreateTourCommandHandler(
        IUnitOfWork unitOfWork,
        IChildFriendlinessService childFriendlinessService,
        IImageService imageService,
        ITileCalculator tileCalculator,
        IOpenRouteService openRouteService,
        ITourRepository tourRepository) : RequestHandler<CreateTourCommand, int>()
    {
        public override async Task<int> Handle(CreateTourCommand request)
        {
            Tour tour = null;
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var tourRouteInfo =
                    await openRouteService.RouteInfo(request.Start, request.Destination, request.TransportType);

                tour = new Tour
                {
                    Name = request.Name,
                    Description = request.Description,
                    From = request.From,
                    To = request.To,
                    TransportType = request.TransportType,
                    Popularity = 0.0f,
                    EstimatedTime = tourRouteInfo.Duration,
                    Distance = tourRouteInfo.Distance
                };

                await tourRepository.Create(tour);
                await tourRepository.SaveAsync();

                tour.ChildFriendliness =
                    await childFriendlinessService.Calculate(tour.Id); // TODO unnötig hier, wird eh nicht gespeichert
                await CreateMap(tour, request.Start, request.Destination);
                await unitOfWork.CommitAsync();
                
                return tour.Id;
            }
            catch (Exception ex)
            {
                // TODO add logger
                await tourRepository.Delete(tour);
                await unitOfWork.RollbackAsync(ex);
                throw ex;
            }
        }

        private async Task<int> CreateMap(Tour tour, Coordinates from, Coordinates to)
        {
            var routeSummary = await openRouteService.RouteInfo(from, to, tour.TransportType);
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
                TourId = tour.Id,
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
