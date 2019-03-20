using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using TRP.Models;
using TRP.ViewModels;
using Xamarin.Forms;

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
            CharacterList.Clear();
            BattleEngineClearData();
        }

        // Sets the new state for the variables for Battle
        public void BattleEngineClearData()
        {
            // Create a score object
            BattleScore = new Score();

            // Create a BattleMessages object
            BattleMessage = new BattleMessages();

            // Clear the lists
            ItemPool.Clear();
            MonsterList.Clear();
            CharacterList.Clear();

            // Reset current player
            PlayerCurrent = null;
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
            MessagingCenter.Send(this, "AddData", BattleScore);
        }

        // Initializes the Battle to begin
        public bool StartBattle(bool isAutoBattle)
        {
            // New BattleScore information
            BattleScore.AutoBattle = isAutoBattle;
            BattleScore.BattleNumber = getLatestBattleNumber() + 1;
            BattleScore.Name = "Battle " + BattleScore.BattleNumber.ToString();

            // Sets battle running to true
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

            // Check if the Character list has enough characters
            if (CharacterList.Count >= 6)
            {
                return true;
            }

            // If the party does not have 6 characters, add them. 
            // TODO, determine the character strength
            // add Characters up to that strength...
            var ScaleLevelMax = 3;
            var ScaleLevelMin = 1;

            // Get up to 6 Characters
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
            myData.Bag = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Bag, AttributeEnum.Unknown);

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

        // Returns string of battle summary
        public string GetResultsOutput()
        {

            string myResult = "" +
                    " Battle Ended" + BattleScore.ScoreTotal +
                    " Total Score :" + BattleScore.ExperienceGainedTotal +
                    " Total Experience :" + BattleScore.ExperienceGainedTotal +
                    " Rounds :" + BattleScore.RoundCount +
                    " Turns :" + BattleScore.TurnCount +
                    " Monster Kills :" + BattleScore.MonstersKilledList;

            Debug.WriteLine(myResult);

            return myResult;
        }

    }
}
