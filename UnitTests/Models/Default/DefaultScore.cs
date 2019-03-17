using TRP.Models;
using System;

namespace UnitTests.Models.Default
{
    public static partial class DefaultModels
    {

        public static Score ScoreDefault()
        {
            var myData = new Score();

            // Base information
            myData.Name = "Name";
            myData.Description = "Description";
            myData.ImageURI = null;

            myData.BattleNumber = 1;
            myData.ScoreTotal = 1;
            myData.GameDate = DateTime.UtcNow;
            myData.AutoBattle = false;
            myData.TurnCount = 1;
            myData.RoundCount = 1;
            myData.MonsterSlainNumber = 1;
            myData.ExperienceGainedTotal = 1;
            myData.CharacterAtDeathList = null;
            myData.MonstersKilledList = null;
            myData.ItemsDroppedList = null;

            return myData;
        }

    }
}
