using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TRP.Models;
using TRP.ViewModels;
using System.Linq;

namespace TRP.GameEngine
{

    /// * 
    // * Need to decide who takes the next turn
    // * Target to Attack
    // * Should Move, or Stay put (can hit with weapon range?)
    // * Death
    // * Manage Round...
    // * /

    public class TurnEngine
    {

        #region Properties
        // Holds the official score
        public Score BattleScore = new Score();

        public string AttackerName = string.Empty;
        public string TargetName = string.Empty;
        public string AttackStatus = string.Empty;

        public string TurnMessage = string.Empty;
        public string TurnMessageSpecial = string.Empty;
        public string LevelUpMessage = string.Empty;

        public int DamageAmount = 0;
        public HitStatusEnum HitStatus = HitStatusEnum.Unknown;

        public List<Item> ItemPool = new List<Item>();

        //public List<Item> ItemList = new List<Item>();
        public List<Monster> MonsterList = new List<Monster>();
        public List<Character> CharacterList = new List<Character>();

        // Attack or Move
        // Roll To Hit
        // Decide Hit or Miss
        // Decide Damage
        // Death
        // Drop Items
        // Turn Over
        #endregion Properties

        // Character Attacks...
        public bool TakeTurn(Character Attacker)
        {
            if (Attacker == null)
                return false;

            // Choose Move or Attack
            if (!Attacker.Alive)
                return false;

            // For Attack, Choose Who
            var Target = AttackChoice(Attacker);

            if (Target == null)
            {
                return false;
            }

            // Do Attack
            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = Target.GetDefense() + Target.Level;
            TurnAsAttack(Attacker, AttackScore, Target, DefenseScore);

            return true;
        }

        // Monster Attacks...
        public bool TakeTurn(Monster Attacker)
        {
            if (Attacker == null)
                return false;

            // Choose Move or Attack
            if (!Attacker.Alive)
                return false;

            // For Attack, Choose Who
            var Target = AttackChoice(Attacker);

            if (Target == null)
            {
                return false;
            }

            // Do Attack
            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = Target.GetDefense() + Target.Level;
            TurnAsAttack(Attacker, AttackScore, Target, DefenseScore);

            return true;
        }

        // Monster Attacks Character
        public bool TurnAsAttack(Monster Attacker, int AttackScore, Character Target, int DefenseScore)
        {
            TurnMessage = string.Empty;
            TurnMessageSpecial = string.Empty;
            AttackStatus = string.Empty;

            if (Attacker == null)
            {
                return false;
            }

            if (Target == null)
            {
                return false;
            }

            BattleScore.TurnCount++;

            // Choose who to attack

            TargetName = Target.Name;
            AttackerName = Attacker.Name;

            var HitSuccess = RollToHitTarget(AttackScore, DefenseScore);

            if (HitStatus == HitStatusEnum.Miss)
            {
                TurnMessage = Attacker.Name + " misses " + Target.Name;
                Debug.WriteLine(TurnMessage);
                return true;
            }

            if (HitStatus == HitStatusEnum.CriticalMiss)
            {
                TurnMessage = Attacker.Name + " swings and really misses " + Target.Name;
                Debug.WriteLine(TurnMessage);
                return true;
            }

            // It's a Hit or a Critical Hit
            //Calculate Damage
            DamageAmount = Attacker.GetDamageRollValue();

            DamageAmount += GameGlobals.ForceMonsterDamangeBonusValue;  // Add The forced damage bonus

            if (HitStatus == HitStatusEnum.Hit)
            {
                Target.TakeDamage(DamageAmount);
                AttackStatus = string.Format(" hits for {0} damage on ", DamageAmount);
            }

            if (HitStatus == HitStatusEnum.CriticalHit)
            {
                //2x damage
                DamageAmount += DamageAmount;

                Target.TakeDamage(DamageAmount);
                AttackStatus = string.Format(" hits really hard for {0} damage on ", DamageAmount);
            }

            TurnMessageSpecial = Target.Name + "has remaining health of " + Target.Attribute.CurrentHealth;

            // Check for alive
            if (Target.Alive == false)
            {
                // Remover target from list...
                CharacterList.Remove(Target);

                // Mark Status in output
                TurnMessageSpecial = " and causes death";

                // Add the monster to the killed list
                BattleScore.CharacterAtDeathList += Target.FormatOutput() + "\n";

                // Drop Items to item Pool
                var myItemList = Target.DropAllItems();

                // Add to Score
                foreach (var item in myItemList)
                {
                    BattleScore.ItemsDroppedList += item.FormatOutput() + "\n";
                    TurnMessageSpecial += "\n\tItem: " + item.Name + " dropped";
                }

                ItemPool.AddRange(myItemList);
            }

            TurnMessage = Attacker.Name + AttackStatus + Target.Name + TurnMessageSpecial;
            Debug.WriteLine(TurnMessage);

            return true;
        }

