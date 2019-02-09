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

        // Constructor: returns instance if instantiated, otherwise creates instance if it's null 
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

        // Collection of Monsters
        public ObservableCollection<Monster> Dataset { get; set; }

        // Command to load data 
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        // Constructor: loads data and listens for broadcast from views
        public MonstersViewModel()
        {
            Title = "Monster List";
            Dataset = new ObservableCollection<Monster>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // Update Database: Delete monster
            MessagingCenter.Subscribe<MonsterDeletePage, Monster>(this, "DeleteData", async (obj, data) =>
            {
                await DeleteAsync(data);
            });

            // Update database: add monster
            MessagingCenter.Subscribe<MonsterNewPage, Monster>(this, "AddData", async (obj, data) =>
            {
                await AddAsync(data);
            });

            // Update database: modify monster
            MessagingCenter.Subscribe<MonsterEditPage, Monster>(this, "EditData", async (obj, data) =>
            {
                await UpdateAsync(data);

            });

        }

        // Return True if a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                return true;
            }

            return false;
        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            _needsRefresh = value;
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
                SetNeedsRefresh(false);
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

        // Refreshes data
        public void ForceDataRefresh()
        {
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }

        #region DataOperations

        public async Task<bool> AddAsync(Monster data)
        {
            Dataset.Add(data);
            var ret = await DataStore.AddAsync_Monster(data);
            return ret;
        }

        public async Task<bool> DeleteAsync(Monster data)
        {
            Dataset.Remove(data);
            var ret = await DataStore.DeleteAsync_Monster(data);
            return ret;
        }

        public async Task<bool> UpdateAsync(Monster data)
        {
            var ret = await DataStore.UpdateAsync_Monster(data);
            return ret;
        }

        // Call to database to ensure most recent
        public async Task<Monster> GetAsync(string id)
        {
            var ret = await DataStore.GetAsync_Monster(id);
            return ret;
        }

        #endregion DataOperations

    }
}