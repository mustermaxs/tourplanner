
namespace Client.ViewModels;

public class BaseViewModel
{
    protected Action _notifyStateChanged;

    public BaseViewModel() {}
    
    public virtual async Task InitializeAsync(Action notifySateChanged)
    {
        _notifyStateChanged = notifySateChanged;
        await Task.CompletedTask;
    }
    
}