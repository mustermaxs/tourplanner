using Client.Dao;

namespace Client.ViewModels;

public class MapViewModel : BaseViewModel
{
    private readonly IMapDao _mapDao;

    private readonly int _tourId;

    public string[] Images { get; set; }

   

    public MapViewModel(IMapDao mapDao, int tourId)
    {
        _mapDao = mapDao;
        _tourId = tourId;
    }

    public async Task SetTourId()
    {
        await _mapDao.Read(_tourId);
    }

    public override async Task InitializeAsync(Action notifySateChanged)
    {

    }
}