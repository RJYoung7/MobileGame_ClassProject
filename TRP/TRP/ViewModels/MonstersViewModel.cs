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
    public class MonstersViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static MonstersViewModel _instance;

        public static MonstersViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MonstersViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Monster> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        public MonstersViewModel()
        {
            Title = "Monster List";
            Dataset = new ObservableCollection<Monster>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // Implement 
            // Update Database: Delete Character
            MessagingCenter.Subscribe<MonsterDeletePage, Monster>(this, "DeleteData", async (obj, data) =>
            {
                Dataset.Remove(data);
                await DataStore.DeleteAsync_Monster(data);
            });


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
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Monster(true);

                // Example of how to sort the database output using a linq query.
                //Sort the list
                dataset = dataset
                    .OrderBy(a => a.Name)
                    .ThenBy(a => a.Description)
                    .ToList();

                // Then load the data structure
                foreach (var data in dataset)
                {
                    Dataset.Add(data);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            finally
            {
                IsBusy = false;
            }

        }

        public void ForceDataRefresh()
        {
            // Implement 
        }

        #region DataOperations

        public async Task<bool> AddAsync(Monster data)
        {
            // Implement 
            return false;
        }

        public async Task<bool> DeleteAsync(Monster data)
        {
            // Implement 
            return false;
        }

        public async Task<bool> UpdateAsync(Monster data)
        {
            // Implement 
            return false;
        }

        // Call to database to ensure most recent
        public async Task<Monster> GetAsync(string id)
        {
            // Implement 
            return null;
        }

        #endregion DataOperations

    }
}