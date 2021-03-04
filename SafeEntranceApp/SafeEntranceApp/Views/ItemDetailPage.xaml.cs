using System.ComponentModel;
using Xamarin.Forms;
using SafeEntranceApp.ViewModels;

namespace SafeEntranceApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}