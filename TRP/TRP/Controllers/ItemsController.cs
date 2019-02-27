using System;
using System.Collections.Generic;
using TRP.Services;
using TRP.Models;
using TRP.ViewModels;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TRP.Controllers
{

    public class ItemsController
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static ItemsController _instance;

        // Constructor: returns instance if instantiated, otherwise creates instance if it's null 
        public static ItemsController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemsController();
                }
                return _instance;
            }
        }

        // Return the Default Image URI for the Local Image for an Item.
        public static string DefaultImageURI = "Item.png";

        #region ServerCalls
        public async Task<List<Item>> GetItemsFromServer(int parameter = 100)
        {
            var URLComponent = "GetItemList/";

            // Grab items
            var DataResult = await HttpClientService.Instance.GetJsonGetAsync(WebGlobals.WebSiteAPIURL + URLComponent + parameter);

            // Parse data
            var itemsList = ParseJson(DataResult);
            if (itemsList == null)
                return null;

            // For each item, try to insert or update it in db
            foreach (var item in itemsList)
            {
                await SQLDataStore.Instance.InsertUpdateAsync_Item(item);
            }

            // Call model to refresh itself to fetch list
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            return itemsList;
        }

        // Asks the server for items based on parameters
        // Number: items to retrieve
        // Level: what max level the items should be
        // Random: random item 
        // Attribute: which attribute the item should be for, or Unknown 
        public async Task<List<Item>> GetItemsFromGame(int number, int level, AttributeEnum attribute, ItemLocationEnum location, bool random, bool updateDataBase)
        {
            var URLComponent = "GetItemListPost/";


            var dict = new Dictionary<string, string>
            {
                { "Number", number.ToString()},
                { "Level", level.ToString()},
                { "Attribute", ((int)attribute).ToString()},
                { "Location", ((int)location).ToString()},
                { "Random", random.ToString()}

            };

            // Convert parameters to a key value pairs to a json object
            JObject finalContentJson = (JObject)JToken.FromObject(dict);

            // Make a call to the helper function to grab items
            var DataResult = await HttpClientService.Instance.GetJsonPostAsync(WebGlobals.WebSiteAPIURL + URLComponent, finalContentJson);

            // Parse items once returned
            var myList = ParseJson(DataResult);
            if (myList == null)
            {
                // Error, no results, return empty list.
                return new List<Item>();
            }


            // Update the db by inserting via view model
            if (updateDataBase)
            {
                foreach (var item in myList)
                {
                    // Call to the View Model, so view model can decide where to put data
                    await ItemsViewModel.Instance.InsertUpdateAsync(item);
                }

                // Call view model to refresh itself 
                ItemsViewModel.Instance.SetNeedsRefresh(true);
            }

            return myList;
        }


        // Parse JSON and return list of items 
        private List<Item> ParseJson(string myJsonData)
        {
            var myData = new List<Item>();

            try
            {
                JObject json;
                json = JObject.Parse(myJsonData);

                var myTempList = json["ItemList"].ToObject<List<JObject>>();

                foreach (var myItem in myTempList)
                {
                    var myTempObject = ConvertFromJson(myItem);
                    if (myTempObject != null)
                    {
                        myData.Add(myTempObject);
                    }
                }

                return myData;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return null;
            }

        }

        // Parse fields of item from JSON
        private Item ConvertFromJson(JObject json)
        {
            var myData = new Item();

            try
            {
                myData.Name = JsonHelper.GetJsonString(json, "Name");
                myData.Guid = JsonHelper.GetJsonString(json, "Guid");
                myData.Id = myData.Guid;    // Set to be the same as Guid, does not come down from server, but needed for DB
                myData.Description = JsonHelper.GetJsonString(json, "Description");
                myData.ImageURI = JsonHelper.GetJsonString(json, "ImageURI");
                myData.Value = JsonHelper.GetJsonInteger(json, "Value");
                myData.Range = JsonHelper.GetJsonInteger(json, "Range");
                myData.Damage = JsonHelper.GetJsonInteger(json, "Damage");
                myData.Location = (ItemLocationEnum)JsonHelper.GetJsonInteger(json, "Location");
                myData.Attribute = (AttributeEnum)JsonHelper.GetJsonInteger(json, "Attribute");
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return null;
            }

            return myData;
        }
        #endregion ServerCalls
    }
}
