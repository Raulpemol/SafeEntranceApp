using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using SafeEntranceApp.Services.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        #endregion

        #region Fields
        private VisitsService visitsService;
        private AlertsApiService alertsApiService;
        private CovidAlertsService alertsService;
        #endregion

        #region Commands
        public ICommand CreateAlertCommand => new Command(CreateAlert);
        public ICommand CloseAlertCommand => new Command(() => AlertVisibility = false);
        #endregion

        public CreateAlertViewModel()
        {
            Title = "SafeEntrance";
            SymptomsDate = DateTime.Now;

            visitsService = new VisitsService();
            alertsApiService = new AlertsApiService();
            alertsService = new CovidAlertsService();
        }

        private async void CreateAlert()
        {
            if (ValidateFields())
            {
                DateTime infectingDate = SymptomsDate.AddDays(-2); //Parameterize this value. Should be obtained from server
                List<Visit> visits = await visitsService.GetSelfInfected(infectingDate);

                CovidAlert alert = new CovidAlert { AlertDate = DateTime.Now, SymptomsDate = SymptomsDate, Visits = visits };
                string centralID = await alertsApiService.InsertAlert(alert.ToJSON());
                if(centralID != null && centralID != string.Empty)
                {
                    alert.CentralID = centralID;
                    await alertsService.Save(alert);

                    AlertText = Constants.ALERT_REGISTERED;
                    AlertVisibility = true;
                }
                else
                {
                    AlertText = Constants.SERVER_ERROR;
                    AlertVisibility = true;
                }
            }
        }

        private bool ValidateFields()
        {
            if (SymptomsDate == null || SymptomsDate.CompareTo(DateTime.Now) == 1)
            {
                AlertText = Constants.WRONG_DATE_MSG;
                AlertVisibility = true;
                return false;
            }
            if (Code == null || Code == string.Empty || Code.Length < 12)
            {
                //TODO: Validate the code against sanitary servers. Will be implemented if in production

                AlertText = Constants.WRONG_CODE_MSG;
                AlertVisibility = true;
                return false;
            }

            return true;
        }
    }
}
