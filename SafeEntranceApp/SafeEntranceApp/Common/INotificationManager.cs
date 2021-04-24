using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Common
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(bool manualRefresh, string title, string message, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message);
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
