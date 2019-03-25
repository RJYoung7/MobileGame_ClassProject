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

        // Current character as player
        public Character CurrentCharacter;
        
        // Enum for round status
        public RoundEnum RoundStateEnum = RoundEnum.Unknown;

        // Clears round 
        public RoundEngine()
        {
            ClearLists();
        }

        // Creates new lists
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
            //myMonsterViewModel.ForceDataRefresh();

            // Scale monsters based on current character levels
            if(myMonsterViewModel.Dataset.Count() > 0)
            {
                // Scale monsters to be within the range of the characters
                var ScaleLevelMax = 1;
                var ScaleLevelMin = 1;
                var ScaleLevelAverage = 1;

                // If there are any characters get min, max, and average levels of all of them
                if (CharacterList.Any())
                {
                    ScaleLevelMin = GetMinCharacterLevel();
                    ScaleLevelMax = GetMaxCharacterLevel();
                    ScaleLevelAverage = GetAverageCharacterLevel();
                }

                // Get 6 monsters
                do
                {
                    // Roll dice to get random monster from dataset
                    var rnd = HelperEngine.RollDice(1, myMonsterViewModel.Dataset.Count);
                    {
                        // Ensure rnd number is less than dataset size
                        if (rnd > myMonsterViewModel.Dataset.Count())
                        {
                            rnd = myMonsterViewModel.Dataset.Count();
                        }

                        // Create a new monster from the monster in the dataset
                        var monster = new Monster(myMonsterViewModel.Dataset[rnd - 1]);

                        // Scale the monster to be between the average level of the characters+1
                        var rndScale = HelperEngine.RollDice(1, ScaleLevelAverage + 1);

                        // Scale monster to be harder later... 
                        monster.ScaleLevel(rndScale);

                        // Add monster to the list
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

                    // Add monster to the list
                    MonsterList.Add(monster);
                }
            }

            // Debug output text for chosen monsters
            var monstersOutput = "Chosen monsters: \n";
            monstersOutput += "Count: " + MonsterList.Count() + "\n"; ;

            // Add name of each monster to debug output statement
            foreach (var mon in MonsterList)
            {
                monstersOutput += mon.FormatOutput() + "\n";
            }

            // Write the debug output statement
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

            // Clear the lists
            ClearLists();
        }

        // Get Round Turn Order
        // Rember Who's Turn
        // Starts next turn during round
        public RoundEnum RoundNextTurn()
        {
            // Debug statements
            Debug.WriteLine("Starting RoundEngine...");
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
            PlayerCurrent = GetNextPlayerInList();

            // Debug output of the next players name
            Debug.WriteLine(PlayerCurrent.Name);

            // Decide Who to Attack
            //Do the turn as a character
            if (PlayerCurrent.PlayerType == PlayerTypeEnum.Character)
            {
                // Get the current character for consumables
                CurrentCharacter = PlayerCharacter(PlayerCurrent);
                Debug.WriteLine("It's a Character!");

                // Get the player
                var myPlayer = CharacterList.Where(a => a.Guid == PlayerCurrent.Guid).FirstOrDefault();

                // Remove player from player list if null or dead
                if (myPlayer == null || !myPlayer.Alive)
                {
                    PlayerList.Remove(PlayerCurrent);

                    // Restart the turn
                    RoundNextTurn();
                }

                // Do the turn...
                TakeTurn(myPlayer);
            }
            // Monsters turn
            else if (PlayerCurrent.PlayerType == PlayerTypeEnum.Monster)
            {
                // Get the monster
                var myPlayer = MonsterList.Where(a => a.Guid == PlayerCurrent.Guid).FirstOrDefault();

                // If monster is dead or null remove it from the player list and restart round
                if (myPlayer == null || !myPlayer.Alive)
                {
                    // Remove
                    PlayerList.Remove(PlayerCurrent);

                    // Restart
                    RoundNextTurn();
                }

                // Do the turn...
                TakeTurn(myPlayer);
            }

            // Update the roundstatenum to next turn
            RoundStateEnum = RoundEnum.NextTurn;

            // Return the enum
            return RoundStateEnum;
        }

        // Add players to list and order them 
        private void MakePlayerList()
        {
            // Instantiate the playerList
            PlayerList = new List<PlayerInfo>();

            // Variable to hold a temporary player
            PlayerInfo tempPlayer;

            // Variable to help with assigning list order
            var ListOrder = 0;

            // Go through the characters and add them to the player list
            foreach (var data in CharacterList)
            {
                // Check to make sure character is alive
                if (data.Alive)
                {
                    // Create a new player with the character data
                    tempPlayer = new PlayerInfo(data);

                    // Remember the order
                    tempPlayer.ListOrder = ListOrder;

                    // Add character to the playerlist
                    PlayerList.Add(tempPlayer);

                    // Increment the list order
                    ListOrder++;
                }
            }

            // Updates list order with monsters
            foreach (var data in MonsterList)
            {
                // Check if monster is alive
                if (data.Alive)
                {
                    // Create a new player with the monster data
                    tempPlayer = new PlayerInfo(data);

                    // Remember the order
                    tempPlayer.ListOrder = ListOrder;

                    // Add monster to the playerlist
                    PlayerList.Add(tempPlayer);

                    // Increment the list order
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

            // Format list for output
            var playerListToString = "Player list this round: ";

            // Get the player list and add each player to a debug string
            foreach (PlayerInfo p in PlayerList)
            {
                playerListToString += p.Name + " ";
            }

            // Output the player list to the debug window
            Debug.WriteLine(playerListToString);

            // Check to see if timewarp chance is enabled.
            if (GameGlobals.EnableReverseOrder)
            {
                // Determine if neworder is needed
                var newOrder = orderChange();

                // if new order is needed...
                if (newOrder == true)
                {
                    // Let player know, and reverse list.
                    BattleMessage.TimeWarpMessage += "Time gets wonky, slowest player goes first.\n";
                    PlayerList.Reverse();

                    // Format new list for output
                    playerListToString = "Player list this round: ";

                    foreach (PlayerInfo p in PlayerList)
                    {
                        playerListToString += p.Name + " ";
                    }
                    Debug.WriteLine(playerListToString);
                } else
                {
                    BattleMessage.TimeWarpMessage += "\n Time feels normal.\n"; 
                }
            }

        }

        // Determine whether time warp occurs based on user provided input.
        public bool orderChange()
        {
            Debug.WriteLine("Checking if time warped");

            // Get threshold for timewarp
            var revChance = 20 - ((GameGlobals.ReverseChance/100)*20);
            Debug.WriteLine("revChance: " + revChance);

            // Roll to determine timewarp
            var roll = HelperEngine.RollDice(1, 20);
            Debug.WriteLine("Roll: " + roll);

            // if roll succeeds, Return true for timewarp.
            if (roll >= revChance)
            {
                Debug.WriteLine("TIME WARP!");
                return true;
            }
            else
            {
                // No time warp
                Debug.WriteLine("Normal Time");
                return false;
            }
            
        }

        // Updates player to lists
        public PlayerInfo GetNextPlayerInList()
        {
            // If list is empty return null
            if (PlayerList.Count == 0)
            {
                return null;
            }

            // Since the list is already ordered by attributes, grab first 
            PlayerCurrent = PlayerList.FirstOrDefault();

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

            return PlayerCurrent;
        }

        // Assigns items to characters based on need.
        public void PickupItemsFromPool(Character character)
        {
            // Have the character, walk the items in the pool, and decide if any are better than current one.
            // No items in the pool...
            if (ItemPool.Count < 1)
            {
                return;
            }

            // Get the better items for each location
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Head);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Necklass);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.PrimaryHand);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.OffHand);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.RightFinger);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.LeftFinger);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Feet);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Bag);
        }

        // Replaces an item assigned to character if there is a better item avaliable.
        public void GetItemFromPoolIfBetter(Character character, ItemLocationEnum setLocation)
        {
            // Get the highest value item by location
            var myList = ItemPool.Where(a => a.Location == setLocation)
                .OrderByDescending(a => a.Value)
                .ToList();

            // If no items in the list, return...
            if (!myList.Any())
            {
                return;
            }

            // Get the item that is on the character in that location
            var currentItem = character.GetItemByLocation(setLocation);

            // If no item in that location, add the item
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

                    // Add the item to the pool if there is one
                    if (droppedItem != null)
                    {
                        // Add the dropped item to the pool
                        ItemPool.Add(droppedItem);
                    }
                }
            }
        }

        // Returns a character for item use.
        public Character PlayerCharacter(PlayerInfo player)
        {
            // Walk the character list to match the player guid with the characterlist guid
            foreach(var c in CharacterList)
            {
                if(player.Guid == c.Guid)
                {
                    return c;
                }
            }

            return null;
        }
    }
}
