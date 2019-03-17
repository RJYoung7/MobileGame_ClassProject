using TRP.Models;

namespace UnitTests.Models.Default
{
    public static partial class DefaultModels
    {

        // Creates an Item for the Locatoin and Attribute
        public static Item ItemDefault(ItemLocationEnum itemLocation, AttributeEnum attribute)
        {

            Item myData;

            myData = new Item
            {
                Name = "Item for " + itemLocation.ToString(),
                Description = "Auto Created",
                ImageURI = null,

                Range = 1,
                Damage = 1,
                Value = 1,

                Attribute = attribute,
                Location = itemLocation
            };

            return myData;
        }

    }
}
