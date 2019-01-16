using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;

using TRP.Models;
using TRP.Views;
using TRP.GameEngine;

namespace TRP.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        #region Singleton
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static ItemsViewModel _instance;

        public static ItemsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemsViewModel();
                }
                return _instance;
            }
        }

        #endregion Singleton

        public ObservableCollection<Item> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        public ItemsViewModel()
        {

            Title = "Item List";
            Dataset = new ObservableCollection<Item>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            #region Messages
            MessagingCenter.Subscribe<ItemDeletePage, Item>(this, "DeleteData", async (obj, data) =>
            {
                await DeleteAsync(data);
            });

            MessagingCenter.Subscribe<ItemNewPage, Item>(this, "AddData", async (obj, data) =>
            {
                await AddAsync(data);
            });

            MessagingCenter.Subscribe<ItemEditPage, Item>(this, "EditData", async (obj, data) =>
            {
                await UpdateAsync(data);
            });

            #endregion Messages
        }

        #region Refresh
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
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Item(true);

                // Example of how to sort the database output using a linq query.
                //Sort the list
                dataset = dataset
                    .OrderBy(a => a.Name)
                    .ThenBy(a => a.Location)
                    .ThenBy(a => a.Attribute)
                    .ThenByDescending(a => a.Value)
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
            // Reset
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }

        #endregion Refresh

        #region DataOperations

        public async Task<bool> AddAsync(Item data)
        {
            Dataset.Add(data);
            var myReturn = await DataStore.AddAsync_Item(data);
            return myReturn;
        }

        public async Task<bool> DeleteAsync(Item data)
        {
            Dataset.Remove(data);
            var myReturn = await DataStore.DeleteAsync_Item(data);
            return myReturn;
        }

        public async Task<bool> UpdateAsync(Item data)
        {
            // Find the Item, then update it
            var myData = Dataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);
            await DataStore.UpdateAsync_Item(myData);

            _needsRefresh = true;

            return true;
        }

        // Call to database to ensure most recent
        public async Task<Item> GetAsync(string id)
        {
            var myData = await DataStore.GetAsync_Item(id);
            return myData;
        }

        // Having this at the ViewModel, because it has the DataStore
        // That allows the feature to work for both SQL and the MOCk datastores...
        public async Task<bool> InsertUpdateAsync(Item data)
        {
            var myReturn = await DataStore.InsertUpdateAsync_Item(data);
            return myReturn;
        }

        public Item CheckIfItemExists(Item data)
        {
            // This will walk the items and find if there is one that is the same.
            // If so, it returns the item...

            var myList = Dataset.Where(a =>
                                        a.Attribute == data.Attribute &&
                                        a.Name == data.Name &&
                                        a.Location == data.Location &&
                                        a.Range == data.Range &&
                                        a.Value == data.Value &&
                                        a.Damage == data.Damage)
                                        .FirstOrDefault();

            if (myList == null)
            {
                // it's not a match, return false;
                return null;
            }

            return myList;
        }

        #endregion DataOperations

        #region ItemConversion

        // Takes an item string ID and looks it up and returns the item
        // This is because the Items on a character are stores as strings of the GUID.  That way it can be saved to the DB.
        public Item GetItem(string ItemID)
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                return null;
            }

            Item myData = DataStore.GetAsync_Item(ItemID).GetAwaiter().GetResult();
            if (myData == null)
            {
                return null;
            }

            return myData;
        }

        #endregion ItemConversion

        // Return a random item from the list of items...
        public string ChooseRandomItemString(ItemLocationEnum location, AttributeEnum attribute)
        {
            // Implement 

            return null;
        }
    }
}