using TRP.Models;
using Newtonsoft.Json.Linq;

namespace UnitTests.Models.Default
{
    public static partial class DefaultModels
    {

        public static BaseMonster BaseMonsterDefault()
        {
            var myData = new BaseMonster();

            myData.Alive = true;

            // Base information
            myData.Name = "Name";
            myData.Description = "Description";
            myData.Level = 1;
            myData.ImageURI = null;

            myData.ExperienceTotal = 0;

            // Set the strings for the items
            myData.Head = null;
            myData.Feet = null;
            myData.Necklass = null;
            myData.RightFinger = null;
            myData.LeftFinger = null;
            myData.Feet = null;

            // Populate the Attributes
            var myAttributes = new AttributeBase();
            myAttributes.Attack = 1;
            myAttributes.Speed = 1;
            myAttributes.MaxHealth = 1;
            myAttributes.CurrentHealth = 1;
            myAttributes.Defense = 1;

            JObject myAttributesJson = (JObject)JToken.FromObject(myAttributes);
            var myAttibutesString = myAttributesJson.ToString();
            myData.AttributeString = myAttibutesString;

            return myData;
        }

    }
}
