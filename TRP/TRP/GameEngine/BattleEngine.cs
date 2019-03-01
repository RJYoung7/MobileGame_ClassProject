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
            ScoresViewModel.Instance.AddAsync(BattleScore).GetAwaiter().GetResult();
        }

        // Initializes the Battle to begin
        public bool StartBattle(bool isAutoBattle)
        {
            return true;
        }

        // Add Characters
        // Scale them to meet Character Strength...
        public bool AddCharactersToBattle()
        {
            // If the party does not have 6 characters, add them. 
            if (CharacterList.Count == 6)
            {
                return true;
            }

            var need = 6 - (CharacterList.Count);
            if (need >= 0)
            {
                var rand = CharactersViewModel.Instance.Dataset.Take(need);
                CharacterList.AddRange(rand);
            }

            return true;
        }

        // Get a random character within range of min and max parameters
        public Character GetRandomCharacter()
        {
            var myData = CharactersViewModel.Instance.ChooseRandomCharacter();
            return myData;
        }

        // Autobattle
        public bool AutoBattle()
        {
            return true;
        }

    }
}
