using System;
using SQLite;
using TRP.Controllers;
using TRP.ViewModels;
using System.Collections.Generic;
using TRP.GameEngine;

namespace TRP.Models
{
    // The Monster is the higher level concept.  This is the Character with all attirbutes defined.
    public class Monster : BaseMonster
    {
        // Remaining Experience Points to give
        public int ExperienceRemaining { get; set; }

        // Add in the actual attribute class
        [Ignore]
        public AttributeBase Attribute { get; set; }

        // If monster has died and become zombie 
        public bool HasBeenZombie { get; set; }

        // Make sure Attribute is instantiated in the constructor
        public Monster()
        {
            Attribute = new AttributeBase();
            Alive = true;
            MonsterType = MonsterTypeEnum.Unknown;
            MonsterTypeString = "";

            // Scale up to the level
            ScaleLevel(Level);
        }

        // Create a monster from datastore
        public Monster(string name, AttributeBase ab, MonsterTypeEnum mt)
        {
            Name = name;
            Alive = true;
            Attribute = ab;
            MonsterType = mt;
            Level = 1;

            // Update this character with these properties.  Updates and fills in all properties.
            Update(this);
        }

        // Passed in from creating via the Database, so use the guid passed in...
        public Monster(BaseMonster newData)
        {
            // Base information
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = GetMonsterImage(newData.MonsterType);
            Alive = newData.Alive;
            MonsterType = newData.MonsterType;
            MonsterTypeString = GetMonsterTypeString(newData.MonsterType);

            // Database information
            Guid = newData.Guid;
            Id = newData.Id;

            // Populate the Attributes
            AttributeString = newData.AttributeString;
            Attribute = new AttributeBase(newData.AttributeString);

            // Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            Necklass = newData.Necklass;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet;
        }

        // Upgrades a monster to a set level
        public bool ScaleLevel(int level)
        {
            // Level of < 1 does not need changing
            if (level < 1)
            {
                return false;
            }

            // Don't exit on same level, because the settings below need to be calculated
            //// Same level does not need changing
            //if (level == this.Level)
            //{
            //    return false;
            //}

            // Don't go down in level...
            if (level < this.Level)
            {
                return false;
            }

            // Level > Max Level
            if (level > LevelTable.MaxLevel)
            {
                return false;
            }

            // Calculate Experience Remaining based on Lookup...
            Level = level;

            // Get the number of points at the next level, and set it for Experience Total...
            ExperienceTotal = LevelTable.Instance.LevelDetailsList[Level + 1].Experience;
            ExperienceRemaining = ExperienceTotal;

            Damage = GetLevelBasedDamage() + LevelTable.Instance.LevelDetailsList[Level].Attack;
            Attribute.Attack = LevelTable.Instance.LevelDetailsList[Level].Attack;
            Attribute.Defense = LevelTable.Instance.LevelDetailsList[Level].Defense;
            Attribute.Speed = LevelTable.Instance.LevelDetailsList[Level].Speed;

            Attribute.MaxHealth = HelperEngine.RollDice(Level, HealthDice);
            Attribute.CurrentHealth = Attribute.MaxHealth;

            AttributeString = AttributeBase.GetAttributeString(Attribute);

            return true;
        }

        // Update the values passed in
        public new void Update(Monster newData)
        {
            if (newData == null)
            {
                return;
            }

            // Update all the fields in the Data, except for the Id and guid
            //Base information
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = GetMonsterImage(newData.MonsterType);
            Alive = newData.Alive;
            MonsterType = newData.MonsterType;
            MonsterTypeString = GetMonsterTypeString(newData.MonsterType);

            //Populate the Attributes
            AttributeString = AttributeBase.GetAttributeString(newData.Attribute);
            Attribute = new AttributeBase(newData.AttributeString);

            //Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            Necklass = newData.Necklass;
            Body = newData.Body;
            PrimaryHand = newData.PrimaryHand;
            OffHand = newData.OffHand;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet;

            HasBeenZombie = false;
        }

        // Update name and make monster a zombie
        public void isZombie(String newName)
        {
            Name = newName;
            Attribute.CurrentHealth = GetHealthMax() / 2;
            HasBeenZombie = true;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            //var UniqueOutput = "Implement";

            var myReturn = this.Name;

            // Implement

            //myReturn += " , Unique Item : " + UniqueOutput;

            return myReturn;
        }

        // Calculate How much experience to return
        // Formula is the % of Damage done up to 100%  times the current experience
        // Needs to be called before applying damage
        public int CalculateExperienceEarned(int damage)
        {
            if (damage < 1)
            {
                return 0;
            }

            int remainingHealth = Math.Max(Attribute.CurrentHealth - damage, 0); // Go to 0 is OK...
            double rawPercent = (double)remainingHealth / (double)Attribute.CurrentHealth;
            double deltaPercent = 1 - rawPercent;
            var pointsAllocate = (int)Math.Floor(ExperienceRemaining * deltaPercent);

            // Catch rounding of low values, and force to 1.
            if (pointsAllocate < 1)
            {
                pointsAllocate = 1;
            }

            // Take away the points from remaining experience
            ExperienceRemaining -= pointsAllocate;
            if (ExperienceRemaining < 0)
            {
                pointsAllocate = 0;
            }

            return pointsAllocate;

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

            myItem = ItemsViewModel.Instance.GetItem(UniqueItem);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }
            return myReturn;
        }

        #endregion Items

        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            Attribute.CurrentHealth = Attribute.CurrentHealth - damage;
            if (Attribute.CurrentHealth <= 0)
            {
                Attribute.CurrentHealth = 0;
                // Death...
                CauseDeath();
            }
        }
    }
}