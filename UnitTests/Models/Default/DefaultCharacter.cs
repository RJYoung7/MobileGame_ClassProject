using TRP.Models;


namespace UnitTests.Models.Default
{
    public static partial class DefaultModels
    {

        public static Character CharacterDefault()
        {
            var myData = new Character();

            myData.Alive = true;

            // Base information
            myData.Name = "Name";
            myData.Description = "Description";
            myData.Level = 1;
            myData.ExperienceTotal = 0;
            myData.ImageURI = null;

            myData.Attribute.Speed = 1;
            myData.Attribute.Defense = 1;
            myData.Attribute.Attack = 1;
            myData.Attribute.CurrentHealth = 1;
            myData.Attribute.MaxHealth = 1;

            // Set the strings for the items
            myData.Head = null;
            myData.Feet = null;
            myData.Necklass = null;
            myData.RightFinger = null;
            myData.LeftFinger = null;
            myData.Feet = null;

            myData.AttributeString = AttributeBase.GetAttributeString(myData.Attribute);

            return myData;
        }

    }
}
