using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TourDetailsPageViewModel
{
    private IHttpService _httpService;

    public TourDetailsPageViewModel(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Tour Tour { get; private set; } = new Tour();
    public List<TourLog> TourLogs { get; private set; } = new List<TourLog>();

    public async Task InitializeAsync(int tourId)
    {
        TourLogs = (List<TourLog>)await _httpService.Get<IEnumerable<TourLog>>($"Tours/{tourId}/logs");
        Tour = await _httpService.Get<Tour>($"Tours/{tourId}");
    }


    public async Task DeleteTour()
    {

        bool deleted = await Tour.Delete();
        
        if (deleted)
        {
            foreach (var log in TourLogs)
            {
                await log.Delete();
            }
        }

    }

}
