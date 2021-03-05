using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using SafeEntranceApp.ViewModels;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;

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
            Image scanPlaceholder = FindByName("scanPlaceholder") as Image;
            if (scanPlaceholder.IsVisible)
            {
                CreateScanner();
                await scanPlaceholder.FadeTo(0, 200);
                scanPlaceholder.IsVisible = false;
                viewModel.ActivateScanCommand.Execute(null);

            }
            else
            {

                scanPlaceholder.IsVisible = true;
                scanPlaceholder.FadeTo(1, 200);
                ReleaseScanner();
                viewModel.ActivateScanCommand.Execute(null);
            }
        }

        public void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                viewModel.ProcessCode(result);
                Frame activateScanFrame = FindByName("activateScanFrame") as Frame;
                if(viewModel.IsInside)
                    activateScanFrame.BackgroundColor = (Color)App.Current.Resources["Accent"];
                else
                    activateScanFrame.BackgroundColor = (Color)App.Current.Resources["SecondaryAccent"];
                ActivateScan();
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
    }
}