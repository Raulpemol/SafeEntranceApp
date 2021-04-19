using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SafeEntranceApp.Services;

[assembly:Xamarin.Forms.Dependency(typeof(SafeEntranceApp.Droid.Services.BackgroundService))]
namespace SafeEntranceApp.Droid.Services
{
    [Service]
    class BackgroundService : Service, IBackgroundService
    {
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string START_SERVICE_ACTION = "START_SERVICE";

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if(intent.Action == START_SERVICE_ACTION)
            {
                ShowNotification();
                StopForeground(true);
            }
            return StartCommandResult.NotSticky;
        }

        public void Start()
        {
            Intent startIntent = new Intent(Application.Context, typeof(BackgroundService));
            startIntent.SetAction(START_SERVICE_ACTION);
            Application.Context.StartService(startIntent);
        }

        public void Stop()
        {
            StopForeground(true);
        }

        private void ShowNotification()
        {
            NotificationChannel channel = new NotificationChannel("SafeEntranceChannel", "Sync service", NotificationImportance.Max);
            NotificationManager manager = (NotificationManager)Application.Context.GetSystemService(NotificationService);

            manager.CreateNotificationChannel(channel);

            var notification = new Notification.Builder(this, "SafeEntranceChannel")
                .SetContentTitle("Sincronizando")
                .SetContentText("Buscando nuevas alertas que puedan afectarte")
                .SetSmallIcon(Resource.Drawable.shield_alert)
                //.SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction(BuildRestartTimerAction())
                //.AddAction(BuildStopServiceAction())
                .Build();

            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }
    }
}