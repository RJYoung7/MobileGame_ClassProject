using TRP.Controllers;
using TRP.GameEngine;

namespace TRP.Models
{
    // The Items that a character can use, a Monster may drop, or may be randomly available.
    // The items are stored in the DB, and during game time a random item is selected.
    // The system supports CRUDi operatoins on the items
    // When in test mode, a test set of items is loaded
    // When in run mode the items from from the database
    // When in online mode, the items come from an api call to a webservice

    // When characters or monsters die, they drop items into the Items Pool for the Battle

    public class Item : Entity<Item>
    {
        // Range of the item, swords are 1, hats/rings are 0, bows are >1
        public int Range { get; set; }

        // The Damage the Item can do if it is used as a weapon in the primary hand
        public int Damage { get; set; }

        // Enum of the different attributes that the item modifies, Items can only modify one item
        public AttributeEnum Attribute { get; set; }

        // Where the Item goes on the character.  Head, Foot etc.
        public ItemLocationEnum Location { get; set; }

        // The Value item modifies.  So a ring of Health +3, has a Value of 3
        public int Value { get; set; }

        // Inheritated properties
        // Id comes from BaseEntity class
        // Name comes from the Entity class... 
        // Description comes from the Entity class
        // ImageURI comes from the Entity class

        // Item Constructor
        public Item()
        {
            CreateDefaultItem();
        }

        // Create a default item for the instantiation
        private void CreateDefaultItem()
        {
            Name = "Unknown";
            Description = "Unknown";
            ImageURI = ItemsController.DefaultImageURI;

            Range = 0;
            Value = 0;
            Damage = 0;

            Location = ItemLocationEnum.Unknown;
            Attribute = AttributeEnum.Unknown;

            ImageURI = null;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = Name + " , " +
                            Description + " for " +
                            Location.ToString() + " with " +
                            Attribute.ToString() +
                            "+" + Value + " , " +
                            "Damage : " + Damage + " , " +
                            "Range : " + Range;

            return myReturn.Trim();
        }

        // Update the item
        public Item(Item data)
        {
            Update(data);
        }

        // Constructor for Item called if needed to create a new item with set values.
        public Item(string name, string description, string imageuri, int range, int value, int damage, ItemLocationEnum location, AttributeEnum attribute)
        {
            // Create default, and then override...
            CreateDefaultItem();

            Name = name;
            Description = description;
            ImageURI = imageuri;

            Range = range;
            Value = value;
            Damage = damage;

            Location = location;
            Attribute = attribute;
        }

        // Update for Item, that will update the fields one by one.
        public void Update(Item newData)
        {
            if (newData == null)
            {
                return;
            }

            // Update all the fields in the Data, except for the Id and guid
            Name = newData.Name;
            Description = newData.Description;
            Value = newData.Value;
            Attribute = newData.Attribute;
            Location = newData.Location;
            Name = newData.Name;
            Description = newData.Description;
            ImageURI = newData.ImageURI;
            Range = newData.Range;
            Damage = newData.Damage;
        }

        // Will update the Item to be stronger...
        public void ScaleLevel(int level)
        {
            var newValue = 1;

            if (GameGlobals.ForceRollsToNotRandom)
            {
                newValue = level;
            }
            else
            {
                // Add value 1 to level passed in...
                newValue = HelperEngine.RollDice(1, level);
            }

            Value = newValue;
        }
    }
}