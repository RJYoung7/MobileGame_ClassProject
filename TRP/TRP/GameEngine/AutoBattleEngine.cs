using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;

namespace TRP.GameEngine
{
    public class AutoBattleEngine : BattleEngine
    {
        public BattleEngine BattleEngine = new BattleEngine();

        public bool RunAutoBattle()
        {
            // pick six characters

            var result = BattleEngine.AddCharactersToBattle();

            BattleScore.BattleNumber += getLatestBattleNumber();

            var charactersOutput = "Chosen characters: \n";

            charactersOutput += "Count: ";
            charactersOutput += BattleEngine.CharacterList.Count + "\n";

            foreach (var ch in BattleEngine.CharacterList)
            {
                charactersOutput += ch.FormatOutput();
            }
            Debug.WriteLine(charactersOutput);

            // Start a battle
            BattleEngine.StartBattle(true);
            Debug.WriteLine("Starting Battle with " + BattleEngine.CharacterList.Count + " Characters.\n");
            
            // start game by initializing a round 
            BattleEngine.StartRound();

            RoundEnum RoundResult;

            // run loop of game until end condition
            do
            {
                RoundResult = BattleEngine.RoundNextTurn();

                if (RoundResult == RoundEnum.NewRound)
                {
                    BattleEngine.NewRound();
                    Debug.WriteLine("New Round: " + BattleEngine.BattleScore.RoundCount);
                }
            } while (RoundResult != RoundEnum.GameOver);

            // when reached end condition, end game 
            BattleEngine.EndBattle();

            Debug.WriteLine(GetResultOutput());
            return true;
        }

        // Gets the current battle instance score
        public int GetScoreValue()
        {
            return BattleEngine.BattleScore.ScoreTotal;
        }

        // Gets the battle instance score object
        public Score GetScoreObject()
        {
            return BattleEngine.BattleScore;
        }

        // Get number of rounds in battle instance
        public int GetNumRounds()
        {
            return BattleEngine.BattleScore.RoundCount;
        }

        // Format result of battle to string
        public string GetResultOutput()
        {
            string myResult = "";

            myResult = "Battle Over\n\n" +
                       "Score: " + BattleEngine.BattleScore.ScoreTotal + "\n" +
                       "Experience Gained: " + BattleEngine.BattleScore.ExperienceGainedTotal + "\n" +
                       "Number of Rounds: " + BattleEngine.BattleScore.RoundCount + "\n" +
                       "Number of Turns: " + BattleEngine.BattleScore.TurnCount + "\n" +
                       "Monsters Killed: " + BattleEngine.BattleScore.MonstersKilledList + "\n";

            Debug.WriteLine(myResult);

            return myResult;
        }
    }
}
