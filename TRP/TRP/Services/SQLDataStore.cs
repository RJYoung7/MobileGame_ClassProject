using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            CreateTables();
            InitializeDatabaseNewTables();
        }

        // Create the Database Tables
        private void CreateTables()
        {
            App.Database.CreateTableAsync<Item>().Wait();
            App.Database.CreateTableAsync<BaseCharacter>().Wait();
            App.Database.CreateTableAsync<BaseMonster>().Wait();
            App.Database.CreateTableAsync<Score>().Wait();
        }

        // Delete the Datbase Tables by dropping them
        private void DeleteTables()
        {
            App.Database.DropTableAsync<Item>().Wait();
            App.Database.DropTableAsync<BaseCharacter>().Wait();
            App.Database.DropTableAsync<BaseMonster>().Wait();
            App.Database.DropTableAsync<Score>().Wait();
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            ScoresViewModel.Instance.SetNeedsRefresh(true);
        }

        public void InitializeDatabaseNewTables()
        {
            //Delete the tables
            DeleteTables();

            // Make them again
            CreateTables();

            // Populate them
            InitializeSeedData();

            // Tell view Models they need to refresh
            NotifyViewModelsOfDataChange();

        }

        private async void InitializeSeedData()
        {
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQLFirst item", Description = "This is an item description." });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQLSecond item", Description = "This is an item description." });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQLThird item", Description = "This is an item description." });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQLFourth item", Description = "This is an item description." });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQLFifth item", Description = "This is an item description." });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQLSixth item", Description = "This is an item description." });

            await AddAsync_Character(new Character("SQLPoppy", new AttributeBase(10, 4, 4, 2), PenguinTypeEnum.Emperor));
            await AddAsync_Character(new Character("SQLPerry", new AttributeBase(10, 4, 2, 4), PenguinTypeEnum.Little));
            await AddAsync_Character(new Character("SQLPaco", new AttributeBase(10, 3, 3, 4), PenguinTypeEnum.Gentoo));
            await AddAsync_Character(new Character("SQLPatrick", new AttributeBase(10, 3, 4, 3), PenguinTypeEnum.Macaroni));

            await AddAsync_Monster(new Monster("SQLLeonard", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.LeopardSeal));
            await AddAsync_Monster(new Monster("SQLArnie", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.Fox));

            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQLFirst Score", ScoreTotal = 111 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQLSecond Score", ScoreTotal = 222 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQLThird Score", ScoreTotal = 333 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQLFourth Score", ScoreTotal = 444 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQLFifth Score", ScoreTotal = 555 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQLSixth Score", ScoreTotal = 666 });

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
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            var result = await App.Database.GetAsync<Item>(id);
            return result;
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Item>().ToListAsync();
            return result;
        }
        #endregion Item

        #region Character
        // Character
        public async Task<bool> InsertUpdateAsync_Character(Character data)
        {
            // Implement

            return false;
        }

        // Conver to BaseCharacter and then add it
        public async Task<bool> AddAsync_Character(Character data)
        {
            data.Update(data);
            var databaseChar = new BaseCharacter(data);
            var result = await App.Database.InsertAsync(databaseChar);
            if (result == 1)
                return true;
            return false;
        }

        // Convert to BaseCharacter and then update it
        public async Task<bool> UpdateAsync_Character(Character data)
        {
            data.Update(data);
            var castData = new BaseCharacter(data);
            var result = await App.Database.UpdateAsync(castData);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Pass in the character and convert to Character to then delete it
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the Character Base, and Load it back as Character
        public async Task<Character> GetAsync_Character(string id)
        {
            var result = await App.Database.GetAsync<BaseCharacter>(id);
            var data = new Character(result);
            return data;
        }

        // Load each character as the base character, 
        // Then then convert the list to characters to push up to the view model
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            var result = await App.Database.Table<BaseCharacter>().ToListAsync();
            var ret = result.Select(c => new Character(c)).ToList();
            return ret;
        }

        #endregion Character

        #region Monster
        //Monster
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            data.Update(data);
            var castData = new BaseMonster(data);
            var result = await App.Database.InsertAsync(castData);
            if (result == 1)
                return true;
            return false;
        }

        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            data.Update(data);
            var castData = new BaseMonster(data);
            var result = await App.Database.UpdateAsync(castData);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Monster> GetAsync_Monster(string id)
        {
            var result = await App.Database.GetAsync<BaseMonster>(id);
            var ret = new Monster(result);
            return ret;
        }

        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            var result = await App.Database.Table<BaseMonster>().ToListAsync();
            var ret = result.Select(m => new Monster(m)).ToList();
            return ret;
        }



        #endregion Monster

        #region Score
        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            var result = await App.Database.GetAsync<Score>(id);
            return result;
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Score>().ToListAsync();
            return result;

        }

        #endregion Score
    }
}