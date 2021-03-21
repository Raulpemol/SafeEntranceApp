using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SafeEntranceApp.Services;
using SafeEntranceApp.Views;

namespace SafeEntranceApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
