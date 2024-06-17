using Client.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Client.Components
{
    public enum PopupStyle
    {
        Normal,
        Error
    };

    public class PopupViewModel : BaseViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = String.Empty;
        public bool IsOpen { get; private set; } = false;

        public void Open(string title, string message, PopupStyle style)
        {
            Title = title;
            Message = message;
            PopupStyle = style;
            IsOpen = true;
            _notifyStateChanged.Invoke();
        }

        public void Open()
        {
            IsOpen = true;
            _notifyStateChanged.Invoke();  
        }

        public void Close()
        {
            IsOpen = false;
            _notifyStateChanged.Invoke();
        }

        private void Clear()
        {
            Title = string.Empty;
            Message = string.Empty;
            PopupStyle = PopupStyle.Normal;
        }

        private PopupStyle _style { get; set; } = PopupStyle.Normal;

        public PopupStyle PopupStyle
        {
            get => _style;
            set { _style = value; }
        }
    }
}