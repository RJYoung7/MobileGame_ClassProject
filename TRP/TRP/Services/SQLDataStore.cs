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

        // Constructor for the SQLDataStore
        private SQLDataStore()
        {
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

        // Delete the Database Tables by dropping them
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

        // Initialize the database tables.  Delete, Create, Add seed data, and notify viewmodel
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

        // Add items, characters, monsters, and scores to the SQL database
        private async void InitializeSeedData()
        {
            // Default SQL Items
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQL Slush Helmet", Description = "A helmet made from slush.",
                ImageURI= "https://www.iconsdb.com/icons/preview/caribbean-blue/helmet-xxl.png", Range=0, Value=1, Damage=0, Location=ItemLocationEnum.Head,
                Attribute = AttributeEnum.Defense });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQL Ice Boots", Description = "Boots with harden iced.",
                ImageURI= "https://vikings.help/users/vikings/imgExtCatalog/big/m321.png", Range=0, Value=3, Damage=0, Location=ItemLocationEnum.Feet,
                Attribute=AttributeEnum.Defense });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "SQL Flame Bow", Description = "Crafted from artic flames.",
                ImageURI= "https://vignette.wikia.nocookie.net/callofduty/images/5/54/Kreeaho%27ahm_nal_Ahmhogaroc_third_person_BO3_Transparent.png",
                Range=4, Value=3, Damage=3, Location=ItemLocationEnum.PrimaryHand, Attribute=AttributeEnum.Attack });

            // Default SQL Characters
            await AddAsync_Character(new Character("SQLPoppy", new AttributeBase(10, 4, 4, 2), PenguinTypeEnum.Emperor));
            await AddAsync_Character(new Character("SQLPerry", new AttributeBase(10, 4, 2, 4), PenguinTypeEnum.Little));
            await AddAsync_Character(new Character("SQLPaco", new AttributeBase(10, 3, 3, 4), PenguinTypeEnum.Gentoo));
            await AddAsync_Character(new Character("SQLPatrick", new AttributeBase(10, 3, 4, 3), PenguinTypeEnum.Macaroni));

            // Default SQL Monsters
            await AddAsync_Monster(new Monster("SQLLeonard", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.LeopardSeal));
            await AddAsync_Monster(new Monster("SQLArnie", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.Fox));

            // Default SQL Scores
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

        // Add item to SQL database
        public async Task<bool> AddAsync_Item(Item data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Update the item in the database
        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Delete the item from the database
        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the item from the database
        public async Task<Item> GetAsync_Item(string id)
        {
            var result = await App.Database.GetAsync<Item>(id);
            return result;
        }

        // Get a list of items from the database
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

        // Convert to BaseCharacter and then add it
        public async Task<bool> AddAsync_Character(Character data)
        {
            // Update character data
            data.Update(data);

            // Convert to BaseCharacter type to add to DB
            var databaseChar = new BaseCharacter(data);

            // Insert BaseCharacter
            var result = await App.Database.InsertAsync(databaseChar);

            // If inserstion is successful, return true
            if (result == 1)
                return true;
            return false;
        }

        // Convert to BaseCharacter and then update it
        public async Task<bool> UpdateAsync_Character(Character data)
        {

            // Update character data
            data.Update(data);

            // Convert Character data to BaseCharacter data
            var castData = new BaseCharacter(data);

            // Update BaseCharacter data in DB
            var result = await App.Database.UpdateAsync(castData);

            // If update was successful, returen true
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Pass in the character and convert to Character to then delete it
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            // Convert Monster to BaseMonster
            var castData = new BaseCharacter(data);

            // Delete BaseMonster from DB
            var result = await App.Database.DeleteAsync(data);

            // If delete was successful, return true
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the Character Base, and Load it back as Character
        public async Task<Character> GetAsync_Character(string id)
        {
            // Get BaseCharacter from BaseCharacter table
            var result = await App.Database.GetAsync<BaseCharacter>(id);

            // Convert BaseCharacter to Character
            var data = new Character(result);

            return data;
        }

        // Load each character as the base character, 
        // Then then convert the list to characters to push up to the view model
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            // Get list of BaseCharacters from BaseCharacter table
            var result = await App.Database.Table<BaseCharacter>().ToListAsync();

            // Convert BaseCharacter list to Characters
            var ret = result.Select(c => new Character(c)).ToList();

            return ret;
        }

        #endregion Character

        #region Monster
        //Monster

        // Convert monster to BaseMonster and Add it to the SQL database
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            // Update Monster Data
            data.Update(data);

            // Convert Monster to BaseMonster
            var castData = new BaseMonster(data);

            // Add Monster to DB
            var result = await App.Database.InsertAsync(castData);

            // If insertion was successful, return true;
            if (result == 1)
                return true;
            return false;
        }

        // Convert the Monster to BaseMonster and update it in the SQL database
        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            // Update Monster Data
            data.Update(data);

            // Convert Monster data to BaseMonster data
            var castData = new BaseMonster(data);

            // Update BaseMonster data in DB
            var result = await App.Database.UpdateAsync(castData);

            // If update was successful, return true
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Convert the Monster to BaseMonster and delete it from the SQL database
        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            // Convert Monster data to BaseMonster data
            var castData = new BaseMonster(data);

            // Delete BaseMonster from DB
            var result = await App.Database.DeleteAsync(castData);

            // If deletion is successful, return true
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the BaseMonster from the SQL database and convert it to a Monster
        public async Task<Monster> GetAsync_Monster(string id)
        {
            // Get BaseMonster from BaseMonster table
            var result = await App.Database.GetAsync<BaseMonster>(id);

            // Convert BaseMonster to Monster
            var ret = new Monster(result);
            return ret;
        }

        // Get the list of BaseMonsters from the database and convert it to a list of Monsters
        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            // Get list of BaseMonsters from BaseMonster table.
            var result = await App.Database.Table<BaseMonster>().ToListAsync();

            // Convert list of BaseMonsters to Monsters
            var ret = result.Select(m => new Monster(m)).ToList();
            return ret;
        }
               
        #endregion Monster

        #region Score
        // Score

        // Add the score to the SQL database
        public async Task<bool> AddAsync_Score(Score data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Update the score in the SQL database
        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Delete the score from the SQL database
        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the score from the SQL database
        public async Task<Score> GetAsync_Score(string id)
        {
            var result = await App.Database.GetAsync<Score>(id);
            return result;
        }

        // Get a list of scores from the SQL database
        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Score>().ToListAsync();
            return result;

        }

        #endregion Score
    }
}