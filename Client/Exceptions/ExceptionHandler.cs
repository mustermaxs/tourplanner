using System.Reflection.Metadata;
using Client.Components;

namespace Client.Exceptions;

public class ExceptionHandler(PopupViewModel popupViewModel)
{
    public PopupViewModel _PopupViewModel { get; } = popupViewModel;

    public void Handle(Exception ex)
    {
        Console.WriteLine("HANDLE EXCEPT ION HURENHARALD");
        _PopupViewModel.Open("Error", ex.Message, PopupStyle.Error);
    }
}