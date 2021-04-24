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
using SafeEntranceApp.Common;
using SafeEntranceApp.Services;

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

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                SetProperty(ref _isButtonEnabled, value);
            }
        }

        private bool[] _syncOptions;
        public bool[] SyncOptions
        {
            get => _syncOptions;
            set
            {
                SetProperty(ref _syncOptions, value);
            }
        }

        private string _selectedOptionText;
        public string SelectedOptionText
        {
            get => _selectedOptionText;
            set
            {
                SetProperty(ref _selectedOptionText, value);
            }
        }
        #endregion

        #region Fields
        private AlertsApiService alertsApiService;
        private VisitsService visitsService;
        private EnvironmentVariablesService environmentService;
        private PlacesApiService placesService;
        private CovidContactService contactService;

        private INotificationManager notificationManager;

        private string[] syncOptionsText { get; }
        #endregion

        #region Commands
        public ICommand RefreshListCommand => new Command(RefreshList);
        public ICommand ClosePopUpCommand => new Command(SelectSyncOption);
        public ICommand OpenPopUpCommand => new Command(() => { PopUpVisibility = true; IsButtonEnabled = false; });
        public ICommand CheckedCommand => new Command(ChangeSyncFrequency);
        #endregion

        public ShowAlertsViewModel()
        {
            Title = "SafeEntrance";
            PopUpTitle = Constants.SYNC_FREQUENCY_MSG;
            IsButtonEnabled = true;

            SyncOptions = new bool[4];
            syncOptionsText = new string[4] { "1 hora", "5 horas", "12 horas", "1 día" };

            alertsApiService = new AlertsApiService();
            visitsService = new VisitsService();
            environmentService = new EnvironmentVariablesService();
            placesService = new PlacesApiService();
            contactService = new CovidContactService();

            notificationManager = DependencyService.Get<INotificationManager>();

            GetData();
        }

        private async void GetData()
        {
            int checkedSyncOption = Preferences.Get("sync_period", 0);
            SyncOptions[checkedSyncOption] = true;
            SelectedOptionText = syncOptionsText[checkedSyncOption];
            Alerts = await contactService.GetAll();
        }

        private async void RefreshList()
        {
            int newAlerts = 0;
            DateTime syncDate = DateTime.Now;

            int daysAfterInfection = int.Parse((await environmentService.GetDaysAfterPossibleInfection()).Replace("\"", ""));
            int minutesForContact = int.Parse((await environmentService.GetMinutesForContact()).Replace("\"", ""));
            DateTime minDate = DateTime.Now.AddDays(-daysAfterInfection);

            var visits = await visitsService.GetAfterDate(minDate);
            
            if(visits.Count > 0)
            {
                DateTime lastSync = Preferences.Get("last_sync", DateTime.MinValue);
                List<CovidContact> contacts = await alertsApiService.GetPossibleContacts(visits, minutesForContact, lastSync);

                if(contacts != null)
                {
                    contacts.ForEach(c => c.PlaceName = Task.Run(() => placesService.GetPlaceName(c.PlaceID)).Result.Replace("\"", ""));
                    contacts.ForEach(c => Task.Run(() => contactService.Save(c)));

                    newAlerts = contacts.Count;
                }
            }

            if (newAlerts > 0)
            {
                DependencyService.Get<INotificationManager>().SendNotification(true, "Sincronización completada", "Cuidado, hay nuevas alertas");
            }
            else
            {
                DependencyService.Get<INotificationManager>().SendNotification(true, "Sincronización completada", "No hay nuevas alertas que te afecten", DateTime.Now.AddSeconds(10));
                //DependencyService.Get<IBackgroundService>().Start(DateTime.Now.AddSeconds(10));
            }

            List<CovidContact> totalAlerts = await contactService.GetAll();
            Alerts = totalAlerts;

            Preferences.Set("last_sync", syncDate);
            IsRefreshing = false;
        }

        private void ChangeSyncFrequency(object param)
        {
            int option = int.Parse((string) param);
            for(int i = 0; i < SyncOptions.Length; i++)
            {
                if (SyncOptions[i] && i != option)
                {
                    SyncOptions[i] = false;
                }
            }
        }

        private void SelectSyncOption()
        {
            for (int i = 0; i < SyncOptions.Length; i++) {
                if (SyncOptions[i])
                {
                    SelectedOptionText = syncOptionsText[i];
                    Preferences.Set("sync_period", i);
                    break;
                }
            }
            
            PopUpVisibility = false; 
            IsButtonEnabled = true;
        }
    }
}
