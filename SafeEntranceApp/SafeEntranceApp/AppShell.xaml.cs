using System;
using System.Collections.Generic;
using SafeEntranceApp.ViewModels;
using SafeEntranceApp.Views;
using Xamarin.Forms;

namespace SafeEntranceApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //ROUTING EXAMPLES
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
