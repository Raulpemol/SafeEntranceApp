using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;


namespace SafeEntranceApp.ViewModels
{
    public class ScannerViewModel : BaseViewModel
    {
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

        private bool _scannerVisibility;
        public bool ScannerVisibility
        {
            get => _scannerVisibility;
            set
            {
                SetProperty(ref _scannerVisibility, value);
            }
        }

        public ICommand ActivateScanCommand => new Command(() => ScannerVisibility = !ScannerVisibility);

        public ScannerViewModel()
        {
            Title = "SafeEntrance";
            ActionEnabled = "Entra a un local";
            ScannerVisibility = false;
        }

        public void ProcessCode(Result result)
        {
            
        }

    }
}