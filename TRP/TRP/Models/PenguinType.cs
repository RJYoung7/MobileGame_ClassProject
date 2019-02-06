using System;
using System.Collections.Generic;
using System.Linq;

namespace TRP.Models
{
    // Enum to specify the type of penguin character
    // A penguin can have one type.
    public enum PenguinTypeEnum
    {
        Unknown = 0,
        Adelie = 1,
        Gentoo = 2,
        Magellanic = 3,
        Rockhopper = 4,
        Macaroni = 5,
        Emperor = 6,
        Little = 7,
        King = 8,
    }

    // Helper functions for PenguinType enum
    public static class PenguinTypeList
    {
        //Returns a list of PenguinTypes a Character can have
        public static List<string> GetPenguinTypeList
        {
            get
            {
                var myList = Enum.GetNames(typeof(PenguinTypeEnum)).ToList();
                var ret = myList;
                return ret;
            }
        }

        // Given a string for this enum, return its int value
        public static PenguinTypeEnum ConvertStringToEnum(string value)
        {
            return (PenguinTypeEnum)Enum.Parse(typeof(PenguinTypeEnum), value);
        }

        // Given a penguin type, return its string value
        public static String ConvertEnumToString(PenguinTypeEnum ptype)
        {
            switch (ptype)
            {
                case PenguinTypeEnum.Adelie:
                    return "Adelie penguin";
                case PenguinTypeEnum.Gentoo:
                    return "Gentoo penguin";
                case PenguinTypeEnum.Little:
                    return "Little penguin";
                case PenguinTypeEnum.Macaroni:
                    return "Macaroni penguin";
                case PenguinTypeEnum.Magellanic:
                    return "Magellanic penguin";
                case PenguinTypeEnum.Rockhopper:
                    return "Rockhopper penguin";
                case PenguinTypeEnum.King:
                    return "King penguin";
                case PenguinTypeEnum.Emperor:
                    return "Emperor penguin";
                default:
                    return "Unknown";
            }
        }
    }
}
