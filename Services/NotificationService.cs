using Avalonia.Controls;

namespace TodoApp.Services;

public class NotificationService
{
    private Window? _ownerWindow;

    public void Initialize(Window window)
    {
        _ownerWindow = window;
    }

    public void ShowSuccess(string message)
    {
        ShowMessage("成功", message);
    }

    public void ShowError(string message)
    {
        ShowMessage("錯誤", message);
    }

    public void ShowInfo(string message)
    {
        ShowMessage("資訊", message);
    }

    private void ShowMessage(string title, string message)
    {
        if (_ownerWindow != null)
        {
            var messageBox = new MessageBox
            {
                Title = title,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            messageBox.SetMessage(message);
            messageBox.ShowDialog(_ownerWindow);
        }
    }
}

public class MessageBox : Window
{
    private readonly TextBlock _textBlock;

    public MessageBox()
    {
        Width = 300;
        Height = 150;
        CanResize = false;
        Background = Avalonia.Media.Brushes.White;
        
        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 10
        };
        
        _textBlock = new TextBlock
        {
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            FontSize = 14
        };
        
        var button = new Button
        {
            Content = "確定",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Padding = new Avalonia.Thickness(20, 8)
        };
        
        button.Click += (s, e) => Close();
        
        panel.Children.Add(_textBlock);
        panel.Children.Add(button);
        Content = panel;
    }
    
    public void SetMessage(string message)
    {
        _textBlock.Text = message;
    }
}

