using System.Linq;
using System.Threading.Tasks;
using Client.Dao;
using Client.Models;
using Client.Dto;
public class TourEditPageViewModel
{
    private ITourDao tourDao;
    public TourEditPageViewModel(ITourDao tourDao)
    {
        this.tourDao = tourDao;
    }

    public Tour Tour { get; set; } = new Tour();

    public async Task UpdateTour()
    {
        Console.WriteLine("UpdateTour", Tour.Name);
        await tourDao.Update(Tour);
    }

    public async Task InitializeAsync(int id)
    {
        var tour = new Tour();
        tour.Id = id;
        Tour = await tourDao.Read(tour);
    }
}