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

        private bool _hasAutoSync;
        public bool HasAutoSync
        {
            get => _hasAutoSync;
            set
            {
                SetProperty(ref _hasAutoSync, value);
            }
        }

        private bool _infoVisibility;
        public bool InfoVisibility
        {
            get => _infoVisibility;
            set
            {
                SetProperty(ref _infoVisibility, value);
            }
        }

        public string InfoText { get; }
        #endregion

        #region Fields
        private CovidContactService contactService;
        private ProcessAlertsService processAlertsService;

        private string[] syncOptionsText { get; }
        #endregion

        #region Commands
        public ICommand RefreshListCommand => new Command(RefreshList);
        public ICommand ClosePopUpCommand => new Command(SelectSyncOption);
        public ICommand OpenPopUpCommand => new Command(() => { PopUpVisibility = true; IsButtonEnabled = false; });
        public ICommand CheckedCommand => new Command(ChangeSyncFrequency);
        public ICommand InfoPopUpCommand => new Command(() => InfoVisibility = !InfoVisibility);
        #endregion

        public ShowAlertsViewModel()
        {
            Title = Constants.APP_NAME;
            PopUpTitle = Constants.SYNC_FREQUENCY_MSG;
            IsButtonEnabled = true;
            InfoText = Constants.ALERTS_HELP_TEXT;

            SyncOptions = new bool[4];
            syncOptionsText = Constants.SYNC_OPTIONS_TEXT;

            contactService = new CovidContactService();
            processAlertsService = new ProcessAlertsService();


            GetData();
        }

        private async void GetData()
        {
            HasAutoSync = Preferences.Get(Constants.AUTO_SYNC_PREFERENCE, false);
            int checkedSyncOption = Preferences.Get(Constants.SYNC_PERIOD_PREFERENCE, 0);
            SyncOptions[checkedSyncOption] = true;
            SelectedOptionText = syncOptionsText[checkedSyncOption];
            Alerts = await contactService.GetAll();
        }

        private async void RefreshList()
        {
            int newAlerts = await processAlertsService.Process();

            if (newAlerts > 0)
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(true, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_ALERTS_MSG, Preferences.Get(Constants.NEXT_SYNC_PREFERENCE, DateTime.Now));
            }
            else
            {
                DependencyService.Get<INotificationManager>()
                    .SendNotification(true, Constants.NOTIFICATION_TITLE, Constants.NOTIFICATION_NO_ALERTS_MSG, Preferences.Get(Constants.NEXT_SYNC_PREFERENCE, DateTime.Now));
            }

            List<CovidContact> totalAlerts = await contactService.GetAll();
            Alerts = totalAlerts;

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
                    Preferences.Set(Constants.SYNC_PERIOD_PREFERENCE, i);
                    DateTime lastSync = Preferences.Get(Constants.LAST_SYNC_PREFERENCE, DateTime.Now);
                    Preferences.Set(Constants.NEXT_SYNC_PREFERENCE, lastSync.AddSeconds(Constants.SYNC_FREQUENCIES[i])); // CAMBIAR DE HORAS A SEGUNDOS ENTRE PRE Y PRO
                    break;
                }
            }
            
            PopUpVisibility = false; 
            IsButtonEnabled = true;
        }
    }
}
