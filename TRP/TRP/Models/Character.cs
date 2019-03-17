using TRP.GameEngine;
using TRP.ViewModels;
using System;
using System.Collections.Generic;
using SQLite;

namespace TRP.Models
{
    // The Character is the higher level concept.  This is the Character with all attirbutes defined.
    public class Character : BaseCharacter
    {
        // Add in the actual attribute class
        [Ignore]
        public AttributeBase Attribute { get; set; }

        // Whether a character has been revived from brink of death via Miracle Max
        public bool IsRevived { get; set; }

        // Make sure Attribute is instantiated in the constructor
        public Character()
        {
            Attribute = new AttributeBase();
            Alive = true;
            TypeBonus = AttributeEnum.Unknown;
            BonusValue = 0;
            BonusString = "";
        }

        // Create a character from datastore
        public Character(string name, AttributeBase ab, PenguinTypeEnum pt)
        {
            Name = name;
            Alive = true;
            Attribute = ab;
            PenguinType = pt;
            Level = 1;

            // Update this character with these properties.  Updates and fills in all properties.
            Update(this);
        }

        // Create a new character, based on a passed in BaseCharacter
        // Used for converting from database format to character
        public Character(BaseCharacter newData)
        {
            //Base information
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = GetCharacterImage(newData.PenguinType);
            Alive = newData.Alive;
            PenguinType = newData.PenguinType;
            TypeBonus = GetCharacterBonus(newData.PenguinType);
            BonusValue = GetCharacterBonusValue(newData.PenguinType);
            BonusString = GetBonusString(newData.PenguinType);

            //Database information
            Guid = newData.Guid;
            Id = newData.Id;

            //Populate the Attributes
            AttributeString = newData.AttributeString;
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
            Bag = newData.Bag;
        }

        // Create a new character, based on existing Character
        public Character(Character newData)
        {
            //Base information
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = GetCharacterImage(newData.PenguinType);
            Alive = newData.Alive;
            PenguinType = newData.PenguinType;
            TypeBonus = GetCharacterBonus(newData.PenguinType);
            BonusValue = GetCharacterBonusValue(newData.PenguinType);
            BonusString = GetBonusString(newData.PenguinType);

            //Database information
            Guid = newData.Guid;
            Id = newData.Id;

            //Populate the Attributes
            AttributeString = newData.AttributeString;
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
            Bag = newData.Bag;
        }

        // Update the character information
        // Updates the attribute string
        public void Update(Character newData)
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
            ImageURI = GetCharacterImage(newData.PenguinType);
            Alive = newData.Alive;
            PenguinType = newData.PenguinType;
            TypeBonus = GetCharacterBonus(newData.PenguinType);
            BonusValue = GetCharacterBonusValue(newData.PenguinType);
            BonusString = GetBonusString(newData.PenguinType);

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
            Bag = newData.Bag;

            // Revive
            IsRevived = false;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = Name + " (" + PenguinType.ToString() + ")" + "\n";
            return myReturn;
        }

        // Upgrades to a set level
        public void ScaleLevel(int level)
        {
            if (level > LevelTable.MaxLevel)
            {
                return;
            }

            if (level <= 1)
            {
                return;
            }

            if (level < Level)
            {
                return;
            }

            Level = level;
        }

        #region Basics
        // Level Up
        public bool LevelUp()
        {
            // Walk the Level Table descending order
            // Stop when experience is >= experience in the table
            for (var i = LevelTable.Instance.LevelDetailsList.Count - 1; i > 0; i--)
            {
                // Check the Level
                // If the Level is > Experience for the Index, increment the Level.
                if (LevelTable.Instance.LevelDetailsList[i].Experience <= ExperienceTotal)
                {
                    var NewLevel = LevelTable.Instance.LevelDetailsList[i].Level;

                    // When leveling up, the current health is adjusted up by an offset of the MaxHealth, rather than full restore
                    var OldCurrentHealth = Attribute.CurrentHealth;
                    var OldMaxHealth = Attribute.MaxHealth;

                    // Set new Health
                    // New health, is d10 of the new level.  So leveling up 1 level is 1 d10, leveling up 2 levels is 2 d10.
                    var NewHealthAddition = HelperEngine.RollDice(NewLevel - Level, 10);

                    // Increment the Max health
                    Attribute.MaxHealth += NewHealthAddition;

                    // Calculate new current health
                    // old max was 10, current health 8, new max is 15 so (15-(10-8)) = current health
                    Attribute.CurrentHealth = (Attribute.MaxHealth - (OldMaxHealth - OldCurrentHealth));

                    // Refresh the Attriburte String
                    AttributeString = AttributeBase.GetAttributeString(this.Attribute);

                    // Set the new level
                    Level = NewLevel;

                    // Done, exit
                    return true;
                }
            }

            return false;
        }

