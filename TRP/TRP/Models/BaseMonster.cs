using System;
using SQLite;
using TRP.Controllers;
using TRP.ViewModels;

namespace TRP.Models
{
    public class BaseMonster : BasePlayer<BaseMonster>
    {
        // Unique Item for Monster
        public string UniqueItem { get; set; }

        // Monster type 
        public MonsterTypeEnum MonsterType { get; set; }

        // Monster type bonus
        public AttributeEnum TypeBonus { get; set; }

        // Damage the Monster can do.
        public int Damage { get; set; }

        // Just base from here down. 
        // This is what will be saved to the Database

        public BaseMonster()
        {

        }

        // Create a base from a monster, this reuses the guid and id
        public BaseMonster(Monster newData)
        {
            // Database information
            Guid = newData.Guid;
            Id = newData.Id;

            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = newData.ImageURI;
            Alive = newData.Alive;

            // Populate the Attributes
            AttributeString = newData.AttributeString;

            // Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            Necklass = newData.Necklass;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet;
            UniqueItem = newData.UniqueItem;

            // Calculate Experience Remaining based on Lookup...
            ExperienceTotal = LevelTable.Instance.LevelDetailsList[Level].Experience;

            Damage = newData.Damage;
        }

        // So when working with the database, pass Character
        public void Update(Monster newData)
        {
            return;
        }

        // Given the type of monster, set the image uri
        public String GetMonsterImage(MonsterTypeEnum mt)
        {
            switch (mt)
            {
                case MonsterTypeEnum.Shark:
                    return "Shark.png";
                case MonsterTypeEnum.LeopardSeal:
                    return "Leopardseal.png";
                case MonsterTypeEnum.PolarBear:
                    return "Polarbear.png";
                case MonsterTypeEnum.SeaEagle:
                    return "Seaeagle.png";
                case MonsterTypeEnum.SeaLion:
                    return "Sealion.png";
                case MonsterTypeEnum.Skua:
                    return "Skua.png";
                case MonsterTypeEnum.Fox:
                    return "Fox.png";
                case MonsterTypeEnum.Orca:
                    return "Orca.png";
                default:
                    return "Iceberg.png";
            }
        }
    }
}