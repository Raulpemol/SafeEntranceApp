using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using SafeEntranceApp.ViewModels;
using System.Threading.Tasks;

namespace SafeEntranceApp.Views
{
    public partial class ScannerPage : ContentPage
    {
        ScannerViewModel viewModel;
        
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
            };
            activateScanFrame.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Frame activateScanFrame = FindByName("activateScanFrame") as Frame;
                activateScanFrame.BackgroundColor = (Color)App.Current.Resources["Accent"];
                viewModel.ProcessCode(result);
            });
        }

        private async void OnActivateScanTapped(object sender, EventArgs e)
        {
            Image scanPlaceholder = FindByName("scanPlaceholder") as Image;
            var scannerView = FindByName("scannerView") as View;
            if (scanPlaceholder.IsVisible)
            {
                await scanPlaceholder.FadeTo(0, 200);
                scanPlaceholder.IsVisible = false;
                viewModel.ActivateScanCommand.Execute(null);
            }
            else
            {
                scanPlaceholder.IsVisible = true;
                viewModel.ActivateScanCommand.Execute(null);
                await scanPlaceholder.FadeTo(1, 200);
            }
        }

        private async void AnimateScanner()
        {
            BoxView scanLine = FindByName("scanLine") as BoxView;
            do
            {
                await scanLine.FadeTo(0, 1000);
                await scanLine.FadeTo(1, 1000);
            } while (true);
        }
    }
}