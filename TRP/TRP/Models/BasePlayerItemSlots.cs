using TRP.ViewModels;
using System.Collections.Generic;
using System.Reflection;

namespace TRP.Models
{
    // Folding ItemSolts into the overall class inheritance, to show approach.  
    // C# does not support multiple inheritance
    // Could use simulated by using a pattern of interfaces, but for this, just doing it the simple way...

    public class BasePlayerItemSlots<T> : Entity<T>
    {
        // Item is a string referencing the database table
        public string Head { get; set; }

        // Feet is a string referencing the database table
        public string Feet { get; set; }

        // Necklasss is a string referencing the database table
        public string Necklass { get; set; }

        // Body is a string referencing the database table
        public string Body { get; set; }

        // PrimaryHand is a string referencing the database table
        public string PrimaryHand { get; set; }

        // Offhand is a string referencing the database table
        public string OffHand { get; set; }

        // RightFinger is a string referencing the database table
        public string RightFinger { get; set; }

        // LeftFinger is a string referencing the database table
        public string LeftFinger { get; set; }

        // Bag is a string referencing the database table
        public string Bag { get; set; }

        // This uses relfection, to get the property from a string
        // Then based on the property, it gets the value which will be the string pointing to the item
        // Then it calls to the view model who has the list of items, and asks for it
        // then it returns the formated string for the Item, and Value.
        private string FormatOutputSlot(string slot)
        {
            var myReturn = "Implement";

            return myReturn;
        }

        public string ItemSlotsFormatOutput()
        {
            var myReturn = "Implement";

            return myReturn.Trim();
        }
    }
}
