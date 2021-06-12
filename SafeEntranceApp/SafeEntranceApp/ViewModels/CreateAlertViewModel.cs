using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using SafeEntranceApp.Services.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SafeEntranceApp.ViewModels
{
    class CreateAlertViewModel : BaseViewModel
    {
        #region Properties
        private string _code = string.Empty;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
            }
        }

        private DateTime _symptomsDate;
        public DateTime SymptomsDate
        {
            get => _symptomsDate;
            set
            {
                SetProperty(ref _symptomsDate, value);
            }
        }

        private bool _isEntryEnabled = true;
        public bool IsEntryEnabled
        {
            get => _isEntryEnabled;
            set
            {
                SetProperty(ref _isEntryEnabled, value);
            }
        }

        private string _alertIcon;
        public string AlertIcon
        {
            get => _alertIcon;
            set
            {
                SetProperty(ref _alertIcon, value);
            }
        }

        private Color _alertColor;
        public Color AlertColor
        {
            get => _alertColor;
            set
            {
                SetProperty(ref _alertColor, value);
            }
        }
        #endregion

        #region Fields
        private VisitsService visitsService;
        private AlertsApiService alertsApiService;
        private CovidAlertsService alertsService;
        private EnvironmentVariablesService environmentService;
        #endregion

        #region Commands
        public ICommand CreateAlertCommand => new Command(CreateAlert);
        public ICommand CloseAlertCommand => new Command(() => { PopUpVisibility = false; IsEntryEnabled = true; });
        #endregion

        public CreateAlertViewModel()
        {
            Title = Constants.APP_NAME;
            SymptomsDate = DateTime.Now;
            TermsText = Constants.TERMS_AND_CONDITIONS;

            visitsService = new VisitsService();
            alertsApiService = new AlertsApiService();
            alertsService = new CovidAlertsService();
            environmentService = new EnvironmentVariablesService();

            TermsVisibility = Preferences.Get(Constants.IS_FIRST_START, true);
        }

        /*
         * Si todos los campos del formulario son correctos, genera una nueva alerta a partir
         * de las visitas registradas en el periodo de infectividad del usuario
         */
        private async void CreateAlert()
        {
            if (ValidateFields())
            {
                int infectDays = int.Parse((await environmentService.GetEnvironmentVariable(EnvironmentVariablesService.SYMPTOMS_DEVELOPING_DAYS)).Replace("\"",""));
                DateTime infectingDate = SymptomsDate.AddDays(-infectDays);
                List<Visit> visits = await visitsService.GetSelfInfected(infectingDate);

                CovidAlert alert = new CovidAlert { Code = Code, AlertDate = DateTime.Now, SymptomsDate = SymptomsDate, State = AlertState.CREADA };
                string centralID = await alertsApiService.InsertAlert(JsonParser.AlertToJSON(alert, visits));
                if(centralID != null && centralID != string.Empty)
                {
                    alert.CentralID = centralID.Replace("\"", "");
                    await alertsService.Save(alert);

                    Code = string.Empty;
                    PopUpTitle = Constants.ALERT_REGISTERED;
                    AlertIcon = Constants.CORRECT_ICON;
                    AlertColor = (Color)App.Current.Resources[Constants.RESOURCE_ALTERNATIVE];
                    PopUpVisibility = true;
                    IsEntryEnabled = false;
                }
                else
                {
                    PopUpTitle = Constants.SERVER_ERROR;
                    AlertIcon = Constants.ERROR_ICON;
                    AlertColor = (Color)App.Current.Resources[Constants.RESOURCE_ACCENT];
                    PopUpVisibility = true;
                    IsEntryEnabled = false;
                }
            }
        }

        /*
         * Comprueba que todos los campos del formulario de registro de positivos sean correctos
         */
        private bool ValidateFields()
        {
            if (SymptomsDate == null || SymptomsDate.CompareTo(DateTime.Now) == 1)
            {
                PopUpTitle = Constants.WRONG_DATE_MSG;
                AlertIcon = Constants.ERROR_ICON;
                AlertColor = (Color)App.Current.Resources[Constants.RESOURCE_ACCENT];
                PopUpVisibility = true;
                IsEntryEnabled = false;
                return false;
            }
            if (Code == null || Code == string.Empty || Code.Length < 12)
            {
                //TODO: Validate the code against sanitary servers. Will be implemented if in production

                PopUpTitle = Constants.WRONG_CODE_MSG;
                AlertIcon = Constants.ERROR_ICON;
                AlertColor = (Color)App.Current.Resources[Constants.RESOURCE_ACCENT];
                PopUpVisibility = true;
                IsEntryEnabled = false;
                return false;
            }

            return true;
        }
    }
}
