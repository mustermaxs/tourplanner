using Client.Components;
using Client.Dao;
using Client.Models;

namespace Client.ViewModels;

public class ToursPageViewModel(ITourDao tourDao, PopupViewModel popupViewModel) : BaseViewModel
{
    public IEnumerable<Tour> Tours { get; private set; } = new List<Tour>();

    public async Task GetToursAsync()
    {
        try
        {
            Tours = await tourDao.ReadMultiple();
            _notifyStateChanged.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tours: {ex.Message}");
            popupViewModel.Open("Error", "Failed to get tours.", PopupStyle.Error);
            throw;
        }
    }
}