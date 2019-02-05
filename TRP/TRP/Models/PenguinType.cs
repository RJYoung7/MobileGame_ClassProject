using System;
using System.Collections.Generic;
using System.Linq;

namespace TRP.Models
{
    // Enum to specify the type of penguin character
    // A penguin can have one type.
    public enum PenguinType
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
                var myList = Enum.GetNames(typeof(PenguinType)).ToList();
                var ret = myList;
                return ret;
            }
        }

        // Given a string for this enum, return its int value
        public static PenguinType ConvertStringToEnum(string value)
        {
            return (PenguinType)Enum.Parse(typeof(PenguinType), value);
        }

        // Given a penguin type, return its string value
        public static String ConvertEnumToString(PenguinType ptype)
        {
            switch (ptype)
            {
                case PenguinType.Adelie:
                    return "Adelie penguin";
                case PenguinType.Gentoo:
                    return "Gentoo penguin";
                case PenguinType.Little:
                    return "Little penguin";
                case PenguinType.Macaroni:
                    return "Macaroni penguin";
                case PenguinType.Magellanic:
                    return "Magellanic penguin";
                case PenguinType.Rockhopper:
                    return "Rockhopper penguin";
                case PenguinType.King:
                    return "King penguin";
                case PenguinType.Emperor:
                    return "Emperor penguin";
                default:
                    return "Unknownsadf";
            }
        }
    }
}
