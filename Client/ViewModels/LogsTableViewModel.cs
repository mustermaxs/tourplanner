using Client.Models;

namespace Client.ViewModels;

public class LogsTableViewModel : BaseViewModel
{
    private IHttpService _httpService { get; set; }
    public IEnumerable<TourLog> TourLogs { get; set; } = new List<TourLog>();

    public LogsTableViewModel(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task GetTourLogsAsync(int tourId)
    {
        try
        {
            TourLogs = await _httpService.Get<IEnumerable<TourLog>>($"Tours/{tourId}/logs");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tour logs. {ex.Message}");
        }
    }

}