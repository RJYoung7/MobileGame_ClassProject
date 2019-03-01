using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using TRP.Models;
using TRP.ViewModels;
using System.Diagnostics;

namespace TRP.GameEngine
{
    public class RoundEngine : TurnEngine
    {
        // Hold the list of players (monster, and character by guid), and order by speed
        public List<PlayerInfo> PlayerList;

        // Player currently engaged
        public PlayerInfo PlayerCurrent;

        public RoundEngine()
        {
            ClearLists();
        }

        private void ClearLists()
        {
            ItemPool = new List<Item>();
            MonsterList = new List<Monster>();
        }

        // Start the round, need to get the ItemPool, and Characters
        public void StartRound()
        {
            // Start on round 0, then the turns will increment the round
            BattleScore.RoundCount = 0;

            // Start 1st round
            NewRound();

            // Print Round.
            Debug.WriteLine("Start Round: " + BattleScore.RoundCount);
        }

        // Call to make a new set of monsters...
        public void NewRound()
        {
            // End previous round
            EndRound();

            // Add monsters
            AddMonstersToRound();

            // Add characters
            MakePlayerList();

            // Update round count
            BattleScore.RoundCount++;
        }

        /// <summary>
        /// Will return the average of the characters level
        /// Will be used to scaled Monsters to appropriate level
        /// </summary>
        /// <returns>An integer representing the averge level</returns>
        public int GetAverageCharacterLevel()
        {
            var data = CharacterList.Average(m => m.Level);
            return (int)Math.Floor(data);
        }

        /// <summary>
        /// Will return the Minimum of the characters levels
        /// Will be used to scale Monsters to appropriate level
        /// </summary>
        /// <returns>An integer representing the minimum level</returns>
        public int GetMinCharacterLevel()
        {
            var data = CharacterList.Min(m => m.Level);
            return data;
        }

        /// <summary>
        /// Will return the Maximum of the characters levels
        /// Will be used to scale Monsters to the appropriate level
        /// </summary>
        /// <returns>An integer representing the maximum level</returns>
        public int GetMaxCharacterLevel()
        {
            var data = CharacterList.Max(m => m.Level);
            return data;
        }

        // Add Monsters
        // Scale them to meet Character Strength...
        private void AddMonstersToRound()
        {
            // Check to see if the monster list is full, if so, no need to add more...
            if(MonsterList.Count() >= 6)
            {
                return;
            }

            // Make suure monster list exists and is loaded...
            var myMonsterViewModel = MonstersViewModel.Instance;
            myMonsterViewModel.ForceDataRefresh();

            if(myMonsterViewModel.Dataset.Count() > 0)
            {
                // Scale monsters to be within the range of the characters
                var ScaleLevelMax = 1;
                var ScaleLevelMin = 1;
                var ScaleLevelAverage = 1;

                if (CharacterList.Any())
                {
                    ScaleLevelMin = GetMinCharacterLevel();
                    ScaleLevelMax = GetMaxCharacterLevel();
                    ScaleLevelAverage = GetAverageCharacterLevel();
                }

                // Get 6 monsters
                do
                {
                    var rnd = HelperEngine.RollDice(1, myMonsterViewModel.Dataset.Count);
                    {
                        var monster = new Monster(myMonsterViewModel.Dataset[rnd - 1]);

                        // Scale the monster to be between the average level of the characters+1
                        var rndScale = HelperEngine.RollDice(1, ScaleLevelAverage + 1);
                        monster.ScaleLevel(rndScale);
                        MonsterList.Add(monster);
                    }

                } while (MonsterList.Count() < 6);
            }
            else
            {
                // No mosnters in DB, so add 6 new ones...
                for (var i = 0; i < 6; i++)
                {
                    var monster = new Monster();

                    // Help identify which monster it is....
                    monster.Name += " " + MonsterList.Count() + 1;
                    MonsterList.Add(monster);
                    
                }
            }
        }

        // At the end of the round
        // Clear the Item List
        // Clear the Monster List
        public void EndRound()
        {
            ClearLists();
        }

        // Get Round Turn Order

        // Rember Who's Turn

        // RoundNextTurn
        public RoundEnum RoundNextTurn()
        {
            // Game Over
            return RoundEnum.GameOver;
        }

        public PlayerInfo GetNextPlayerTurn()
        {
            // Recalculate Order

            var PlayerCurrent = GetNextPlayerInList();

            return PlayerCurrent;
        }

        public void OrderPlayerListByTurnOrder()
        {
            var myReturn = new List<PlayerInfo>();

            MakePlayerList();
        }

        private void MakePlayerList()
        {
            PlayerList = new List<PlayerInfo>();
        }

        public PlayerInfo GetNextPlayerInList()
        {
            return null;
        }

        public void PickupItemsFromPool(Character character)
        {
            return;
        }

        public void GetItemFromPoolIfBetter(Character character, ItemLocationEnum setLocation)
        {
            return;
        }
    }
}
