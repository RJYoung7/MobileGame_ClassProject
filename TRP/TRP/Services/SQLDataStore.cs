using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRP.Models;
using TRP.ViewModels;

namespace TRP.Services
{
    public sealed class SQLDataStore : IDataStore
    {

        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static SQLDataStore _instance;

        public static SQLDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLDataStore();
                }
                return _instance;
            }
        }

        private SQLDataStore()
        {
            // Implement
            // CreateTables();
        }

        // Create the Database Tables
        private void CreateTables()
        {
            // Implement
        }

        // Delete the Datbase Tables by dropping them
        private void DeleteTables()
        {
            // Implement
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            // Implement
        }

        public void InitializeDatabaseNewTables()
        {
            // Implement

            // Delete the tables

            // make them again

            // Populate them

            // Tell View Models they need to refresh

        }

        private async void InitilizeSeedData()
        {
            // Implement

        }

        #region Item
        // Item

        // Add InsertUpdateAsync_Item Method

        // Check to see if the item exists
        // Add your code here.

        // If it does not exist, then Insert it into the DB
        // Add your code here.
        // return true;

        // If it does exist, Update it into the DB
        // Add your code here
        // return true;

        // If you got to here then return false;

        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {
            // Implement

            return false;
        }

        public async Task<bool> AddAsync_Item(Item data)
        {
            // Implement

            return false;
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            // Implement

            return false;
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            // Implement

            return false;
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            // Implement
            return null;
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            // Implement
            return null;
        }
        #endregion Item

        #region Character
        // Character

        // Conver to BaseCharacter and then add it
        public async Task<bool> AddAsync_Character(Character data)
        {
            // Implement

            return false;
        }

        // Convert to BaseCharacter and then update it
        public async Task<bool> UpdateAsync_Character(Character data)
        {
            // Implement
            return false;
        }

        // Pass in the character and convert to Character to then delete it
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            // Implement
            return false;
        }

        // Get the Character Base, and Load it back as Character
        public async Task<Character> GetAsync_Character(string id)
        {
            // Implement
            return null;
        }

        // Load each character as the base character, 
        // Then then convert the list to characters to push up to the view model
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            // Implement
            return null;
        }

        #endregion Character

        #region Monster
        //Monster
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            // Implement
            return false;
        }

        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            // Implement
            return false;
        }

        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            // Implement
            return false;
        }

        public async Task<Monster> GetAsync_Monster(string id)
        {
            // Implement
            return null;
        }

        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            // Implement
            return null;
        }

        #endregion Monster

        #region Score
        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            // Implement
            return false;
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            // Implement
            return false;
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            // Implement
            return false;
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            // Implement
            return null;
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            // Implement
            return null;

        }

        #endregion Score
    }
}