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
        // Create an instance of battleengine
        public BattleEngine BattleEngine = new BattleEngine();

        // The following function runs the autobattle
        public bool RunAutoBattle()
        {
            // pick six characters
            var result = BattleEngine.AddCharactersToBattle();
            if (!result)
                return false;

            // Get the latest battlenumber for the score
            BattleScore.BattleNumber += getLatestBattleNumber();

            // Debug output info about characters
            var charactersOutput = "Chosen characters: \n";
            charactersOutput += "Count: " + BattleEngine.CharacterList.Count + "\n";

            // Add each character to debug output string
            foreach (var ch in BattleEngine.CharacterList)
            {
                charactersOutput += ch.FormatOutput();
            }

            // Output the debug string for characters
            Debug.WriteLine(charactersOutput);

            // Start a battle
            BattleEngine.StartBattle(true);
            Debug.WriteLine("Starting Battle with " + BattleEngine.CharacterList.Count + " Characters.\n");
            
            // start game by initializing a round 
            BattleEngine.StartRound();

            // Variable to hold the round result
            RoundEnum RoundResult;

            // Run loop of game until end condition
            do
            {
                // Start the next turn
                RoundResult = BattleEngine.RoundNextTurn();

                // Start a new round if roundresult is set to new round
                if (RoundResult == RoundEnum.NewRound)
                {
                    // Do a new round
                    BattleEngine.NewRound();
                    Debug.WriteLine("New Round: " + BattleEngine.BattleScore.RoundCount);
                }
            } while (RoundResult != RoundEnum.GameOver);

            // When reached end condition, end game 
            BattleEngine.EndBattle();

            // Output the results to the debug window
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
