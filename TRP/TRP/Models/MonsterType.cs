using System;
using System.Collections.Generic;
using System.Linq;

namespace TRP.Models
{
    // Enum to specify the type of monster
    // A monster can have one type.
    public enum MonsterTypeEnum
    {
        Unknown = 0,
        Orca = 1,
        SeaLion = 2,
        PolarBear = 3,
        LeopardSeal = 4,
        SeaEagle = 5,
        Skua = 6,
        Shark = 7,
        Fox = 8,
    }

    // Helper functions for MonsterType enum
    public static class MonsterTypeList
    {
        //Returns a list of MonsterType a Monster can have
        public static List<string> GetMonsterTypeList
        {
            get
            {
                var myList = from name in Enum.GetNames(typeof(MonsterTypeEnum)) where name != "Unknown" select name;
                var ret = myList.ToList();
                return ret;
            }
        }

        // Given a string for this enum, return its int value
        public static MonsterTypeEnum ConvertStringToEnum(string value)
        {
            return (MonsterTypeEnum)Enum.Parse(typeof(MonsterTypeEnum), value);
        }

        // Given a monster type, return its string value
        public static String ConvertEnumToString(MonsterTypeEnum mtype)
        {
            switch (mtype)
            {
                case MonsterTypeEnum.Orca:
                    return "Orca";
                case MonsterTypeEnum.SeaLion:
                    return "Sea Lion";
                case MonsterTypeEnum.PolarBear:
                    return "Polar Bear";
                case MonsterTypeEnum.LeopardSeal:
                    return "Leopard Seal";
                case MonsterTypeEnum.SeaEagle:
                    return "Sea Eagle";
                case MonsterTypeEnum.Skua:
                    return "Skua";
                case MonsterTypeEnum.Shark:
                    return "Shark";
                case MonsterTypeEnum.Fox:
                    return "Fox";
                default:
                    return "Unknown";
            }
        }
    }
}
