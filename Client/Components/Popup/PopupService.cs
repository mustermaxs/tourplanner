namespace Client.Components;

using System;

public interface IPopupService
{
    void Show();
    void Hide();
    public event Action? OnShow;
    public event Action? OnHide;    
}

public class PopupService : IPopupService
{
    public event Action? OnShow;
    public event Action? OnHide;

    public void Show()
    {
        Console.WriteLine("SHOW DU OASCH");
        // OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}