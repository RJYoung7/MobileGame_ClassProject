using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using TRP.Services;
using TRP.Models;

namespace TRP.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region RefactorLater
        // Mock datastore
        private IDataStore DataStoreMock => DependencyService.Get<IDataStore>() ?? MockDataStore.Instance;

        // SQL datastore
        private IDataStore DataStoreSql => DependencyService.Get<IDataStore>() ?? SQLDataStore.Instance;

        // Set the datastore to mockdatstore instance
        public IDataStore DataStore = MockDataStore.Instance;

        // Constructor:  Set the datastore in view model to the mock datastore
        public BaseViewModel()
        {
            SetDataStore(DataStoreEnum.Mock);
        }

        // Method to set datastore to either mock or sql
        public void SetDataStore(DataStoreEnum data)
        {
            switch (data)
            {
                case DataStoreEnum.Mock:
                    DataStore = DataStoreMock;
                    break;

                case DataStoreEnum.SQL:
                case DataStoreEnum.Unknown:
                default:
                    DataStore = DataStoreSql;
                    break;
            }
        }
        #endregion

        // Bool to tell if datastore is busy
        private bool _isBusy = false;

        // Methods to get and set _isBusy bool
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        // String to hold the title of the viewmodel
        private string _title = string.Empty;

        // Methods to get and set the title
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        // Method to set the property
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
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
        // Event handler for the detect change property
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to handle the property change
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
