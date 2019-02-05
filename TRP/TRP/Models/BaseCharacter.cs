using System;
using TRP.GameEngine;

namespace TRP.Models
{
    public class BaseCharacter : BasePlayer<BaseCharacter>
    {
        public PenguinType PType { get; set; }

        public AttributeEnum TypeBonus { get; set; }

        public double BonusValue { get; set; }

        public string BonusString { get; set; }
        // Just base from here down. 
        // This is what will be saved to the Database

        // So when working with the database, pass Character
        public BaseCharacter()
        {

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
            PType = newData.PType;
        }

        // Update character from the passed in data
        public void Update(BaseCharacter newData)
        {
            return;
        }

        //Given the type of a character, set the image uri 
        public String GetCharacterImage(PenguinType pt)
        {
            switch (pt)
            {
                case PenguinType.Emperor:
                    return "Emperor.png";
                case PenguinType.Gentoo:
                    return "Gentoo.png";
                case PenguinType.Little:
                    return "Little.png";
                case PenguinType.Macaroni:
                    return "Macaroni.png";
                default:
                    return "Baby.png";
            }
        }

        //Given the type of a character, set the bonus type
        public AttributeEnum GetCharacterBonus(PenguinType pt)
        {
            switch (pt)
            {
                case PenguinType.Emperor:
                    return AttributeEnum.Attack;
                case PenguinType.Adelie:
                    return AttributeEnum.Defense;
                case PenguinType.Gentoo:
                    return AttributeEnum.Speed;
                case PenguinType.King:
                    return AttributeEnum.MaxHealth;
                case PenguinType.Little:
                    return AttributeEnum.Attack;
                case PenguinType.Macaroni:
                    return AttributeEnum.Defense;
                case PenguinType.Magellanic:
                    return AttributeEnum.Speed;
                case PenguinType.Rockhopper:
                    return AttributeEnum.MaxHealth;
                default:
                    return AttributeEnum.Unknown;
            }
        }

        //Given the type of a character, set the bonus value 
        public double GetCharacterBonusValue(PenguinType pt)
        {
            switch (pt)
            {
                case PenguinType.Emperor:
                    return 0.1;
                case PenguinType.Adelie:
                    return 0.1;
                case PenguinType.Gentoo:
                    return 0.1;
                case PenguinType.King:
                    return 0.1;
                case PenguinType.Little:
                    return 0.1;
                case PenguinType.Macaroni:
                    return 0.1;
                case PenguinType.Magellanic:
                    return 0.1;
                case PenguinType.Rockhopper:
                    return 0.1;
                default:
                    return 0.0;
            }
        }

        public string GetBonusString(PenguinType pt)
        {
            double value = GetCharacterBonusValue(pt)*100;
            string attribute = GetCharacterBonus(pt).ToString();

            string ret = attribute + " +" + value + "%";

            return ret;

        }

    }
}