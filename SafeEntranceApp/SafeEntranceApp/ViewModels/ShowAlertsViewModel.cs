using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using SafeEntranceApp.Services.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;

namespace SafeEntranceApp.ViewModels
{
    class ShowAlertsViewModel : BaseViewModel
    {
        #region Properties
        private List<CovidContact> _alerts;
        public List<CovidContact> Alerts
        {
            get => _alerts;
            set
            {
                SetProperty(ref _alerts, value);
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }
        #endregion

        #region Fields
        private AlertsApiService alertsApiService;
        private VisitsService visitsService;
        private EnvironmentVariablesService environmentService;
        private PlacesApiService placesService;
        private CovidContactService contactService;
        #endregion

        #region Commands
        public ICommand RefreshListCommand => new Command(RefreshList);
        #endregion

        public ShowAlertsViewModel()
        {
            Title = "SafeEntrance";

            alertsApiService = new AlertsApiService();
            visitsService = new VisitsService();
            environmentService = new EnvironmentVariablesService();
            placesService = new PlacesApiService();
            contactService = new CovidContactService();

            GetData();
        }

        private async void GetData()
        {
            Alerts = await contactService.GetAll();
        }

        private async void RefreshList()
        {
            DateTime syncDate = DateTime.Now;

            int daysAfterInfection = int.Parse((await environmentService.GetDaysAfterPossibleInfection()).Replace("\"", ""));
            int minutesForContact = int.Parse((await environmentService.GetMinutesForContact()).Replace("\"", ""));
            DateTime minDate = DateTime.Now.AddDays(-daysAfterInfection);

            var visits = await visitsService.GetAfterDate(minDate);
            
            if(visits.Count > 0)
            {
                DateTime lastSync = Preferences.Get("last_sync", DateTime.MinValue);
                List<CovidContact> contacts = await alertsApiService.GetPossibleContacts(visits, minutesForContact, lastSync);

                contacts.ForEach(c => c.PlaceName = Task.Run(() => placesService.GetPlaceName(c.PlaceID)).Result.Replace("\"", ""));
                contacts.ForEach(c => Task.Run(() => contactService.Save(c)));
            }

            List<CovidContact> totalAlerts = await contactService.GetAll();
            Alerts = totalAlerts;

            Preferences.Set("last_sync", syncDate);
            IsRefreshing = false;
        }
    }
}
