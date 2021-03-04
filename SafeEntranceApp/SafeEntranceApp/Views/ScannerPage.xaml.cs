using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using SafeEntranceApp.ViewModels;

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
            AnimateScanner();
        }

        public void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Scanned result", "Barcode", "OK");
            });
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