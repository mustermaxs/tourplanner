using Client.Models;
using Client.Dao;

public class ToursPageViewModel
{
    private ITourDao _tourDao;
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();

    public ToursPageViewModel(ITourDao tourDao)
    {
        _tourDao = tourDao;
    }

    public async Task GetToursAsync()
    {
        try
        {
            Tours = await _tourDao.ReadMultiple();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tours: {ex.Message}");
            throw;
        }
    }
}