using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRP.Models;
using TRP.ViewModels;
using TRP.GameEngine;
using TRP.Views;

using Xamarin.Forms.Mocks;


namespace UnitTests.GameEngine
{
    [TestFixture]
    public class AutoBattleEngineUnitTests
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        /// <summary>
        /// Can Create a new Auto Battle Instace.  
        /// Constructor should not be null
        /// </summary>
        [Test]
        public void AutoBattleEngine_Instantiate_Should_Pass()
        {

            // Arrange

            // Act
            var Actual = new AutoBattleEngine();

            // Reset

            // Assert
            Assert.AreNotEqual(null, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Get Score Value from AutoBattle
        [Test]
        public void AutoBattleEngine_GetScoreValue_Should_Pass()
        {

            var myEngine = new AutoBattleEngine();
            // No score is expected
            var Expected = 0; 

            // Get score
            var Actual = myEngine.GetScoreValue();

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }


        // Get Score Object from AutoBattle
        [Test]
        public void AutoBattleEngine_GetScoreObject_Should_Pass()
        {

            var myEngine = new AutoBattleEngine();

            // Get score object
            var Actual = myEngine.GetScoreObject();

            // Assert that it's not null.
            Assert.AreNotEqual(null, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Get Number of Rounds from AutoBattle
        [Test]
        public void AutoBattleEngine_GetNumRounds_Should_Pass()
        {

            var myEngine = new AutoBattleEngine();
            // No Rounds
            var Expected = 0;

            // Get number of rounds
            var Actual = myEngine.GetNumRounds();

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Get output string
        [Test]
        public void AutoBattleEngine_GetResultOutput_Should_Pass()
        {
            var myEngine = new AutoBattleEngine();

            // Get number of rounds
            var Actual = myEngine.GetResultOutput();

            // Assert that it's not null.
            Assert.AreNotEqual(null, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Add characters to battle when character list is already 6 returns true
        [Test]
        public void AutoBattleEngine_AddCharactersToBattle_Count_is_6_Should_Pass()
        {
            // Arrange
            var myEngine = new BattleEngine();
            var Expected = true;

            for (var i = 0; i < 6; i++)
            {
                myEngine.CharacterList.Add(new Character());
            }

            // Act
            var Actual = myEngine.AddCharactersToBattle();

            // Assert that it's not null.
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }



    }
}
