using System;
using System.Collections.Generic;
using System.Text;

namespace TRP.GameEngine
{
    class AutoBattleEngine : BattleEngine
    {
        public BattleEngine BattleEngine = new BattleEngine();

        public bool RunAutoBattle()
        {
            // pick six characters
            if (BattleEngine.AddCharactersToBattle() == false)
            {
                return false;
            }

            // start game by initializing a round 

            // run loop of game until end condition

            // when reached end condition, end game 

            return true;
        }
    }
}
