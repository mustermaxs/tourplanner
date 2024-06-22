using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities;
using Tourplanner.Services;
using Tourplanner.Exceptions;

namespace Tourplanner.Repositories
{
    public interface IMapRepository : IRepository<Map>
    {
        public Task<Map?> GetMapWithTilesForTour(int tourId);
        public Task<Map?> GetMapByTourId(int tourId);
    }

    public class MapRepository : Repository<Map>, IMapRepository
    {
        private IImageService _imageService;

        public MapRepository(TourContext ctx, IImageService imageService) : base(ctx)
        {
            _imageService = imageService;
        }


        public async Task<Map?> GetMapWithTilesForTour(int tourId)
        {
            try
            {
                var map = await dbSet.Include(m => m.Tiles)
                    .FirstOrDefaultAsync(m => m.TourId == tourId);

                if (map is null) return null;

                foreach (var mapTile in map.Tiles)
                {
                    mapTile.Base64Encoded = _imageService.ReadImageAsBase64(mapTile.Path);
                }

                return map;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DataAccessLayerException("Failed to fetch map with tiles for tour", e);
            }
        }

        public async Task<Map?> GetMapByTourId(int tourId)
        {
            try
            {
                return await dbSet.Include(m => m.Tiles)
                    .FirstOrDefaultAsync(m => m.TourId == tourId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DataAccessLayerException("Failed to fetch map for tour", e);
            }
        }
    }
}