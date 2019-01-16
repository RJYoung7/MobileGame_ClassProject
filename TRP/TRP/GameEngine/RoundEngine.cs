using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using TRP.Models;
using TRP.ViewModels;
using System.Diagnostics;

namespace TRP.GameEngine
{
    class RoundEngine : TurnEngine
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
        }

        // Call to make a new set of monsters...
        public void NewRound()
        {
        }

        // Add Monsters
        // Scale them to meet Character Strength...
        private void AddMonstersToRound()
        {
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
