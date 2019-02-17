using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TRP.Models;
using TRP.Views;
using System.Linq;
using TRP.Controllers;

namespace TRP.ViewModels
{
    public class ScoresViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static ScoresViewModel _instance;

        public static ScoresViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScoresViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Score> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        public ScoresViewModel()
        {
            Title = "Score List";
            Dataset = new ObservableCollection<Score>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            MessagingCenter.Subscribe<ScoreDeletePage, Score>(this, "DeleteData", async (obj, data) =>
            {
                Dataset.Remove(data);
                await DataStore.DeleteAsync_Score(data);
            });

            MessagingCenter.Subscribe<ScoreNewPage, Score>(this, "AddData", async (obj, data) =>
            {
                Dataset.Add(data);
                await DataStore.AddAsync_Score(data);
            });

            MessagingCenter.Subscribe<ScoreEditPage, Score>(this, "EditData", async (obj, data) =>
            {
                // Find the Score, then update it
                var myData = Dataset.FirstOrDefault(arg => arg.Id == data.Id);
                if (myData == null)
                {
                    return;
                }

                myData.Update(data);
                await DataStore.UpdateAsync_Score(myData);

                _needsRefresh = true;

            });
        }

        #region DataOperations

        public async Task<bool> AddAsync(Score data)
        {
            Dataset.Add(data);
            var ret = await DataStore.AddAsync_Score(data);
            return ret;
        }

        public async Task<bool> DeleteAsync(Score data)
        {
            Dataset.Remove(data);
            var ret = await DataStore.DeleteAsync_Score(data);
            return ret;
        }

        public async Task<bool> UpdateAsync(Score data)
        {
            var ret = await DataStore.UpdateAsync_Score(data);
            return ret;
        }

        // Call to database to ensure most recent
        public async Task<Score> GetAsync(string id)
        {
            var ret = await DataStore.GetAsync_Score(id);
            return ret;
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
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Score(true);
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

        // Refreshes data
        public void ForceDataRefresh()
        {
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }
        #endregion DataOperations
    }
}