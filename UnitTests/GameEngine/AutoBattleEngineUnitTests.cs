using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRP.Models;
using TRP.GameEngine;

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
    }
}
