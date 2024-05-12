using System.Linq;
using System.Threading.Tasks;
using Client.Pages;

public class TourEditPageViewModel
{
    private IHttpService _httpService;

    public TourEditPageViewModel(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Tour Tour { get; set; } = new Tour();

    public void UpdateTour()
    {
        Console.WriteLine("UpdateTour", Tour.Name);
        _httpService.Put<UpdateTourDto>(
            new UpdateTourDto(
                Tour.Name,
                Tour.Description,
                Tour.From,
                Tour.To,
                Tour.TransportType
            ),
            $"tours/{Tour.Id}"
        );
    }

    public async Task InitializeAsync(int id)
    {
        Tour = await _httpService.Get<Tour>($"Tours/{id}");
    }
}