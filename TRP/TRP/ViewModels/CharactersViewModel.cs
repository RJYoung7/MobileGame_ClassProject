using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TRP.Models;
using TRP.Views;
using System.Linq;

namespace TRP.ViewModels
{
    public class CharactersViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static CharactersViewModel _instance;

        public static CharactersViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharactersViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Character> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        public CharactersViewModel()
        {

            Title = "Character List";
            Dataset = new ObservableCollection<Character>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // Implement 

        }

        // Return True if a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            // Implement 
            return false;
        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            // Implement 
        }

        private async Task ExecuteLoadDataCommand()
        {
            // Implement 
            return;
        }

        public void ForceDataRefresh()
        {
            // Implement 
        }

        #region DataOperations

        public async Task<bool> AddAsync(Character data)
        {
            // Implement 
            return false;
        }

        public async Task<bool> DeleteAsync(Character data)
        {
            // Implement 
            return false;
        }

        public async Task<bool> UpdateAsync(Character data)
        {
            // Implement 
            return false;
        }

        // Call to database to ensure most recent
        public async Task<Character> GetAsync(string id)
        {
            // Implement 
            return null;
        }

        #endregion DataOperations

    }
}