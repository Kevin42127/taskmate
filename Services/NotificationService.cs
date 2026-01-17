using System;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;

namespace TaskMateApp.Services
{
    public class NotificationService
    {
        private WindowNotificationManager? _notificationManager;

        public void Initialize(Window window)
        {
            var topLevel = TopLevel.GetTopLevel(window);
            _notificationManager = new WindowNotificationManager(topLevel)
            {
                Position = NotificationPosition.TopRight,
                MaxItems = 3
            };
        }

        public void Show(string title, string message, NotificationType type = NotificationType.Information)
        {
            if (_notificationManager == null) return;

            Dispatcher.UIThread.Post(() =>
            {
                var notification = new Notification(title, message, type);
                _notificationManager.Show(notification);
            });
        }

        public void ShowSuccess(string message)
        {
            Show("成功", message, NotificationType.Success);
        }

        public void ShowError(string message)
        {
            Show("錯誤", message, NotificationType.Error);
        }

        public void ShowWarning(string message)
        {
            Show("警告", message, NotificationType.Warning);
        }

        public void ShowInfo(string message)
        {
            Show("資訊", message, NotificationType.Information);
        }
    }
}