        // Level up to a number, say Level 3
        public int LevelUpToValue(int Value)
        {
            // Adjust the experience to the min for that level.
            // That will trigger level up to happen

            if (Value < 0)
            {
                // Skip, and return old level
                return Level;
            }

            if (Value <= Level)
            {
                // Skip, and return old level
                return Level;
            }

            if (Value > LevelTable.MaxLevel)
            {
                Value = LevelTable.MaxLevel;
            }

            AddExperience(LevelTable.Instance.LevelDetailsList[Value].Experience + 1);

            return Level;
        }

        // Add experience
        public bool AddExperience(int newExperience)
        {
            // Don't allow going lower in experience
            if (newExperience < 0)
            {
                return false;
            }

            // Increment the Experience
            ExperienceTotal += newExperience;

            // Can't level UP if at max.
            if (Level >= LevelTable.MaxLevel)
            {
                return false;
            }

            // Then check for Level UP
            // If experience is higher than the experience at the next level, level up is OK.
            if (ExperienceTotal >= LevelTable.Instance.LevelDetailsList[Level + 1].Experience)
            {
                return LevelUp();
            }
            return false;
        }

        #endregion Basics

        #region GetAttributes
        // Get Attributes

        // Get Attack
        public int GetAttack()
        {
            // Base Attack
            var myReturn = Attribute.Attack;

            // Attack Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Level].Attack;

            // Get Attack bonus from Items
            myReturn += GetItemBonus(AttributeEnum.Attack);

            return myReturn;
        }

        // Get Speed
        public int GetSpeed()
        {
            // Base value
            var myReturn = Attribute.Speed;

            // Get Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Level].Speed;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.Speed);

            return myReturn;
        }

        // Get Defense
        public int GetDefense()
        {
            // Base value
            var myReturn = Attribute.Defense;

            // Get Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Level].Defense;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.Defense);

            return myReturn;
        }

        // Get Max Health
        public int GetHealthMax()
        {
            // Base value
            var myReturn = Attribute.MaxHealth;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.MaxHealth);

