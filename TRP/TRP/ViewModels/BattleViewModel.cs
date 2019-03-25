using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TRP.Models;
using TRP.Views;
using TRP.Controllers;
using System.Linq;
using TRP.GameEngine;
using TRP.Views.Battle;
using System.Collections.Generic;

namespace TRP.ViewModels
{
    public class BattleViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static BattleViewModel _instance;

        // Constructor: returns instance if instantiated, otherwise creates instance if it's null 
        public static BattleViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleViewModel();
                }
                return _instance;
            }
        }

        // Battle Engine
        public BattleEngine BattleEngine;

        public ObservableCollection<Character> SelectedCharacters { get; set; } //selected party of characters
        public ObservableCollection<Character> AvailableCharacters { get; set; } //available characters left
        public ObservableCollection<Monster> SelectedMonsters { get; set; } //selected party of monsters
        public ObservableCollection<Item> availItems { get; set; }



        public Command LoadDataCommand { get; set; } // load data command 

        private bool _needsRefresh; // boolean for whether data is stale or not

        // Constructor: loads data and listens for broadcast from views
        public BattleViewModel()
        {
            Title = "Battle Begin";

            // Initialize battle engine
            BattleEngine = new BattleEngine();

            // Create observable collections
            SelectedCharacters = new ObservableCollection<Character>();
            AvailableCharacters = new ObservableCollection<Character>();
            SelectedMonsters = new ObservableCollection<Monster>();
            availItems = new ObservableCollection<Item>();

            // Load data command
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // Load the data
            ExecuteLoadDataCommand().GetAwaiter().GetResult();

            // For adding Characters to party
            MessagingCenter.Subscribe<CharactersSelectPage, IList<Character>>(this, "AddData", (obj, data) =>
            {
                SelectedCharacters.Clear();
                BattleEngine.CharacterList = data.ToList<Character>();
                foreach (var c in data) {
                    SelectedCharacters.Add(c);
                }
            });

            //Messages for adding a character to party
            MessagingCenter.Subscribe<CharactersSelectPage, Character>(this, "AddSelectedCharacter", async (obj, data) =>
            {
                SelectedListAdd(data);
            });

            // Messages for removing a character from the party
            MessagingCenter.Subscribe<CharactersSelectPage, Character>(this, "RemoveSelectedCharacter", async (obj, data) =>
            {
                SelectedListRemove(data);
            });

            //Messages to start new round
            MessagingCenter.Subscribe<BattleEngine, RoundEnum>(this, "NewRound", async (obj, data) =>
            {
                BattleEngine.NewRound();
            });

            //Messages for round next turn
            MessagingCenter.Subscribe<BattlePage, RoundEnum>(this, "RoundNextTurn", async (obj, data) =>
            {
                ExecuteLoadDataCommand().GetAwaiter().GetResult();
                BattleEngine.RoundNextTurn();
            });

            // Messages to end battle
            MessagingCenter.Subscribe<BattlePage, RoundEnum>(this, "EndBattle", async (obj, data) =>
            {
                BattleEngine.EndBattle();
            });
        }

        // Calls engine to start round 
        public void StartRound()
        {
            Instance.BattleEngine.StartRound();
        }

        // Calls engine for next turn in a round
        public void RoundNextTurn()
        {
            BattleViewModel.Instance.BattleEngine.RoundNextTurn();
        }

        //Calls engine for a new round 
        public void NewRound()
        {
            BattleViewModel.Instance.BattleEngine.NewRound();
        }

        #region DataOperations
        // Call to database to remove a character from the party
        public bool SelectedListRemove(Character data)
        {
            SelectedCharacters.Remove(data);
            return true;
        }

        // Call to database to add a character to party
        public bool SelectedListAdd(Character data)
        {
            SelectedCharacters.Add(data);
            return true;
        }
        #endregion DataOperations

        // Clear current lists so they can be reused 
        public void ClearCharacterLists()
        {
            AvailableCharacters.Clear();
            SelectedCharacters.Clear();
            SelectedMonsters.Clear();
            availItems.Clear();
            ExecuteLoadDataCommand();
        }

        // Returns whether refhres is needed. If true, then set false.
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

        // Command that Loads the Data
        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Reload the Available character list from the Character View Model
                AvailableCharacters.Clear();
                var availableCharacters = CharactersViewModel.Instance.Dataset;
                foreach (var data in availableCharacters)
                {
                    AvailableCharacters.Add(data);
                }

                // Reload the Selected Monster List from the Battle engine monster list
                SelectedMonsters.Clear();
                var selectedMon = BattleEngine.MonsterList;
                foreach (var mon in selectedMon)
                {
                    SelectedMonsters.Add(mon);
                }

                // Reload the Selected Character List from the Battle engine character list
                SelectedCharacters.Clear();
                var selectedChar = BattleEngine.CharacterList;
                foreach (var ch in selectedChar)
                {
                    SelectedCharacters.Add(ch);
                }

                // Reload the availItems List from the Battle engine itemspool list
                availItems.Clear();
                var avaItems = BattleEngine.ItemPool;
                foreach (var it in avaItems)
                {
                    availItems.Add(it);
                }
            }
            // Catch any exceptions
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Set isBusy to false
            finally
            {
                IsBusy = false;
            }
        }

        // Force a data refresh
        public void ForceDataRefresh()
        {
            // Reset
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }
    }
}