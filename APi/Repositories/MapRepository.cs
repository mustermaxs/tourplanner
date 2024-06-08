using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities;
using Tourplanner.Services;
using Tourplanner.Exceptions;

namespace Tourplanner.Repositories
{
    public interface IMapRepository : IRepository<Map>
    {
        public Task<int> CreateReturnId(Map map);
        public Task<Map?> GetMapWithTilesForTour(int tourId);
    }
    public class MapRepository : Repository<Map>, IMapRepository
    {
        private IImageService _imageService;

        public MapRepository(TourContext ctx, IImageService imageService) : base(ctx)
        {
            _imageService = imageService;
        }

        public async Task<int> CreateReturnId(Map map)
        {
            await dbSet.AddAsync(map);
            await SaveAsync();
            return map.Id;
        }

        public async Task<Map?> GetMapWithTilesForTour(int tourId)
        {
            var map = await dbSet.Include(m => m.Tiles)
            .FirstOrDefaultAsync(m =>m.TourId == tourId);

            if (map is null) return null;

            foreach (var mapTile in map.Tiles)
            {
                mapTile.Base64Encoded = _imageService.ReadImageAsBase64(mapTile.Path);
            }
            
            return map;
        }
     }
}