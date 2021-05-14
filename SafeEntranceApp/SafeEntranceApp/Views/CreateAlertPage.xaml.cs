using SafeEntranceApp.Common;
using SafeEntranceApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeEntranceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAlertPage : ContentPage
    {
        private CreateAlertViewModel viewModel;

        public CreateAlertPage()
        {
            InitializeComponent();

            viewModel = new CreateAlertViewModel();
            BindingContext = viewModel;
        }

        public void OnTextChanged(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            entry.TextColor = (Color)App.Current.Resources[Constants.RESOURCE_TEXT_BLACK];
        }

        protected override bool OnBackButtonPressed()
        {
            if (viewModel.PopUpVisibility)
            {
                viewModel.PopUpVisibility = false;
                viewModel.IsEntryEnabled = true;
                return true;
            }
            else
            {
                Shell.Current.GoToAsync("//ScannerPage");
                return true;
            }
        }
    }
}