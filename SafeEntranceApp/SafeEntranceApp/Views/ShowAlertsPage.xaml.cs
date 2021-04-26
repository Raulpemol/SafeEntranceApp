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
    public partial class ShowAlertsPage : ContentPage
    {
        private ShowAlertsViewModel viewModel;

        public ShowAlertsPage()
        {
            viewModel = new ShowAlertsViewModel();
            BindingContext = viewModel;

            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (viewModel.PopUpVisibility)
            {
                viewModel.PopUpVisibility = false;
                viewModel.IsButtonEnabled = true;

                viewModel.ClosePopUpCommand.Execute(null);

                return true;
            }
            else if (viewModel.InfoVisibility)
            {
                viewModel.InfoVisibility = false;

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