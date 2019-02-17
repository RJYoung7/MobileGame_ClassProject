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

        // Call to database operation for delete
        public async Task<bool> DeleteAsync(Score data)
        {
            // Implement 
            return false;
        }

        // Call to database operation for add
        public async Task<bool> AddAsync(Score data)
        {
            // Implement 
            return false;
        }

        // Call to database operation for update
        public async Task<bool> UpdateAsync(Score data)
        {
            // Implement 
            return false;
        }

        // Call to database to ensure most recent
        public async Task<Score> GetAsync(string id)
        {
            // Implement 
            return null;
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
            _needsRefresh = value;
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
    }
}