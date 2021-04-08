using SafeEntranceApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.ViewModels
{
    class ShowAlertsViewModel : BaseViewModel
    {
        private List<CovidContact> _alerts;
        public List<CovidContact> Alerts
        {
            get => _alerts;
            set
            {
                SetProperty(ref _alerts, value);
            }
        }
        public ShowAlertsViewModel()
        {
            Title = "SafeEntrance";
            Alerts = new List<CovidContact>
            { 
                new CovidContact { PlaceName = "Mi casa", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Tu bar", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Su restaurante", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Casa Juan", ContactDate = DateTime.Now},
                new CovidContact { PlaceName = "Ese bar es el mejor", ContactDate = DateTime.Now},
            };
        }
    }
}