        // Character attacks Monster
        public bool TurnAsAttack(Character Attacker, int AttackScore, Monster Target, int DefenseScore)
        {
            TurnMessage = string.Empty;
            TurnMessageSpecial = string.Empty;
            AttackStatus = string.Empty;
            LevelUpMessage = string.Empty;

            if (Attacker == null)
            {
                return false;
            }

            if (Target == null)
            {
                return false;
            }

            BattleScore.TurnCount++;

            // Choose who to attack

            TargetName = Target.Name;
            AttackerName = Attacker.Name;
            //Debug.WriteLine(AttackerName + " chooses to attack " + TargetName);

            var HitSuccess = RollToHitTarget(AttackScore, DefenseScore);

            if (HitStatus == HitStatusEnum.Miss)
            {
                TurnMessage = Attacker.Name + " misses " + Target.Name;
                Debug.WriteLine(TurnMessage);

                return true;
            }

            if (HitStatus == HitStatusEnum.CriticalMiss)
            {
                TurnMessage = Attacker.Name + " swings and critically misses " + Target.Name;
                Debug.WriteLine(TurnMessage);

                if (GameGlobals.EnableCriticalMissProblems)
                {
                    TurnMessage += DetermineCriticalMissProblem(Attacker);
                }
                return true;
            }

            // It's a Hit or a Critical Hit
            if (HitStatus == HitStatusEnum.Hit || HitStatus == HitStatusEnum.CriticalHit)
            {
                //Calculate Damage
                DamageAmount = Attacker.GetDamageRollValue();

                DamageAmount += GameGlobals.ForceCharacterDamangeBonusValue;   // Add the Forced Damage Bonus (used for testing...)

                AttackStatus = string.Format(" hits for {0} damage on ", DamageAmount);

                if (GameGlobals.EnableCriticalHitDamage)
                {
                    if (HitStatus == HitStatusEnum.CriticalHit)
                    {
                        //2x damage
                        DamageAmount += DamageAmount;
                        AttackStatus = string.Format(" hits really hard for {0} damage on ", DamageAmount) + ".\n";
                    }
                }

                Target.TakeDamage(DamageAmount);

                var experienceEarned = Target.CalculateExperienceEarned(DamageAmount);

                var LevelUp = Attacker.AddExperience(experienceEarned);
                if (LevelUp)
                {
                    LevelUpMessage = Attacker.Name + " is leveled up and is now " + Attacker.Level + " with max health of " + Attacker.GetHealthMax();
                    Debug.WriteLine(LevelUpMessage);
                }

                BattleScore.ExperienceGainedTotal += experienceEarned;
            }

            TurnMessageSpecial = "\t" + " remaining health: " + Target.Attribute.CurrentHealth;

            // Check for alive
            if (Target.Alive == false)
            {
                // Remove target from list...
                MonsterList.Remove(Target);

                // Mark Status in output
                TurnMessageSpecial = "\n\t" + Target.Name + " dies.\n";

                // Add one to the monsters killd count...
                BattleScore.MonsterSlainNumber++;

                // Add the monster to the killed list
                BattleScore.MonstersKilledList += Target.FormatOutput() + "\n";

                // Drop Items to item Pool
                var myItemList = Target.DropAllItems();

                // If Random drops are enabled, then add some....
                myItemList.AddRange(GetRandomMonsterItemDrops(BattleScore.RoundCount));

                // Add to Score
                foreach (var item in myItemList)
                {
                    BattleScore.ItemsDroppedList += item.FormatOutput() + "\n";
                    TurnMessageSpecial += " Item " + item.Name + " dropped\n";
                }

                ItemPool.AddRange(myItemList);
            }

            TurnMessage = "-" + Attacker.Name + AttackStatus + Target.Name + TurnMessageSpecial;
            Debug.WriteLine(TurnMessage + "\n");
            
            return true;
        }

        public HitStatusEnum RollToHitTarget(int AttackScore, int DefenseScore)
        {

            var d20 = HelperEngine.RollDice(1, 20);

            // Turn On UnitTestingSetRoll
            if (GameGlobals.ForceRollsToNotRandom)
            {
                // Don't let it be 0, if it was not initialized...
                if (GameGlobals.ForceToHitValue < 1)
                {
                    GameGlobals.ForceToHitValue = 1;
                }

                d20 = GameGlobals.ForceToHitValue;
            }

            if (d20 == 1)
            {
                // Force Miss
                HitStatus = HitStatusEnum.CriticalMiss;
                return HitStatus;
            }

            if (d20 == 20)
            {
                // Force Hit
                HitStatus = HitStatusEnum.CriticalHit;
                return HitStatus;
            }

            var ToHitScore = d20 + AttackScore;
            if (ToHitScore < DefenseScore)
            {
                AttackStatus = " misses ";
                // Miss
                HitStatus = HitStatusEnum.Miss;
                DamageAmount = 0;
            }
            else
            {
                // Hit
                HitStatus = HitStatusEnum.Hit;
            }

            return HitStatus;
        }

