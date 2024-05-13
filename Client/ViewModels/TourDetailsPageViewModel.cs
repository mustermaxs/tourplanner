using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Client.Dao;
using Client.Models;
using Client.Dto;

public class TourDetailsPageViewModel
{
    private ITourDao _tourDao;
    private ITourLogDao _tourLogDao;

    public TourDetailsPageViewModel(ITourDao tourDao, ITourLogDao TourLogDao)
    {
        _tourDao = tourDao;
        _tourLogDao = TourLogDao;
    }

    public Tour Tour { get; private set; } = new Tour();
    public List<TourLog> TourLogs { get; private set; } = new List<TourLog>();

    public async Task InitializeAsync(int tourId)
    {
        TourLogs = (List<TourLog>) await _tourLogDao.ReadMultiple(tourId);
        Tour = new Tour();
        Tour.Id = tourId;
        Tour = await _tourDao.Read(Tour);
    }

    public async Task DeleteTour()
    {
        await _tourDao.Delete(Tour);

        foreach (var log in TourLogs)
        {
            await _tourLogDao.Delete(log);
        }
    }
}