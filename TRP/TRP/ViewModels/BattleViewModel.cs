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
        public ObservableCollection<Monster> SelectedMonsters { get; set; } //selected party of characters

        public Command LoadDataCommand { get; set; } // load data command 

        private bool _needsRefresh; // boolean for whether data is stale or not

        // Constructor: loads data and listens for broadcast from views
        public BattleViewModel()
        {
            Title = "Battle Begin";
            BattleEngine = new BattleEngine();

            SelectedCharacters = new ObservableCollection<Character>();
            AvailableCharacters = new ObservableCollection<Character>();
            SelectedMonsters = new ObservableCollection<Monster>();

            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            ExecuteLoadDataCommand().GetAwaiter().GetResult();

            // For adding Characters to party
            MessagingCenter.Subscribe<CharactersSelectPage, IList<Character>>(this, "AddData", (obj, data) =>
            {
                SelectedCharacters.Clear();
                BattleEngine.CharacterList = data.ToList<Character>();
                foreach (var c in data) {
                    SelectedCharacters.Add(c);
                }
                //SelectedCharacters = data.ToArray();
                
            });

            //Messages for adding a character to party, removing a character from party
            MessagingCenter.Subscribe<CharactersSelectPage, Character>(this, "AddSelectedCharacter", async (obj, data) =>
            {
                SelectedListAdd(data);
            });

            MessagingCenter.Subscribe<CharactersSelectPage, Character>(this, "RemoveSelectedCharacter", async (obj, data) =>
            {
                SelectedListRemove(data);
            });

            //Messages to start and end battle

            //Messages to start and end rounds
            MessagingCenter.Subscribe<BattleEngine, RoundEnum>(this, "NewRound", async (obj, data) =>
            {
                BattleEngine.NewRound();
            });

            MessagingCenter.Subscribe<BattleEngine, RoundEnum>(this, "EndBattle", async (obj, data) =>
            {
                BattleEngine.EndBattle();
            });

            //Messages for turns in round 
            MessagingCenter.Subscribe<BattlePage, RoundEnum>(this, "RoundNextTurn", async (obj, data) =>
            {
                BattleEngine.RoundNextTurn();
            });
        }

        // Calls engine to start battle
        public void StartBattle()
        {
            Instance.BattleEngine.StartBattle(false);
        }

        // Calls engine to end battle
        public void EndBattle()
        {
            Instance.BattleEngine.EndBattle();
        }

        public RoundEnum currentRoundEnum()
        {
            return Instance.BattleEngine.RoundStateEnum;
        }

        // Calls engine to start round 
        public void StartRound()
        {
            Instance.BattleEngine.StartRound();
        }

        // Loads copies of selected characters into battle engine
        public void LoadCharacters()
        {
            foreach (var data in SelectedCharacters)
            {
                BattleViewModel.Instance.BattleEngine.CharacterList.Add(new Character(data));
            }

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

        // Call to database to grab character
        public Character Get(string id)
        {
            var myData = SelectedCharacters.FirstOrDefault(arg => arg.Id == id);
            if (myData == null)
            {
                return null;
            }

            return myData;

        }
        #endregion DataOperations

        // Clear current lists so they can be reused 
        public void ClearCharacterLists()
        {
            AvailableCharacters.Clear();
            SelectedCharacters.Clear();
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
                // SelectedCharacters, no need to change them.

                // Reload the Character List from the Character View Moel
                AvailableCharacters.Clear();
                var availableCharacters = CharactersViewModel.Instance.Dataset;
                foreach (var data in availableCharacters)
                {
                    AvailableCharacters.Add(data);
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

    }
}