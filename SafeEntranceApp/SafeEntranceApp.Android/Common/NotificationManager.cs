﻿using System;
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

[assembly: Dependency(typeof(SafeEntranceApp.Droid.Common.NotificationManager))]
namespace SafeEntranceApp.Droid.Common
{
    [Service(Exported = true, Name = "com.uniovi.safeentranceapp.fetch")]
    public class NotificationManager : Service, INotificationManager
    {
        private static AlarmReceiver m_ScreenOffReceiver;

        private AlertsApiService alertsApiService;
        private VisitsService visitsService;
        private EnvironmentVariablesService environmentService;
        private PlacesApiService placesService;
        private CovidContactService contactService;


        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";


        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        Android.App.NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static NotificationManager Instance { get; private set; }

        public NotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                alertsApiService = new AlertsApiService();
                visitsService = new VisitsService();
                environmentService = new EnvironmentVariablesService();
                placesService = new PlacesApiService();
                contactService = new CovidContactService();

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

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
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
            int newAlerts = 0;
            DateTime syncDate = DateTime.Now;

            int daysAfterInfection = int.Parse((await environmentService.GetDaysAfterPossibleInfection()).Replace("\"", ""));
            int minutesForContact = int.Parse((await environmentService.GetMinutesForContact()).Replace("\"", ""));
            DateTime minDate = DateTime.Now.AddDays(-daysAfterInfection);

            var visits = await visitsService.GetAfterDate(minDate);

            if (visits.Count > 0)
            {
                DateTime lastSync = Preferences.Get("last_sync", DateTime.MinValue);
                List<CovidContact> contacts = await alertsApiService.GetPossibleContacts(visits, minutesForContact, lastSync);

                if (contacts != null)
                {
                    contacts.ForEach(c => c.PlaceName = Task.Run(() => placesService.GetPlaceName(c.PlaceID)).Result.Replace("\"", ""));
                    contacts.ForEach(c => Task.Run(() => contactService.Save(c)));

                    newAlerts = contacts.Count;
                }
            }

            Preferences.Set("last_sync", syncDate);
            Preferences.Set("next_sync", syncDate.AddSeconds(Constants.SYNC_FREQUENCIES[Preferences.Get("sync_period", 0)]));

            if (newAlerts > 0)
            {
                DependencyService.Get<INotificationManager>().SendNotification(false, "Sincronización completada", "Cuidado, hay nuevas alertas");
            }
            else
            {
                DependencyService.Get<INotificationManager>().SendNotification(false, "Sincronización completada", "No hay nuevas alertas que te afecten", Preferences.Get("next_sync", DateTime.Now));
            }

            Show(title, message);
        }

        private void Show(string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(Constants.TITLE_KEY, title);
            intent.PutExtra(Constants.MESSAGE_KEY, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

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

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        public override void OnCreate()
        {
            registerScreenOffReceiver();
            base.OnCreate();
        }

        public override void OnDestroy()
        {
            UnregisterReceiver(m_ScreenOffReceiver);
            m_ScreenOffReceiver = null;
            base.OnDestroy();
        }
        private void registerScreenOffReceiver()
        {
            m_ScreenOffReceiver = new AlarmReceiver();
            IntentFilter filter = new IntentFilter("com.uniovi.safeentranceapp.TEST");
            RegisterReceiver(m_ScreenOffReceiver, filter);
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
            Intent restartServiceIntent = new Intent(Android.App.Application.Context, typeof(NotificationManager));
            restartServiceIntent.SetPackage(PackageName);

            PendingIntent restartServicePendingIntent = PendingIntent.GetService(AndroidApp.Context, 1, restartServiceIntent, PendingIntentFlags.OneShot);
            AlarmManager alarmService = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);
            long triggerTime = GetNotifyTime(DateTime.Now.AddSeconds(10));
            alarmService.Set(AlarmType.ElapsedRealtime, triggerTime, restartServicePendingIntent);
            base.OnTaskRemoved(rootIntent);
        }
    }
}