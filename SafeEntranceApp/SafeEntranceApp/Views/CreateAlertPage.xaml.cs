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
    }
}