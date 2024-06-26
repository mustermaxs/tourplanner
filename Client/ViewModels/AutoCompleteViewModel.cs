namespace Client.ViewModels;

public class AutoCompleteViewModel : BaseViewModel
{
    public List<string> Suggestions { get; set; } = new();
    
}