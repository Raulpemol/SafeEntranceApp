using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using SafeEntranceApp.Services.Server;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SafeEntranceApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental");
            Forms.SetFlags("RadioButton_Experimental");
            Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Preferences.Set("auto_sync", true);

            LoadApplication(new App());

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
            );
            UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);

            return base.FinishedLaunching(app, options);
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            Window.RootViewController.PresentViewController(okayAlertController, true, null);

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            FetchNewAlerts();
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        private async void FetchNewAlerts()
        {
            AlertsApiService alertsApiService = new AlertsApiService();
            VisitsService visitsService = new VisitsService();
            EnvironmentVariablesService environmentService = new EnvironmentVariablesService();
            PlacesApiService placesService = new PlacesApiService();
            CovidContactService contactService = new CovidContactService();

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

            if (newAlerts > 0)
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(false, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_ALERTS_MSG);
            }
            else
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(false, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_NO_ALERTS_MSG);
            }
        }
    }
}
