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

        public RoundEnum RoundStateEnum = RoundEnum.Unknown;

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
            

            //var ScaleLevelMax = 0;
            //var ScaleLevelMin = 0;

            ////Scale monsters based on the rounds so far
            //if (BattleScore.RoundCount < 10)
            //{
            //    ScaleLevelMax = 3;
            //    ScaleLevelMin = 1;
            //}
            //if (BattleScore.RoundCount > 10 && BattleScore.RoundCount <= 40)
            //{
            //    ScaleLevelMax = 4;
            //    ScaleLevelMin = 2;
            //}

            //if (BattleScore.RoundCount > 40 && BattleScore.RoundCount <= 70)
            //{
            //    ScaleLevelMax = 8;
            //    ScaleLevelMin = 4;
            //}
            //if (BattleScore.RoundCount > 70 && BattleScore.RoundCount <= 100)
            //{
            //    ScaleLevelMax = 12;
            //    ScaleLevelMin = 8;
            //}
            //if (BattleScore.RoundCount > 100)
            //{
            //    ScaleLevelMax = 20;
            //    ScaleLevelMin = 12;
            //}

            // Make suure monster list exists and is loaded...
            var myMonsterViewModel = MonstersViewModel.Instance;
            //myMonsterViewModel.ForceDataRefresh();

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
                        // Scale monster to be harder later... 
                        //var rndScale = HelperEngine.RollDice(ScaleLevelMin, ScaleLevelMax);
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

            var monstersOutput = "Chosen monsters: \n";

            monstersOutput += "Count: ";
            monstersOutput += MonsterList.Count() + "\n";

            foreach (var mon in MonsterList)
            {
                monstersOutput += mon.FormatOutput() + "\n";
            }
            Debug.WriteLine(monstersOutput);
        }

        // At the end of the round
        // Clear the Item List
        // Clear the Monster List
        public void EndRound()
        {
            // Have each character pickup items...
            foreach (var character in CharacterList)
            {
                PickupItemsFromPool(character);
            }

            ClearLists();
        }

        // Get Round Turn Order

        // Rember Who's Turn

        // RoundNextTurn
        public RoundEnum RoundNextTurn()
        {
            Debug.WriteLine("From Round Engine: " + RoundStateEnum);
            // No charaacters, game is over...
            if(CharacterList.Count < 1)
            {
                RoundStateEnum = RoundEnum.GameOver;
                return RoundStateEnum;
            }

            // Check if round is over
            if(MonsterList.Count < 1)
            {
                // If over, New Round
                RoundStateEnum = RoundEnum.NewRound;
                return RoundStateEnum;
            }

            // Decide Who gets next turn
            // Remember who just went...
            PlayerCurrent = GetNextPlayerInList();

            // Decide Who to Attack
            //Do the Turn
            if (PlayerCurrent.PlayerType == PlayerTypeEnum.Character)
            {
                // Get the player
                var myPlayer = CharacterList.Where(a => a.Guid == PlayerCurrent.Guid).FirstOrDefault();

                // Do the turn...
                TakeTurn(myPlayer);
            }
            // Add Monster turn here...
            else if (PlayerCurrent.PlayerType == PlayerTypeEnum.Monster)
            {
                // Get the player
                var myPlayer = MonsterList.Where(a => a.Guid == PlayerCurrent.Guid).FirstOrDefault();

                // Do the turn...
                TakeTurn(myPlayer);
            }

            RoundStateEnum = RoundEnum.NextTurn;
            return RoundStateEnum;
            // Game Over
            //return RoundEnum.GameOver;
        }

        // Add players to list and order them 
        private void MakePlayerList()
        {
            PlayerList = new List<PlayerInfo>();
            PlayerInfo tempPlayer;

            var ListOrder = 0;

            foreach (var data in CharacterList)
            {
                if (data.Alive)
                {
                    tempPlayer = new PlayerInfo(data);

                    // Remember the order
                    tempPlayer.ListOrder = ListOrder;

                    PlayerList.Add(tempPlayer);

                    ListOrder++;
                }
            }

            foreach (var data in MonsterList)
            {
                if (data.Alive)
                {

                    tempPlayer = new PlayerInfo(data);

                    // Remember the order
                    tempPlayer.ListOrder = ListOrder;

                    PlayerList.Add(tempPlayer);

                    ListOrder++;
                }
            }

            //Order the list 
            PlayerList = PlayerList.OrderByDescending(a => a.Speed)
                .ThenByDescending(a => a.Level)
                .ThenByDescending(a => a.ExperiencePoints)
                .ThenByDescending(a => a.PlayerType)
                .ThenBy(a => a.Name)
                .ThenBy(a => a.ListOrder)
                .ToList();

            var playerListToString = "Player list this round: ";
            foreach (PlayerInfo p in PlayerList)
            {
                playerListToString += p.Name + " ";
            }
            Debug.WriteLine(playerListToString);
        }

        public PlayerInfo GetNextPlayerInList()
        {
            if (PlayerList.Count == 0)
            {
                return null;
            }

            // Since the list is already ordered by attributes, grab first 
            PlayerCurrent = PlayerList.FirstOrDefault();

            // Does not seem to recognize dead players? The dead thing never gets attacked agai
            //while (!PlayerCurrent.Alive)
            //{
            //    PlayerList.Remove(PlayerCurrent);
            //    Debug.WriteLine("Removing dead " + PlayerCurrent.Name + "\n");
            //}

            // Lazy dequeue and enqueue the selected player
            PlayerList.Remove(PlayerCurrent);
            PlayerList.Add(PlayerCurrent);

            // Debug messages to make sure queue is working 
            var GetNextDebug = "Dequeued/enqueued " + PlayerCurrent.Name + "... \n";
            GetNextDebug += "Player list now looks like this: ";
            foreach (PlayerInfo p in PlayerList)
            {
                GetNextDebug += p.Name + "\n";
            }
            //Debug.WriteLine(GetNextDebug);
            return PlayerCurrent;
        }

        public void PickupItemsFromPool(Character character)
        {
            // Have the character, walk the items in the pool, and decide if any are better than current one.

            // No items in the pool...
            if (ItemPool.Count < 1)
            {
                return;
            }

            GetItemFromPoolIfBetter(character, ItemLocationEnum.Head);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Necklass);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.PrimaryHand);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.OffHand);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.RightFinger);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.LeftFinger);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Feet);
        }

        public void GetItemFromPoolIfBetter(Character character, ItemLocationEnum setLocation)
        {
            var myList = ItemPool.Where(a => a.Location == setLocation)
                .OrderByDescending(a => a.Value)
                .ToList();

            // If no items in the list, return...
            if (!myList.Any())
            {
                return;
            }

            var currentItem = character.GetItemByLocation(setLocation);
            if (currentItem == null)
            {
                // If no item in the slot then put on the first in the list
                character.AddItem(setLocation, myList.FirstOrDefault().Id);
                return;
            }

            foreach (var item in myList)
            {
                if (item.Value > currentItem.Value)
                {
                    // Put on the new item, which drops the one back to the pool
                    var droppedItem = character.AddItem(setLocation, item.Id);

                    // Remove the item just put on from the pool
                    ItemPool.Remove(item);

                    if (droppedItem != null)
                    {
                        // Add the dropped item to the pool
                        ItemPool.Add(droppedItem);
                    }
                }
            }
        }

    }
}