            return myReturn;
        }

        // Get Current Health
        public int GetHealthCurrent()
        {
            // Base value
            var myReturn = Attribute.CurrentHealth;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.CurrentHealth);

            return myReturn;
        }

        // Returns the Dice for the item
        // Sword 10, is Sword Dice 10
        public int GetDamageDice()
        {
            var myReturn = 0;

            var myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            if (myItem != null)
            {
                // Damage is base damage plus dice of the weapon.  So sword of Damage 10 is d10
                myReturn += myItem.Damage;
            }

            return myReturn;
        }

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            var myReturn = GetLevelBasedDamage();

            var myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            if (myItem != null)
            {
                // Damage is base damage plus dice of the weapon.  So sword of Damage 10 is d10
                myReturn += HelperEngine.RollDice(1, myItem.Damage);
            }

            return myReturn;
        }

        #endregion GetAttributes

        #region Items
        // Drop All Items
        // Return a list of items for the pool of items
        public List<Item> DropAllItems()
        {
            var myReturn = new List<Item>();

            // Drop all Items
            Item myItem;

            myItem = RemoveItem(ItemLocationEnum.Head);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.Necklass);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.PrimaryHand);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.OffHand);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.RightFinger);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.LeftFinger);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.Feet);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            return myReturn;
        }

        // Remove Item from a set location
        // Does this by adding a new item of Null to the location
        // This will return the previous item, and put null in its place
        // Returns the item that was at the location
        // Nulls out the location
        public Item RemoveItem(ItemLocationEnum itemlocation)
        {
            var myReturn = AddItem(itemlocation, null);

            // Save Changes
            return myReturn;
        }

        // Get the Item at a known string location (head, foot etc.)
        public Item GetItem(string itemString)
        {
            return ItemsViewModel.Instance.GetItem(itemString);
        }

        // Get the Item at a known string location (head, foot etc.)
        public Item GetItemByLocation(ItemLocationEnum itemLocation)
        {
            switch (itemLocation)
            {
                case ItemLocationEnum.Head:
                    return GetItem(Head);

                case ItemLocationEnum.Necklass:
                    return GetItem(Necklass);

                case ItemLocationEnum.PrimaryHand:
                    return GetItem(PrimaryHand);

                case ItemLocationEnum.OffHand:
                    return GetItem(OffHand);

                case ItemLocationEnum.RightFinger:
                    return GetItem(RightFinger);

                case ItemLocationEnum.LeftFinger:
                    return GetItem(LeftFinger);

                case ItemLocationEnum.Feet:
                    return GetItem(Feet);

                case ItemLocationEnum.Bag:
                    return GetItem(Bag);
            }

            return null;
        }

        // Add Item
        // Looks up the Item
        // Puts the Item ID as a string in the location slot
        // If item is null, then puts null in the slot
        // Returns the item that was in the location
        public Item AddItem(ItemLocationEnum itemlocation, string itemID)
        {
            Item myReturn;

            switch (itemlocation)
            {
                case ItemLocationEnum.Feet:
                    myReturn = GetItem(Feet);
                    Feet = itemID;
                    break;

                case ItemLocationEnum.Head:
                    myReturn = GetItem(Head);
                    Head = itemID;
                    break;

                case ItemLocationEnum.Necklass:
                    myReturn = GetItem(Necklass);
                    Necklass = itemID;
                    break;

                case ItemLocationEnum.PrimaryHand:
                    myReturn = GetItem(PrimaryHand);
                    PrimaryHand = itemID;
                    break;

                case ItemLocationEnum.OffHand:
                    myReturn = GetItem(OffHand);
                    OffHand = itemID;
                    break;

                case ItemLocationEnum.RightFinger:
                    myReturn = GetItem(RightFinger);
                    RightFinger = itemID;
                    break;

                case ItemLocationEnum.LeftFinger:
                    myReturn = GetItem(LeftFinger);
                    LeftFinger = itemID;
                    break;

                case ItemLocationEnum.Bag:
                    myReturn = GetItem(Bag);
                    Bag = itemID;
                    break;

                default:
                    myReturn = null;
                    break;
            }

            return myReturn;
        }

        // Walk all the Items on the Character.
        // Add together all Items that modify the Attribute Enum Passed in
        // Return the sum
        public int GetItemBonus(AttributeEnum attributeEnum)
        {
            var myReturn = 0;
            Item myItem;

            myItem = ItemsViewModel.Instance.GetItem(Head);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(Necklass);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(OffHand);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(RightFinger);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(LeftFinger);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(Feet);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            return myReturn;
        }

        // Use an item, currently only applies to health.
        public void UseItem(Item consume)
        {
            if(consume == null)
            {
                return;
            }

            var maxHP = Attribute.MaxHealth;
            var currentHP = Attribute.CurrentHealth;

            // Prevent increaseing health past max health
            if(currentHP + consume.Value > maxHP)
            {
                Attribute.CurrentHealth = maxHP;
            }
            else
            {
                // Increase health by item value
                Attribute.CurrentHealth += consume.Value;
            }

            RemoveItem(consume.Location);
        }

        #endregion Items

        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            if (damage < 1)
            {
                return;
            }

            Attribute.CurrentHealth -= damage;
            if (GetHealthCurrent() <= 0)
            {
                // Death...
                CauseDeath();
            }
        }

        // Use Miracle Max
        public void Revive()
        {
            IsRevived = true;
            Attribute.CurrentHealth = GetHealthMax();
        }
    }
}