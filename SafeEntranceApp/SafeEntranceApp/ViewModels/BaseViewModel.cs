using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using SafeEntranceApp.Models;
using SafeEntranceApp.Services;
using System.Windows.Input;
using Xamarin.Essentials;
using SafeEntranceApp.Common;

namespace SafeEntranceApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties
        bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set { SetProperty(ref _isBusy, value); }
        }

        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set { SetProperty(ref _title, value); }
        }

        bool _popUpVisibility = false;
        public bool PopUpVisibility
        {
            get => _popUpVisibility;
            set
            {
                SetProperty(ref _popUpVisibility, value);
            }
        }

        string _popUpTitle = string.Empty;
        public string PopUpTitle
        {
            get => _popUpTitle;
            set
            {
                SetProperty(ref _popUpTitle, value);
            }
        }

        private bool _termsVisibility;
        public bool TermsVisibility
        {
            get => _termsVisibility;
            set
            {
                SetProperty(ref _termsVisibility, value);
            }
        }

        private string _termsText;
        public string TermsText
        {
            get => _termsText;
            set
            {
                SetProperty(ref _termsText, value);
            }
        }
        #endregion

        public ICommand AcceptTermsCommand => new Command(() =>
        {
            Preferences.Set(Constants.IS_FIRST_START, false);
            TermsVisibility = false;
        });

        /*
         * Actualiza el enlace de datos de una propiedad
         */
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
