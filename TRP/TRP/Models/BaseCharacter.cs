using System;
using TRP.GameEngine;

namespace TRP.Models
{
    public class BaseCharacter : BasePlayer<BaseCharacter>
    {
        public PenguinType PType { get; set; }

        public AttributeEnum TypeBonus { get; set; }

        public double BonusValue { get; set; }
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
            // Database information
            Guid = newData.Guid;
            Id = newData.Id;

            // Populate the Attributes
            AttributeString = newData.AttributeString;

            // Set the strings for the items
            Head = newData.Head;
            //Feet = newData.Feet;
            //Necklass = newData.Necklass;
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
                    return 0.05;
                default:
                    return 0.0;
            }
        }
    }
}