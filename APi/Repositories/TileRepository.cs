using Tourplanner.Entities;
using Tourplanner.Exceptions;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Entities
{
    public interface ITileRepository : IRepository<Tile>
    {
    }

    public class TileRepository : Repository<Tile>, ITileRepository
    {
        private readonly IImageService _imageService;

        public TileRepository(TourContext ctx, IImageService imageService) : base(ctx)
        {
            _imageService = imageService;
        }

        public async Task Create(Tile tile)
        {
            try
            {
                await dbSet.AddAsync(tile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DataAccessLayerException("Failed to create tile", e);
            }
        }

        public async Task<IEnumerable<Tile>> GetAllByMapId(int mapId)
        {
            try
            {
                var tiles = dbSet.Where(t => t.MapId == mapId).ToList();
                tiles.ForEach(t => t.Base64Encoded = _imageService.ReadImageAsBase64(t.Path));
                return tiles;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DataAccessLayerException("Failed to fetch tiles for map", e);
            }
        }
    }
}