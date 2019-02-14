using System;
using TRP.GameEngine;

namespace TRP.Models
{
    public class BaseCharacter : BasePlayer<BaseCharacter>
    {
        // PenguinType data for this character
        public PenguinTypeEnum PenguinType { get; set; }

        // Penguin type bonus for this character
        public AttributeEnum TypeBonus { get; set; }

        // Penguin type bonus value for this character
        public double BonusValue { get; set; }

        // String of type bonus
        public string BonusString { get; set; }
        // Just base from here down. 
        // This is what will be saved to the Database

        // So when working with the database, pass Character
        public BaseCharacter()
        {

        }
        public BaseCharacter(string name, PenguinTypeEnum pt)
        {
            Name = name;
            Alive = true;
            PenguinType = pt;
            Level = 1;

            // Update this character with these properties.  Updates and fills in all properties.
            Update(this);
        }

        // Makes BaseCharacter using character for constructor
        public BaseCharacter(Character newData)
        {
            // Base information
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = newData.ImageURI;
            Alive = newData.Alive;
            TypeBonus = newData.TypeBonus;
            BonusValue = newData.BonusValue;
            BonusString = newData.BonusString;

            // Database information
            Guid = newData.Guid;
            Id = newData.Id;

            // Populate the Attributes
            AttributeString = newData.AttributeString;

            // Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            PrimaryHand = newData.PrimaryHand;
            OffHand = newData.OffHand;
            Body = newData.Body;
            Necklass = newData.Necklass;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet;
            PenguinType = newData.PenguinType;
        }

        // Update character from the passed in data
        public void Update(BaseCharacter newData)
        {
            return;
        }

        //Given the type of a character, set the image uri 
        public String GetCharacterImage(PenguinTypeEnum pt)
        {
            switch (pt)
            {
                case PenguinTypeEnum.Emperor:
                    return "Emperor.png";
                case PenguinTypeEnum.Gentoo:
                    return "Gentoo.png";
                case PenguinTypeEnum.Little:
                    return "Little.png";
                case PenguinTypeEnum.Macaroni:
                    return "Macaroni.png";
                case PenguinTypeEnum.Adelie:
                    return "Adelie.png";
                case PenguinTypeEnum.King:
                    return "King.png";
                case PenguinTypeEnum.Rockhopper:
                    return "Rockhopper.png";
                case PenguinTypeEnum.Magellanic:
                    return "Magellanic.png";
                default:
                    return "Baby.png";
            }
        }

        //Given the type of a character, set the bonus type
        public AttributeEnum GetCharacterBonus(PenguinTypeEnum pt)
        {
            switch (pt)
            {
                case PenguinTypeEnum.Emperor:
                    return AttributeEnum.Attack;
                case PenguinTypeEnum.Adelie:
                    return AttributeEnum.Defense;
                case PenguinTypeEnum.Gentoo:
                    return AttributeEnum.Speed;
                case PenguinTypeEnum.King:
                    return AttributeEnum.MaxHealth;
                case PenguinTypeEnum.Little:
                    return AttributeEnum.Attack;
                case PenguinTypeEnum.Macaroni:
                    return AttributeEnum.Defense;
                case PenguinTypeEnum.Magellanic:
                    return AttributeEnum.Speed;
                case PenguinTypeEnum.Rockhopper:
                    return AttributeEnum.MaxHealth;
                default:
                    return AttributeEnum.Unknown;
            }
        }

        //Given the type of a character, set the bonus value 
        public double GetCharacterBonusValue(PenguinTypeEnum pt)
        {
            switch (pt)
            {
                case PenguinTypeEnum.Emperor:
                    return 0.1;
                case PenguinTypeEnum.Adelie:
                    return 0.1;
                case PenguinTypeEnum.Gentoo:
                    return 0.1;
                case PenguinTypeEnum.King:
                    return 0.1;
                case PenguinTypeEnum.Little:
                    return 0.1;
                case PenguinTypeEnum.Macaroni:
                    return 0.1;
                case PenguinTypeEnum.Magellanic:
                    return 0.1;
                case PenguinTypeEnum.Rockhopper:
                    return 0.1;
                default:
                    return 0.0;
            }
        }

        // Given the type of a character, return its type bonus as string
        public string GetBonusString(PenguinTypeEnum pt)
        {
            double value = GetCharacterBonusValue(pt)*100;
            string attribute = GetCharacterBonus(pt).ToString();

            string ret = attribute + " +" + value + "%";

            return ret;

        }

    }
}