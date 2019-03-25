using TRP.Models;
using Newtonsoft.Json.Linq;

namespace UnitTests.Models.Default
{
    public static partial class DefaultModels
    {

        public static BaseCharacter BaseCharacterDefault()
        {
            var myData = new BaseCharacter(DefaultModels.CharacterDefault());

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
