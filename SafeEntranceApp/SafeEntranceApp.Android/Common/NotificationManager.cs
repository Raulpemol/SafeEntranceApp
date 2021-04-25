using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Xamarin.Forms;
using SafeEntranceApp.Common;
using AndroidApp = Android.App.Application;
using SafeEntranceApp.Droid.Receivers;
using SafeEntranceApp.Services.Server;
using SafeEntranceApp.Services.Database;
using Xamarin.Essentials;
using SafeEntranceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeEntranceApp.Services;

[assembly: Dependency(typeof(SafeEntranceApp.Droid.Common.NotificationManager))]
namespace SafeEntranceApp.Droid.Common
{
    [Service(Exported = true, Name = "com.uniovi.safeentranceapp.fetch")]
    public class NotificationManager : INotificationManager
    {
        private ProcessAlertsService processAlertsService;


        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";


        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 1;

        Android.App.NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static NotificationManager Instance { get; private set; }

        public NotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                processAlertsService = new ProcessAlertsService();

                CreateNotificationChannel();
                Instance = this;
            }
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void SendNotification(bool manualRefresh, string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }
            if (manualRefresh)
            {
                Show(title, message);
            }

            if (notifyTime != null)
            {
                Intent intent = new Intent(AndroidApp.Context, typeof(AlarmReceiver));
                intent.PutExtra(Constants.TITLE_KEY, title);
                intent.PutExtra(Constants.MESSAGE_KEY, message);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Process(title, message);
            }
        }

        public async void Process(string title, string message)
        {
            int newAlerts = await processAlertsService.Process();

            if (newAlerts > 0)
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(false, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_ALERTS_MSG, Preferences.Get("next_sync", DateTime.Now));
            }
            else
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(false, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_NO_ALERTS_MSG, Preferences.Get("next_sync", DateTime.Now));
            }

            Show(title, message);
        }

        private void Show(string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(Constants.TITLE_KEY, title);
            intent.PutExtra(Constants.MESSAGE_KEY, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

            var notification = new Notification.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.shield_alert)
                .Build();

            manager.Notify(messageId++, notification);
        }

        void CreateNotificationChannel()
        {
            manager = (Android.App.NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }
    }
}