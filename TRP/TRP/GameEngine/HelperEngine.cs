using System;
using TRP.Models;

namespace TRP.GameEngine
{
    public static class HelperEngine
    {
        /// <summary>
        /// Random should only be instantiated once
        /// Because each call to new Random will reset the seed value, and thus the numbers generated
        /// You can control the seed value for Random by passing a value to the constructor
        /// Do that if you want to be able able get the same sequence of Random over and over
        /// </summary>
        private static Random rnd = new Random();

        /// <summary>
        /// Method to Roll A Random Dice, a Set number of times
        /// </summary>
        /// <param name="rolls">The number of Rolls to Make</param>
        /// <param name="dice">The Dice to Roll</param>
        /// <returns></returns>
        public static int RollDice(int rolls, int dice)
        {
            if (rolls < 1)
            {
                return 0;
            }

            if (dice < 1)
            {
                return 0;
            }

            // Forces roll to be specified value from debug switch
            if (Models.GameGlobals.ForceRollsToNotRandom)
            {
                return rolls * Models.GameGlobals.ForcedRandomValue;
            }

            var myReturn = 0;

            for (var i = 0; i < rolls; i++)
            {
                // Add one to the dice, because random is between.  So 1-10 is rnd.Next(1,11)
                myReturn += rnd.Next(1, dice + 1);
            }

            return myReturn;
        }
    }
}
