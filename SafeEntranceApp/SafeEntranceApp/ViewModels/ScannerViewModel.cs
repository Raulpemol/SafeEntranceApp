using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using SafeEntranceApp.Services.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;


namespace SafeEntranceApp.ViewModels
{
    public class ScannerViewModel : BaseViewModel
    {
        #region Properties
        private string _actionEnabled;
        public string ActionEnabled
        {
            get => _actionEnabled;
            set
            {
                SetProperty(ref _actionEnabled, value);
            }
        }

        private string _doorSourceImage;
        public string DoorSourceImage
        {
            get => _doorSourceImage;
            set
            {
                SetProperty(ref _doorSourceImage, value);
            }
        }

        private bool _scannerVisibility;
        public bool ScannerVisibility
        {
            get => _scannerVisibility;
            set
            {
                SetProperty(ref _scannerVisibility, value);
            }
        }

        private bool _isInside;
        public bool IsInside
        {
            get => _isInside;
            set
            {
                SetProperty(ref _isInside, value);
            }
        }

        private Color _scanButtonColor;
        public Color ScanButtonColor
        {
            get => _scanButtonColor;
            set
            {
                SetProperty(ref _scanButtonColor, value);
            }
        }
        #endregion

        #region Fields
        private CodeProcessor codeProcessor;
        private PlacesApiService placesApiService;
        private VisitsService visitsService;

        private int currentVisitId;
        #endregion

        #region Commands
        public ICommand ActivateScanCommand => new Command(() => ScannerVisibility = !ScannerVisibility);
        public ICommand CloseAlertCommand => new Command(() => PopUpVisibility = false);
        #endregion

        public ScannerViewModel()
        {
            Title = Constants.APP_NAME;
            PopUpTitle = Constants.WRONG_QR_MSG;
            TermsText = Constants.TERMS_AND_CONDITIONS;
            GetData();
            ScannerVisibility = false;
        }

        private void GetData()
        {
            TermsVisibility = Preferences.Get(Constants.IS_FIRST_START, true);

            codeProcessor = new CodeProcessor();
            placesApiService = new PlacesApiService();
            visitsService = new VisitsService();

            IsInside = Preferences.Get(Constants.USER_STATE_PREFERENCE, false);
            currentVisitId = Preferences.Get(Constants.CURRENT_VISIT_PREFERENCE, 0);
            if (IsInside)
            {
                ActionEnabled = Constants.EXIT_PLACE_ACTION;
                ScanButtonColor = (Color)App.Current.Resources[Constants.RESOURCE_ACCENT];
                DoorSourceImage = Constants.DOOR_OPEN;
            }
            else
            {
                ActionEnabled = Constants.ENTER_PLACE_ACTION;
                ScanButtonColor = (Color)App.Current.Resources[Constants.RESOURCE_SECONDARY_ACCENT];
                DoorSourceImage = Constants.DOOR_CLOSED;
            }
        }

        public async void ProcessCode(Result result)
        {
            string placeId = codeProcessor.ProcessResult(result);

            if (await ValidatePlace(placeId))
            {
                DateTime scanTime = DateTime.Now;

                if (IsInside)
                {
                    Visit currentVisit = await visitsService.GetById(currentVisitId);
                    if (currentVisit.PlaceID.Equals(placeId))
                    {
                        ExitPlace(currentVisit, scanTime);
                    }
                    else
                    {
                        currentVisit.ExitDateTime = scanTime;
                        await visitsService.Save(currentVisit);
                        EnterPlace(placeId, scanTime);
                    }
                }
                else
                {
                    EnterPlace(placeId, scanTime);
                }

                IsInside = !IsInside;
                PopUpVisibility = false;
                Preferences.Set(Constants.USER_STATE_PREFERENCE, IsInside);
            }
            else
            {
                PopUpVisibility = true;
            }
        }

        private async Task<bool> ValidatePlace(string placeId)
        {
            if (placeId.Equals(string.Empty))
                return false;

            string validPlaceId = await placesApiService.GetPlace(placeId);

            if (validPlaceId == null)
                return false;

            return true;
        }

        private async void EnterPlace(string placeId, DateTime scanTime)
        {
            Visit currentVisit = new Visit { PlaceID = placeId, EnterDateTime = scanTime };
            if(await visitsService.Save(currentVisit) == 1)
            {
                currentVisitId = currentVisit.ID;

                ActionEnabled = Constants.EXIT_PLACE_ACTION;
                ScanButtonColor = (Color)App.Current.Resources[Constants.RESOURCE_ACCENT];
                DoorSourceImage = Constants.DOOR_OPEN;

                Preferences.Set(Constants.CURRENT_VISIT_PREFERENCE, currentVisitId);
            }
        }

        private async void ExitPlace(Visit currentVisit, DateTime scanTime)
        {
            currentVisit.ExitDateTime = scanTime;
            if(await visitsService.Save(currentVisit) == 1)
            {
                ActionEnabled = Constants.ENTER_PLACE_ACTION;
                ScanButtonColor = (Color)App.Current.Resources[Constants.RESOURCE_SECONDARY_ACCENT];
                DoorSourceImage = Constants.DOOR_CLOSED;

                currentVisitId = 0;
                Preferences.Set(Constants.CURRENT_VISIT_PREFERENCE, 0);
            }
        }
    }
}