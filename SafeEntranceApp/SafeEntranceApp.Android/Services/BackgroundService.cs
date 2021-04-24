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
using SafeEntranceApp.Droid.Receivers;
using SafeEntranceApp.Services;

[assembly:Xamarin.Forms.Dependency(typeof(SafeEntranceApp.Droid.Services.BackgroundService))]
namespace SafeEntranceApp.Droid.Services
{
    [Service]
    class BackgroundService : Service
    {
        private static AlarmReceiver m_ScreenOffReceiver;

        public override IBinder OnBind(Intent intent)
        {
            return null;
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

        //From this thread: https://stackoverflow.com/questions/20592366/the-process-of-the-service-is-killed-after-the-application-is-removed-from-the-a
        public override void OnTaskRemoved(Intent rootIntent)
        {
            Intent restartServiceIntent = new Intent(Application.Context, typeof(BackgroundService));
            restartServiceIntent.SetPackage(PackageName);

            PendingIntent restartServicePendingIntent = PendingIntent.GetService(Application.Context, 1, restartServiceIntent, PendingIntentFlags.OneShot);
            AlarmManager alarmService = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            alarmService.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1000, restartServicePendingIntent);
            base.OnTaskRemoved(rootIntent);
        }

        private void registerScreenOffReceiver()
        {
            m_ScreenOffReceiver = new AlarmReceiver();
            IntentFilter filter = new IntentFilter("com.uniovi.safeentranceapp.TEST");
            RegisterReceiver(m_ScreenOffReceiver, filter);
        }
    }
}