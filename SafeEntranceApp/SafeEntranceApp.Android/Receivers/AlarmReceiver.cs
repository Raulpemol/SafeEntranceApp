using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SafeEntranceApp.Common;
using SafeEntranceApp.Droid.Common;
using SafeEntranceApp.Services;
using Xamarin.Forms;

namespace SafeEntranceApp.Droid.Receivers
{
    [BroadcastReceiver(Enabled = true, Label = "Notifications Broadcast Receiver")]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(Constants.TITLE_KEY);
                string message = intent.GetStringExtra(Constants.MESSAGE_KEY);

                NotificationManager manager = NotificationManager.Instance ?? new NotificationManager();
                manager.Process(title, message);
            }
        }
    }
}