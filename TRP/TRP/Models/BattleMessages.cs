using System;
using System.Collections.Generic;
using System.Text;

namespace TRP.Models
{
    public class BattleMessages
    {
        public PlayerTypeEnum PlayerType;

        public string AttackerName = string.Empty;
        public string TargetName = string.Empty;
        public string AttackStatus = string.Empty;

        public string TurnMessage = string.Empty;
        public string TurnMessageSpecial = string.Empty;
        public string LevelUpMessage = string.Empty;

        public string TimeWarpMessage = string.Empty;

        public int DamageAmount = 0;
        public int CurrentHealth = 0;

        public HitStatusEnum HitStatus = HitStatusEnum.Unknown;

        // Return message string
        public string GetTurnMessageString()
        {
            // To concatentate multiple strings
            var myResult = "";

            // Check to see if there is a turn message
            if (!TurnMessage.Equals(""))
            {
                myResult += TurnMessage + "\n";
            }

            // Check to see if there is a Attack status
            if (!AttackStatus.Equals(""))
            {
                myResult += AttackStatus + "\n";
            }
            myResult += TurnMessageSpecial + "\n" + LevelUpMessage;

            ResetBattleMessages();

            return myResult;

        }

        // Clear message
        public void ResetBattleMessages()
        {
            AttackerName = string.Empty;
            TargetName = string.Empty;
            AttackStatus = string.Empty;

            TurnMessage = string.Empty;
            TurnMessageSpecial = string.Empty;
            LevelUpMessage = string.Empty;

            TimeWarpMessage = string.Empty;
            DamageAmount = 0;
            CurrentHealth = 0;
        }
    }
}
