using NUnit.Framework;
using Xamarin.Forms.Mocks;
using TRP.Models;
using TRP.GameEngine;
using TRP.ViewModels;
using TRP.Services;
using UnitTests.Models.Default;

namespace UnitTests.GameEngine
{
    [TestFixture]
    public class RoundEngineUnitTests
    {
        // Creating a new RoundEngine 
        [Test]
        public void RoundEngine_Instantiate_Should_Pass()
        {
            MockForms.Init();

            var Actual = new RoundEngine();
            Assert.AreNotEqual(null, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Starting a round in a new RoundEngine should pass
        [Test]
        public void RoundEngine_StartRound_Should_Pass()
        {
            // Arrange
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            var Expected = 1;

            // Act
            myRoundEngine.StartRound();
            var Actual = myRoundEngine.BattleScore.RoundCount;

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Ending a round that was started in a new RoundEngine should pass
        [Test]
        public void RoundEngine_EndRound_Should_Pass()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();
            myRoundEngine.EndRound();

            var Actual = myRoundEngine.BattleScore.RoundCount;
            var Expected = 1; // Started out as zero, nothing happened...

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Starting a round with no characters should return game over 
        [Test]
        public void RoundEngine_RoundNextTurn_NoCharacters_GameOver_Should_Pass()
        {
            MockForms.Init();

            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            var Actual = myRoundEngine.RoundNextTurn();
            var Expected = RoundEnum.GameOver;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        // Round engine with at least one character left should start next round 
        [Test]
        public void RoundEngine_RoundNextTurn_1_Character_Should_Have_NextTurn_Should_Pass()
        {
            MockForms.Init();

            // Start round with one monster and character
            var myRoundEngine = new RoundEngine();
            myRoundEngine.MonsterList.Add(new Monster(DefaultModels.MonsterDefault()));
            myRoundEngine.CharacterList.Add(new Character(DefaultModels.CharacterDefault()));
            myRoundEngine.StartRound();
            var Expected = RoundEnum.NextTurn;
            var Actual = myRoundEngine.RoundNextTurn();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
    }
}
