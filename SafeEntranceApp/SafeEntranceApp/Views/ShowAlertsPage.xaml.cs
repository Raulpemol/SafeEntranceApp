using SafeEntranceApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}