using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SafeEntranceApp.Services;
using SafeEntranceApp.Views;
using SafeEntranceApp.Common;
using Xamarin.Essentials;

namespace SafeEntranceApp
{
    public partial class App : Application
    {
        private ProcessAlertsService alertsService;
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            MainPage = new AppShell();

            bool firstStart = Preferences.Get(Constants.IS_FIRST_START, true);
            if (!firstStart)
            {
                alertsService = new ProcessAlertsService();
                FetchAlerts();
            }
        }

        private async void FetchAlerts()
        {
            int newAlerts = await alertsService.Process();

            if (newAlerts > 0)
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(true, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_ALERTS_MSG, Preferences.Get("next_sync", DateTime.Now));
            }
            else
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(true, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_NO_ALERTS_MSG, Preferences.Get("next_sync", DateTime.Now));
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
