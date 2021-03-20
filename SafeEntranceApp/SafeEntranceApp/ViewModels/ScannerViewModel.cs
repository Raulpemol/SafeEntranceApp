using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using System;
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
                // LoadUserState
                // Update ActionEnabled
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
        private VisitsService visitsService;
        #endregion

        #region Commands
        public ICommand ActivateScanCommand => new Command(() => ScannerVisibility = !ScannerVisibility);
        #endregion

        public ScannerViewModel()
        {
            Title = "SafeEntrance";
            GetData();
            ScannerVisibility = false;
        }

        private void GetData()
        {
            codeProcessor = new CodeProcessor();
            visitsService = new VisitsService();

            IsInside = Preferences.Get("user_state", true);
            if (IsInside)
            {
                ActionEnabled = "Salir del local";
                ScanButtonColor = (Color)App.Current.Resources["Accent"];
                DoorSourceImage = "door_open.png";
            }
            else
            {
                ActionEnabled = "Entrar a un local";
                ScanButtonColor = (Color)App.Current.Resources["SecondaryAccent"];
                DoorSourceImage = "door_closed.png";
            }
        }

        public void ProcessCode(Result result)
        {
            string placeId = codeProcessor.ProcessResult(result);

            if (validatePlace(placeId))
            {
                if (IsInside)
                {
                    ActionEnabled = "Entrar a un local";
                    ScanButtonColor = (Color)App.Current.Resources["SecondaryAccent"];
                    DoorSourceImage = "door_closed.png";
                }
                else
                {
                    ActionEnabled = "Salir del local";
                    ScanButtonColor = (Color)App.Current.Resources["Accent"];
                    DoorSourceImage = "door_open.png";
                }

                IsInside = !IsInside;
                Preferences.Set("user_state", IsInside);

            }
        }

        private bool validatePlace(string placeId)
        {
            if (placeId.Equals(string.Empty))
            {
                return false;
            }
            return true;
        }
    }
}