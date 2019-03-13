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

        public int DamageAmount = 0;
        public int CurrentHealth = 0;

        private string htmlHead = @"<html><body bgcolor=""#E8D0B6""><p>";
        private string htmlTail = @"</p></body></html>";

        public HitStatusEnum HitStatus = HitStatusEnum.Unknown;

        /// <summary>
        /// Return formatted string
        /// </summary>
        /// <param name="hitStatus"></param>
        /// <returns></returns>
        public string GetSwingResult()
        {
            return BattleMessageHitEnum.Instance.GetMessage(HitStatus);
        }

        /// <summary>
        /// Return formatted Damage
        /// </summary>
        /// <returns></returns>
        public string GetDamageMessage()
        {
            return string.Format(" for {0} damage ", DamageAmount);
        }

        /// <summary>
        /// Returns the String Attacker Hit Defender
        /// </summary>
        /// <returns></returns>
        public string GetTurnMessage()
        {
            return AttackerName + GetSwingResult() + TargetName;
        }

        /// <summary>
        /// Remaining Health Message
        /// </summary>
        /// <returns></returns>
        public string GetCurrentHealthMessage()
        {
            return " remaining health is " + CurrentHealth.ToString();
        }

        /// <summary>
        /// Returns a blank HTML page, used for clearing the output window
        /// </summary>
        /// <returns></returns>
        public string GetHTMLBlankMessage()
        {
            var myResult = htmlHead + htmlTail;
            return myResult;
        }

        /// <summary>
        /// Output the Turn as a HTML string
        /// </summary>
        /// <returns></returns>
        public string GetHTMLFormattedTurnMessage()
        {
            var myResult = string.Empty;

            var AttackerStyle = @"<span style=""color:blue"">";
            var DefenderStyle = @"<span style=""color:green"">";

            if (PlayerType == PlayerTypeEnum.Monster)
            {
                // If monster, swap the colors
                DefenderStyle = @"<span style=""color:blue"">";
                AttackerStyle = @"<span style=""color:green"">";
            }

            var SwingResult = string.Empty;
            switch (HitStatus)
            {
                case HitStatusEnum.Miss:
                    SwingResult = @"<span style=""color:yellow"">";
                    break;

                case HitStatusEnum.CriticalMiss:
                    SwingResult = @"<span bold style=""color:yellow; font-weight:bold;"">";
                    break;

                case HitStatusEnum.CriticalHit:
                    SwingResult = @"<span bold style=""color:red; font-weight:bold;"">";
                    break;

                case HitStatusEnum.Hit:
                default:
                    SwingResult = @"<span style=""color:red"">";
                    break;
            }



            var htmlBody = string.Empty;
            htmlBody += string.Format(@"{0}{1}</span>", AttackerStyle, AttackerName);
            htmlBody += string.Format(@"{0}{1}</span>", SwingResult, GetSwingResult());
            htmlBody += string.Format(@"{0}{1}</span>", DefenderStyle, TargetName);
            htmlBody += string.Format(@"<br><span>{0}</span>", TurnMessageSpecial);
            htmlBody += string.Format(@"<br><span>{0}</span>", LevelUpMessage);

            myResult = htmlHead + htmlBody + htmlTail;
            return myResult;
        }
    }
}
