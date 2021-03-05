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

        private void OnActivateScanTapped(object sender, EventArgs e)
        {
            AnimateScanner();
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