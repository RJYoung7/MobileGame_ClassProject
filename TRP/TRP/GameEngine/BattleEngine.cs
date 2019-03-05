using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.GameEngine
{
    // Battle is the top structure
    public class BattleEngine : RoundEngine
    {
        // The status of the actual battle, running or not (over)
        private bool isBattleRunning = false;

        // Constructor calls Init
        public BattleEngine()
        {
            BattleEngineInit();
        }

        // Sets the new state for the variables for Battle
        private void BattleEngineInit()
        {
            BattleScore = new Score();
            CharacterList = new List<Character>();
            ItemPool = new List<Item>();
        }

        // Determine if Auto Battle is On or Off
        public bool GetAutoBattleState()
        {
            return BattleScore.AutoBattle;
        }

        // Return if the Battle is Still running
        public bool BattleRunningState()
        {
            return isBattleRunning;
        }

        // Battle is over
        // Update Battle State, Log Score to Database
        public void EndBattle()
        {
            // Set Score
            BattleScore.ScoreTotal = BattleScore.ExperienceGainedTotal;

            // Set off state
            isBattleRunning = false;

            // Save the Score to the Datastore
            ScoresViewModel.Instance.Dataset.Add(BattleScore);
            //ScoresViewModel.Instance.AddAsync(BattleScore).GetAwaiter().GetResult();
        }

        // Initializes the Battle to begin
        public bool StartBattle(bool isAutoBattle)
        {
            // New Battle
            // Load Characters
            BattleScore.AutoBattle = isAutoBattle;
            BattleScore.BattleNumber = getLatestBattleNumber() + 1;
            BattleScore.Name = "Battle " + BattleScore.BattleNumber.ToString();
            isBattleRunning = true;

            // Characters not Initialized, so false start...
            if(CharacterList.Count < 1)
            {
                return false;
            }

            return true;
        }

        // Add Characters
        // Scale them to meet Character Strength...
        public bool AddCharactersToBattle()
        {
            // Check if the Character list is empty
            if (CharactersViewModel.Instance.Dataset.Count < 1)
            {
                return false;
            }

            // If the party does not have 6 characters, add them. 
            if (CharacterList.Count >= 6)
            {
                return true;
            }

            // TODO, determine the character strength
            // add Characters up to that strength...
            var ScaleLevelMax = 3;
            var ScaleLevelMin = 1;

            //var need = 6 - (CharacterList.Count);
            //if (need >= 0)
            //{
            //    var rand = CharactersViewModel.Instance.Dataset.Take(need);
            //    CharacterList.AddRange(rand);
            //}
            // Get 6 Characters
            do
            {
                var Data = GetRandomCharacter(ScaleLevelMin, ScaleLevelMax);
                CharacterList.Add(Data);
            } while (CharacterList.Count < GameGlobals.MaxNumberPartyPlayers);

            return true;
        }

        // Get a random character within range of min and max parameters
        public Character GetRandomCharacter(int ScaleLevelMin, int ScaleLevelMax)
        {
            var myCharacterViewModel = CharactersViewModel.Instance;

            var rnd = HelperEngine.RollDice(1, myCharacterViewModel.Dataset.Count);

            var myData = new Character(myCharacterViewModel.Dataset[rnd - 1]);

            // Help identify which Character it is...
            myData.Name += " " + (1 + CharacterList.Count).ToString();

            var rndScale = HelperEngine.RollDice(ScaleLevelMin, ScaleLevelMax);
            myData.ScaleLevel(rndScale);

            // Add Items...
            myData.Head = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Head, AttributeEnum.Unknown);
            myData.Necklass = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Necklass, AttributeEnum.Unknown);
            myData.PrimaryHand = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.PrimaryHand, AttributeEnum.Unknown);
            myData.OffHand = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.OffHand, AttributeEnum.Unknown);
            myData.RightFinger = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.RightFinger, AttributeEnum.Unknown);
            myData.LeftFinger = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.LeftFinger, AttributeEnum.Unknown);
            myData.Feet = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Feet, AttributeEnum.Unknown);

            return myData;
        }

        // Finds the latest battle number
        public int getLatestBattleNumber()
        {
            var BattleNumber = ScoresViewModel.Instance.Dataset.Max(s => s.BattleNumber);
            return BattleNumber;
        }

        // Autobattle
        public bool AutoBattle()
        {
            return true;
        }

    }
}
