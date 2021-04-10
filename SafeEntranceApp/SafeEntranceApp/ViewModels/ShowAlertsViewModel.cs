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
        }

        private async void RefreshList()
        {
            Alerts = new List<CovidContact>
            {
                new CovidContact { PlaceName = "Mi casa", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Tu bar", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Su restaurante", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Casa Juan", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Ese bar es el mejor", ContactDate = DateTime.Now},
            };

            int daysAfterInfection = int.Parse((await environmentService.GetDaysAfterPossibleInfection()).Replace("\"", ""));
            DateTime minDate = DateTime.Now.AddDays(-daysAfterInfection);
            var visits = await visitsService.GetAfterDate(minDate);
            

            if(visits.Count > 0)
            {
                List<CovidContact> contacts = await alertsApiService.GetPossibleContacts(visits);
            }

            IsRefreshing = false;
        }
    }
}
