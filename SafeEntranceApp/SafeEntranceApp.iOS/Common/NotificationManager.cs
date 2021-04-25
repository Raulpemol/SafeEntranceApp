using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SafeEntranceApp.Common;
using UIKit;

namespace SafeEntranceApp.iOS.Common
{
    public class NotificationManager : INotificationManager
    {
        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void ReceiveNotification(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void SendNotification(bool manualRefresh, string title, string message, DateTime? notifyTime = null)
        {
            UILocalNotification notification = new UILocalNotification();
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(15);
            notification.AlertAction = title;
            notification.AlertBody = message;
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}