using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using SafeEntranceApp.Models;
using SafeEntranceApp.Services;

namespace SafeEntranceApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

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

        bool _alertVisibility = false;
        public bool AlertVisibility
        {
            get => _alertVisibility;
            set
            {
                SetProperty(ref _alertVisibility, value);
            }
        }

        string _alertText = string.Empty;
        public string AlertText
        {
            get => _alertText;
            set
            {
                SetProperty(ref _alertText, value);
            }
        }

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
