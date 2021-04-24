using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Essentials;
using Xamarin.Forms;
using Android.Content;
using SafeEntranceApp.Droid.Receivers;
using SafeEntranceApp.Common;

namespace SafeEntranceApp.Droid
{
    [Activity(Label = "SafeEntranceApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.SetFlags("RadioButton_Experimental");
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());

            RequestCameraPermissions();
            //ManageSyncAlarm();
        }

        protected override void OnDestroy()
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
            intent.PutExtra(Constants.TITLE_KEY, "TEST");
            intent.PutExtra(Constants.MESSAGE_KEY, "ESTO ES UNA PRUEBA");

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 10, intent, PendingIntentFlags.CancelCurrent);
            long triggerTime = GetNotifyTime(DateTime.Now.AddSeconds(10));
            AlarmManager alarmManager = Android.App.Application.Context.GetSystemService(AlarmService) as AlarmManager;
            alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);

            base.OnDestroy();
        }

        private async void RequestCameraPermissions()
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (!cameraStatus.Equals(Permission.Granted))
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ManageSyncAlarm()
        {
            AlarmManager alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            Intent intent = new Intent(this, typeof(AlarmReceiver));
            //SendBroadcast(intent);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, 0);
            alarmManager.Set(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + 10000, pendingIntent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(Constants.TITLE_KEY);
                string message = intent.GetStringExtra(Constants.MESSAGE_KEY);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
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