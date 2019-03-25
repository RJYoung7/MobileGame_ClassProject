using NUnit.Framework;
using TRP.Models;
using TRP.GameEngine;
using TRP.ViewModels;
using TRP.Services;

namespace UnitTests.Models.Default
{
    [TestFixture]
    public class CharacterUnitTests
    {
        // Character scale level to its own level should do nothing 
        [Test]
        public void Character_ScaleLevel_To_Own_Level_Should_Pass()
        {
            var newChar = new Character();
            int Expected = newChar.Level;

            newChar.ScaleLevel(Expected);
            var Actual = newChar.Level;
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Character should not scale to level 0
        [Test]
        public void Character_ScaleLevel_0_Should_Fail()
        {
            var newChar = new Character();
            int Expected = 0;

            newChar.ScaleLevel(Expected);
            var Actual = newChar.Level;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Character should not scale to negative level 
        [Test]
        public void Character_ScaleLevel_Neg1_Should_Fail()
        {
            var TestChar = new Character();
            var Actual = -1;
            var Expected = 1;

            TestChar.ScaleLevel(Actual);

            Assert.AreNotEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Character should not scale to over max level 
        [Test]
        public void Character_ScaleLevel_MaxLevel_Plus_Should_Fail()
        {
            var TestChar = new Character();
            var Level = 10000;
            var Expected = 1;
            var Actual = 1;

            TestChar.ScaleLevel(Level);

            Assert.AreEqual(Actual, Expected, TestContext.CurrentContext.Test.Name);
        }
    }
}
