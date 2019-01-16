using System;
using SQLite;
using TRP.Controllers;
using TRP.ViewModels;
using System.Collections.Generic;

namespace TRP.Models
{
    // The Monster is the higher level concept.  This is the Character with all attirbutes defined.
    public class Monster : BaseMonster
    {
        // Remaining Experience Points to give
        public int ExperienceRemaining { get; set; }

        // Add in the actual attribute class
        public AttributeBase Attribute { get; set; }

        // Make sure Attribute is instantiated in the constructor
        public Monster()
        {
            Name = "Monster";
            Attribute = new AttributeBase();

            Alive = true;
            Level = 1;

            // Scale up to the level
            // // Implement ScaleLevel(Level);
        }

        // Passed in from creating via the Database, so use the guid passed in...
        public Monster(BaseMonster newData)
        {
            // Implement

        }

        // For making a new one for lists etc..
        public Monster(Monster newData)
        {
            // Implement

        }

        // Upgrades a monster to a set level
        public void ScaleLevel(int level)
        {
            // Implement
        }

        // Update the values passed in
        public new void Update(Monster newData)
        {
            // Implement

            return;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var UniqueOutput = "Implement";

            var myReturn = "Implement";

            // Implement

            myReturn += " , Unique Item : " + UniqueOutput;

            return myReturn;
        }

        // Calculate How much experience to return
        // Formula is the % of Damage done up to 100%  times the current experience
        // Needs to be called before applying damage
        public int CalculateExperienceEarned(int damage)
        {
            // Implement
            return 0;

        }

        #region GetAttributes
        // Get Attributes

        // Get Attack
        public int GetAttack()
        {
            // Base Attack
            var myReturn = Attribute.Attack;

            return myReturn;
        }

        // Get Speed
        public int GetSpeed()
        {
            // Base value
            var myReturn = Attribute.Speed;

            return myReturn;
        }

        // Get Defense
        public int GetDefense()
        {
            // Base value
            var myReturn = Attribute.Defense;

            return myReturn;
        }

        // Get Max Health
        public int GetHealthMax()
        {
            // Base value
            var myReturn = Attribute.MaxHealth;

            return myReturn;
        }

        // Get Current Health
        public int GetHealthCurrent()
        {
            // Base value
            var myReturn = Attribute.CurrentHealth;

            return myReturn;
        }

        // Get the Level based damage
        // Then add in the monster damage
        public int GetDamage()
        {
            var myReturn = 0;
            myReturn += Damage;

            return myReturn;
        }

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            return GetDamage();
        }

        #endregion GetAttributes

        #region Items
        // Gets the unique item (if any) from this monster when it dies...
        public Item GetUniqueItem()
        {
            var myReturn = ItemsViewModel.Instance.GetItem(UniqueItem);

            return myReturn;
        }

        // Drop all the items the monster has
        public List<Item> DropAllItems()
        {
            var myReturn = new List<Item>();

            // Drop all Items
            Item myItem;

            // Implement

            return myReturn;
        }

        #endregion Items

        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            // Implement
            return;

            // Implement   CauseDeath();
        }
    }
}