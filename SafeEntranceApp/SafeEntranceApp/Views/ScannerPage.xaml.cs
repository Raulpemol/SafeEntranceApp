using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using SafeEntranceApp.ViewModels;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;
using Xamarin.Essentials;
using SafeEntranceApp.Common;

namespace SafeEntranceApp.Views
{
    public partial class ScannerPage : ContentPage
    {
        private ScannerViewModel viewModel;

        private ZXingScannerView scanner;

        public ScannerPage()
        {
            InitializeComponent();

            viewModel = new ScannerViewModel();
            BindingContext = viewModel;
            LoadCustomComponents();
        }

        protected override void OnAppearing()
        {
            viewModel.TermsVisibility = Preferences.Get(Constants.IS_FIRST_START, true);
            base.OnAppearing();
        }

        private void LoadCustomComponents()
        {
            Frame activateScanFrame = FindByName("activateScanFrame") as Frame;
            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                var fadeOutAnimationTask = activateScanFrame.FadeTo(0.3, 200);
                await Task.WhenAll(fadeOutAnimationTask);
                var fadeInAnimationTask = activateScanFrame.FadeTo(1, 200);
                await Task.WhenAll(fadeInAnimationTask);

                ActivateScan();
            };
            activateScanFrame.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private async void ActivateScan()
        {
            Frame scanPlaceholder = FindByName("scanPlaceholder") as Frame;
            if (scanPlaceholder.IsVisible)
            {
                await scanPlaceholder.FadeTo(0, 200);
                scanPlaceholder.IsVisible = false;
                
                CreateScanner();
                viewModel.ActivateScanCommand.Execute(null);
            }
            else
            {
                scanPlaceholder.IsVisible = true;
                await scanPlaceholder.FadeTo(1, 200);
                ReleaseScanner();
                viewModel.ActivateScanCommand.Execute(null);
            }
        }

        public void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ActivateScan();
                viewModel.ProcessCode(result);
            });
        }

        private void CreateScanner()
        {
            StackLayout scannerContainer = FindByName("scannerContainer") as StackLayout;

            ReleaseScanner();

            Device.BeginInvokeOnMainThread(() =>
            {
                scanner = new ZXingScannerView()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 310,
                    WidthRequest = 300,
                    IsScanning = true,
                    IsAnalyzing = true
                };
                scanner.OnScanResult += OnScanResult;

                scannerContainer.Children.Add(scanner);
            });
                           
        }

        private void ReleaseScanner()
        {
            StackLayout scannerContainer = FindByName("scannerContainer") as StackLayout;

            Device.BeginInvokeOnMainThread(() => {
                if (scanner != null)
                {
                    scanner.IsTorchOn = false;
                    scanner.IsAnalyzing = false;
                    scanner.IsScanning = false;
                    scanner.IsVisible = false;
                    scanner.OnScanResult -= OnScanResult;
                    scannerContainer.Children.Remove(scanner);
                    scanner = null;
                }
            });
        }

        protected override bool OnBackButtonPressed()
        {
            if (viewModel.PopUpVisibility)
            {
                viewModel.PopUpVisibility = false;
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}