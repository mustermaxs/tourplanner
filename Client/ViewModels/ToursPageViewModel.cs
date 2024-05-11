using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public class ToursViewModel
{
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();
    private IHttpService _httpService;
    public ToursViewModel(IHttpService httpService)
    {
        _httpService = httpService;
    }

public async Task GetToursAsync()
{
    try
    {
        Tours = await _httpService.Get<IEnumerable<Tour>>("Tours");

        foreach (var tour in Tours)
        {
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
