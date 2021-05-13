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
            Title = "SafeEntrance";
            PopUpTitle = Constants.WRONG_QR_MSG;
            GetData();
            ScannerVisibility = false;
        }

        private void GetData()
        {
            codeProcessor = new CodeProcessor();
            placesApiService = new PlacesApiService();
            visitsService = new VisitsService();

            IsInside = Preferences.Get("user_state", false);
            currentVisitId = Preferences.Get("current_visit", 0);
            if (IsInside)
            {
                ActionEnabled = "Salir del local";
                ScanButtonColor = (Color)App.Current.Resources["Accent"];
                DoorSourceImage = Constants.DOOR_OPEN;
            }
            else
            {
                ActionEnabled = "Entrar a un local";
                ScanButtonColor = (Color)App.Current.Resources["SecondaryAccent"];
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
                Preferences.Set("user_state", IsInside);
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

                ActionEnabled = "Salir del local";
                ScanButtonColor = (Color)App.Current.Resources["Accent"];
                DoorSourceImage = Constants.DOOR_OPEN;

                Preferences.Set("current_visit", currentVisitId);
            }
        }

        private async void ExitPlace(Visit currentVisit, DateTime scanTime)
        {
            currentVisit.ExitDateTime = scanTime;
            if(await visitsService.Save(currentVisit) == 1)
            {
                ActionEnabled = "Entrar a un local";
                ScanButtonColor = (Color)App.Current.Resources["SecondaryAccent"];
                DoorSourceImage = Constants.DOOR_CLOSED;

                currentVisitId = 0;
                Preferences.Set("current_visit", 0);
            }
        }
    }
}