        // Decide which to attack
        public Monster AttackChoice(Character data)
        {
            if (MonsterList == null)
            {
                return null;
            }

            if (MonsterList.Count < 1)
            {
                return null;
            }

            //// For now, just use a simple selection of the first in the list.
            //// Later consider, strongest, closest, with most Health etc...
            //foreach (var Defender in MonsterList)
            //{
            //    if (Defender.Alive)
            //    {
            //        return Defender;
            //    }
            //}

            // Select first one to hit in the list for now...
            // Attack the Weakness (lowest HP) Monster first 
            var DefenderWeakest = MonsterList.OrderBy(m => m.Attribute.CurrentHealth).FirstOrDefault();
            if (DefenderWeakest.Alive)
            {
                return DefenderWeakest;
            }

            return null;
        }

        // Decide which to attack
        public Character AttackChoice(Monster data)
        {
            if (CharacterList == null)
            {
                return null;
            }

            if (CharacterList.Count < 1)
            {
                return null;
            }

            // For now, just use a simple selection of the first in the list.
            // Later consider, strongest, closest, with most Health etc...

            foreach (var Defender in CharacterList)
            {
                if (Defender.Alive)
                {
                    // Select first one to hit in the list for now...
                    return Defender;
                }
            }
            return null;
        }

        // Will drop between 1 and 4 items from the item set...
        public List<Item> GetRandomMonsterItemDrops(int round)
        {
            var myList = new List<Item>();

            if (!GameGlobals.AllowMonsterDropItems)
            {
                return myList;
            }

            var myItemsViewModel = ItemsViewModel.Instance;

            if (myItemsViewModel.Dataset.Count > 0)
            {
                // Random is enabled so build up a list of items dropped...
                var ItemCount = HelperEngine.RollDice(1, 4);
                for (var i = 0; i < ItemCount; i++)
                {
                    var rnd = HelperEngine.RollDice(1, myItemsViewModel.Dataset.Count);
                    var itemBase = myItemsViewModel.Dataset[rnd - 1];
                    var item = new Item(itemBase);
                    item.ScaleLevel(round);

                    // Make sure the item is added to the global list...
                    var myItem = ItemsViewModel.Instance.CheckIfItemExists(item);
                    if (myItem == null)
                    {
                        // Item does not exist, so add it to the datstore

                        // TODO:  Need way to not save the Item
                        ItemsViewModel.Instance.InsertUpdateAsync(item).GetAwaiter().GetResult();
                    }
                    else
                    {
                        // Swap them becaues it already exists, no need to create a new one...
                        item = myItem;
                    }

                    // Add the item to the local list...
                    myList.Add(item);
                }
            }

            return myList;
        }

        public string DetermineCriticalMissProblem(Character attacker)
        {
            if (attacker == null)
            {
                return " Invalid Character ";
            }

            var myReturn = " Nothing Bad Happened ";
            Item droppedItem;

            // It may be a critical miss, roll again and find out...
            var rnd = HelperEngine.RollDice(1, 10);
            /*
                1. Primary Hand Item breaks, and is lost forever
                2-4, Character Drops the Primary Hand Item back into the item pool
                5-6, Character drops a random equipped item back into the item pool
                7-10, Nothing bad happens, luck was with the attacker
             */

            switch (rnd)
            {
                case 1:
                    myReturn = " Luckily, nothing to drop from " + ItemLocationEnum.PrimaryHand;
                    var myItem = ItemsViewModel.Instance.GetItem(attacker.PrimaryHand);
                    if (myItem != null)
                    {
                        myReturn = " Item " + myItem.Name + " from " + ItemLocationEnum.PrimaryHand + " Broke, and lost forever";
                    }

                    attacker.PrimaryHand = null;
                    break;

                case 2:
                case 3:
                case 4:
                    // Put on the new item, which drops the one back to the pool
                    myReturn = " Luckily, nothing to drop from " + ItemLocationEnum.PrimaryHand;
                    droppedItem = attacker.AddItem(ItemLocationEnum.PrimaryHand, null);
                    if (droppedItem != null)
                    {
                        // Add the dropped item to the pool
                        ItemPool.Add(droppedItem);
                        myReturn = attacker.Name + " dropped " + droppedItem.Name + " from " + ItemLocationEnum.PrimaryHand;
                    }
                    break;

                case 5:
                case 6:
                    var LocationRnd = HelperEngine.RollDice(1, ItemLocationList.GetListCharacter.Count);
                    var myLocationEnum = ItemLocationList.GetLocationByPosition(LocationRnd);
                    myReturn = " Luckily, nothing to drop from " + myLocationEnum;

                    // Put on the new item, which drops the one back to the pool
                    droppedItem = attacker.AddItem(myLocationEnum, null);
                    if (droppedItem != null)
                    {
                        // Add the dropped item to the pool
                        ItemPool.Add(droppedItem);
                        myReturn = attacker.Name + " dropped " + droppedItem.Name + " from " + myLocationEnum;
                    }
                    break;
            }

            return myReturn;
        }
    }
}
