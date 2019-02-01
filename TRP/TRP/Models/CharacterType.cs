using System;
using System.Collections.Generic;
using System.Linq;

namespace TRP.Models
{
    // Helper functions for the CharacterTypeList
    public class CharacterType
    {
        public string TypeName { get; set; }

        public string ImageURI { get; set; }

        public string BonusName { get; set; }

        public int BonusValue { get; set; }

        // Add in the actual attribute class
        public CharacterType(string typename, string imageURI, string bonusName, int bonusValue)
        {
            TypeName = typename;
            ImageURI = imageURI;
            BonusName = bonusName;
            BonusValue = bonusValue;
        }

        // Update the character information
        // Updates the attribute string
        public void Update(CharacterType newData)
        {

            // Implement
            return;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = " Implement";
            return myReturn;
        }
    }
}
