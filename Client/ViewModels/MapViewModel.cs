using Client.Dao;
using Client.Exceptions;
using Client.Models;

namespace Client.ViewModels;

public class MapViewModel : BaseViewModel
{
    private readonly IMapDao _mapDao;

    private readonly int _tourId;
    public Map Map { get; private set; } = new Map();

    public IEnumerable<(string Image, int X, int Y)> ImageTiles { get; private set; }

    public MapViewModel(IMapDao mapDao, int tourId)
    {
        _mapDao = mapDao;
        _tourId = tourId;
    }

    public override async Task InitializeAsync(Action notifyStateChanged)
    {
        try
        {
            Console.WriteLine("GET MAP");
            Map = await _mapDao.Read(_tourId);
            ArrangeImages();
            notifyStateChanged.Invoke();
        }
        catch (Exception e)
        {
            throw new UserActionException("Failed to fetch tour");
        }
    }

    private void ArrangeImages()
    {
        ImageTiles = Map.Tiles
            .Select(tile => (tile.Image, tile.X, tile.Y));
    }

}