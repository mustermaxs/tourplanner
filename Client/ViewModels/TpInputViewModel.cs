using Client.Exceptions;
using Client.Utils.Specifications;

namespace Client.ViewModels;

public class TpInputViewModel : BaseViewModel
{
    public string? UserInput { get; set; } = "";
    public string InputType { get; set; } = "text";
    public string? InvalidInputMessage { get; set; } = "Input is invalid";
    public ISpecification<dynamic>? Specification { get; set; }
    private bool _inputIsValid = true;
    private Action OnShowError { get; set; } = () => { };
    private Action OnHideError { get; set; } = () => { };

    public async Task InitializeAsync(Action notifySateChanged, Action showError, Action hideError)
    {
        OnShowError = showError;
        OnHideError = hideError;
        await base.InitializeAsync(notifySateChanged);
    }

    public bool InputIsValid
    {
        get
        {
            _inputIsValid = Specification?.IsSatisfiedBy(ConvertInput(UserInput)) ?? true;
            return _inputIsValid;
        }
        private set => _inputIsValid = value;
    }

    private Type GetTypeForCast()
    {
        switch (InputType)
        {
            case "number":
                return typeof(float);
            case "date":
                return typeof(DateTime);
            case "text":
                return typeof(string);
            default:
                return typeof(string);
        }
    }

    private dynamic ConvertInput(string input)
    {
        try
        {
            return (dynamic)Convert.ChangeType(input, GetTypeForCast());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void ShowError(string? message)
    {
        InputIsValid = false;
        OnShowError();
        _notifyStateChanged.Invoke();
    }

    public void HideError()
    {
        InputIsValid = true;
        OnShowError();
        _notifyStateChanged.Invoke();
    }
}