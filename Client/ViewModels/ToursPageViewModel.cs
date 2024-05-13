using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Models;
using Client.Dao;

public class ToursViewModel
{
    private ITourDao _tourDao;
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();

    public ToursViewModel(ITourDao tourDao)
    {
        _tourDao = tourDao;
    }

    public async Task GetToursAsync()
    {
        try
        {
            Tours = await _tourDao.ReadMultiple();

            foreach (var tour in Tours)
            {
                Console.WriteLine(tour.Name);
                Console.WriteLine(tour.Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tours: {ex.Message}");
            throw;
        }
    }
}