using Tourplanner.Entities;
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
            await dbSet.AddAsync(tile);
        }

        public async Task<IEnumerable<Tile>> GetAllByMapId(int mapId)
        {
            var tiles = dbSet.Where(t => t.MapId == mapId).ToList();
            tiles.ForEach(t => t.Base64Encoded = _imageService.ReadImageAsBase64(t.Path));
            return tiles;
        }
    }
}