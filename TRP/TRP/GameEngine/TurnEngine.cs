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

        public BattleMessages BattleMessage = new BattleMessages();

        public string AttackerName = string.Empty;
        public string TargetName = string.Empty;
        public string AttackStatus = string.Empty;

        public string TurnMessage = string.Empty;
        public string TurnMessageSpecial = string.Empty;
        public string LevelUpMessage = string.Empty;

        
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
            BattleMessage.ResetBattleMessages();

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

            BattleMessage.TargetName = Target.Name;
            BattleMessage.AttackerName = Attacker.Name;

            var HitStatus = RollToHitTarget(AttackScore, DefenseScore);

            if (HitStatus == HitStatusEnum.Miss)
            {
                BattleMessage.TurnMessage = Attacker.Name + " misses " + Target.Name;
                Debug.WriteLine(BattleMessage.TurnMessage);
                return true;
            }

            if (HitStatus == HitStatusEnum.CriticalMiss)
            {
                var iNum = ItemsViewModel.Instance.Dataset.Count; 
                var rnd = HelperEngine.RollDice(1, iNum);
                var idx = rnd - 1;
                Item randItem = ItemsViewModel.Instance.Dataset[idx];
                ItemPool.Add(randItem);

                BattleMessage.TurnMessage += "CRITICAL MISS-- " + Attacker.Name + " swings and critically misses " +
                                                Target.Name + " and adds " + randItem.Name + " to item pool";


                Debug.WriteLine(BattleMessage.TurnMessage);
                return true;
            }

            // It's a Hit or a Critical Hit
            //Calculate Damage
            BattleMessage.DamageAmount = Attacker.GetDamageRollValue();

            BattleMessage.DamageAmount += GameGlobals.ForceMonsterDamangeBonusValue;  // Add The forced damage bonus

            if (HitStatus == HitStatusEnum.Hit)
            {
                Target.TakeDamage(BattleMessage.DamageAmount);
                BattleMessage.AttackStatus = string.Format(Attacker.Name + " hits for {0} damage on " + Target.Name, BattleMessage.DamageAmount);
            }

            if (GameGlobals.EnableCriticalHitDamage)
            {
                if (BattleMessage.HitStatus == HitStatusEnum.CriticalHit)
                {
                    //2x damage
                    BattleMessage.DamageAmount += BattleMessage.DamageAmount;

                    Target.TakeDamage(BattleMessage.DamageAmount);
                    BattleMessage.AttackStatus = string.Format("CRITICAL HIT -- " + Attacker.Name + " hits really hard for {0} damage on " + Target.Name, BattleMessage.DamageAmount) + ".\n";
                }
            }

            BattleMessage.TurnMessageSpecial += Target.Name + " has remaining health of " + Target.Attribute.CurrentHealth;

            // Check for alive
            if (Target.Alive == false)
            {
                // Mark Status in output
                BattleMessage.TurnMessageSpecial += " and causes death.\n";

                // If character has not been revived yet, they can be revived
                if (!Target.IsRevived && GameGlobals.EnableRevivalOnce)
                {
                    Target.IsRevived = true;
                    Target.Revive();
                    BattleMessage.TurnMessageSpecial += Target.Name + " has been revived by Miracle Max!";
                }

                // Otherwise, character has been revived and stays dead
                else {
                    // Remover target from list...
                    CharacterList.Remove(Target);

                    // Add the character to the killed list
                    BattleScore.CharacterAtDeathList += Target.FormatOutput() + "\n";

                    // Drop Items to item Pool
                    var myItemList = Target.DropAllItems();

                    // Add to Score
                    BattleMessage.TurnMessageSpecial += "\nItems dropped are (";
                    foreach (var item in myItemList)
                    {
                        BattleScore.ItemsDroppedList += item.FormatOutput() + "\n";
                        BattleMessage.TurnMessageSpecial += item.Name;
                    }
                    BattleMessage.TurnMessageSpecial += ")";

                    // Calculate chance for monster to steal item
                    if (GameGlobals.EnableMonsterStolenItem)
                    {
                        var itemStolen = MonsterStealsItem(myItemList);
                    
                        if (itemStolen != null)
                        {
                            BattleMessage.TurnMessageSpecial += "\n" + itemStolen.Name + " was stolen! It's gone.\n";
                            myItemList.Remove(itemStolen);
                        }
                    }

                    ItemPool.AddRange(myItemList);
                }
            }
            
            Debug.WriteLine(BattleMessage.TurnMessage);

            return true;
        }

        // Rolls a dice and if roll is >= 17, an item from dropped items list is stolen
        public Item MonsterStealsItem(List<Item> itemsDropped)
        {
            var chance = 20 - ((GameGlobals.MonsterStealsChance / 100) * 20);
            var roll = HelperEngine.RollDice(1, 20);

            if (roll >= chance) { 
                var item = itemsDropped.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                return item;
            }
            return null;
        }

        // Character attacks Monster
        public bool TurnAsAttack(Character Attacker, int AttackScore, Monster Target, int DefenseScore)
        {
            BattleMessage.ResetBattleMessages();
            Debug.WriteLine("Item: " + Attacker.GetItemByLocation(ItemLocationEnum.Bag));

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

            BattleMessage.TargetName = Target.Name;
            BattleMessage.AttackerName = Attacker.Name;

            // Get hit status
            var HitSuccess = RollToHitTarget(AttackScore, DefenseScore);

            bool missed = false;

            // Logic for a miss
            if (BattleMessage.HitStatus == HitStatusEnum.Miss)
            {
                // If mulligan is enabled, character can retry their attack
                if (GameGlobals.EnableMulligan)
                {
                    BattleMessage.TurnMessage += BattleMessage.AttackerName + " misses " + BattleMessage.TargetName + "\n";
                    var chance = 20 - ((GameGlobals.MulliganChance / 100) * 20);
                    var roll = HelperEngine.RollDice(1, 20);

                    if (roll >= chance)
                    {
                        missed = true;
                        BattleMessage.HitStatus = HitStatusEnum.Hit;
                        Debug.WriteLine("However, there is a Mulligan");
                    }
                }
                // Otherwise they miss as normal
                else
                {
                    BattleMessage.TurnMessage += BattleMessage.AttackerName + " misses " + BattleMessage.TargetName;

                    Debug.WriteLine(BattleMessage.TurnMessage);

                    return true;
                }
            }

            // Logic for a critical miss
            if (BattleMessage.HitStatus == HitStatusEnum.CriticalMiss)
            {
                // If mulligan is enabled, character can retry their attack
                if (GameGlobals.EnableMulligan)
                {
                    BattleMessage.TurnMessage += BattleMessage.AttackerName + " misses " + BattleMessage.TargetName + "\n";
                    var chance = 20 - ((GameGlobals.MulliganChance / 100) * 20);
                    var roll = HelperEngine.RollDice(1, 20);

                    if (roll >= chance)
                    {
                        missed = true;
                        BattleMessage.HitStatus = HitStatusEnum.Hit;
                        Debug.WriteLine("However, there is a Mulligan");
                    }
                }
                // Otherwise they miss as normal
                else
                {
                    BattleMessage.TurnMessage += "CRITICAL MISS-- " + Attacker.Name + " swings and critically misses " +
                                                Target.Name;
                    Debug.WriteLine(BattleMessage.TurnMessage);

                    if (GameGlobals.EnableCriticalMissProblems)
                    {
                        BattleMessage.TurnMessage += DetermineCriticalMissProblem(Attacker);
                    }

                    return true;
                }
            }

            // Logic for a hit or critical hit
            if (BattleMessage.HitStatus == HitStatusEnum.Hit || BattleMessage.HitStatus == HitStatusEnum.CriticalHit)
            {
                //Calculate Damage
                BattleMessage.DamageAmount = Attacker.GetDamageRollValue();

                if (missed)
                {
                    Debug.WriteLine("Mulligan occured ");
                    BattleMessage.TurnMessageSpecial += "Mulligan occured.\n";
                    BattleMessage.DamageAmount /= 2;
                    BattleMessage.DamageAmount += 1;
                    missed = false;
                }

                BattleMessage.DamageAmount += GameGlobals.ForceCharacterDamangeBonusValue;   // Add the Forced Damage Bonus (used for testing...)

                // Normal hit message
                if (BattleMessage.HitStatus == HitStatusEnum.Hit)
                {
                    BattleMessage.AttackStatus = string.Format(Attacker.Name + " hits for {0} damage on " + Target.Name, BattleMessage.DamageAmount);
                }

                if (GameGlobals.EnableCriticalHitDamage)
                {
                    if (BattleMessage.HitStatus == HitStatusEnum.CriticalHit)
                    {
                        //2x damage
                        BattleMessage.DamageAmount += BattleMessage.DamageAmount;
                        BattleMessage.AttackStatus = string.Format("CRITICAL HIT -- " + Attacker.Name + " hits really hard for {0} damage on " + Target.Name, BattleMessage.DamageAmount) + ".\n";
                    }
                }

                Target.TakeDamage(BattleMessage.DamageAmount);

                // See if a rebound occurs after dealing damage to monster
                if (ReboundDamage())
                {
                    // Calculate rebound damage
                    var rebDmg = HelperEngine.RollDice(1, (BattleMessage.DamageAmount / 2));

                    // Apply damange
                    if (Attacker.GetHealthCurrent() <= rebDmg)
                    {
                        // If rebound damage would cause death, set it so that character only has one HP left.
                        rebDmg = Attacker.GetHealthCurrent() - 1;
                        Attacker.TakeDamage(rebDmg);
                        BattleMessage.TurnMessageSpecial = string.Format(Attacker.Name + " gets hit by rebound for {0}.", rebDmg);
                    }
                    else
                    {
                        // Otherwise, apply damage
                        Attacker.TakeDamage(rebDmg);
                        BattleMessage.TurnMessageSpecial = string.Format(Attacker.Name + " gets hit by rebound for {0}.", rebDmg);
                    }
                }

                var experienceEarned = Target.CalculateExperienceEarned(BattleMessage.DamageAmount);

                var LevelUp = Attacker.AddExperience(experienceEarned);
                if (LevelUp)
                {
                    BattleMessage.LevelUpMessage = BattleMessage.AttackerName + " is leveled up and to " + Attacker.Level + " with max health of " + Attacker.GetHealthMax();
                    Debug.WriteLine(BattleMessage.LevelUpMessage);
                }

                BattleScore.ExperienceGainedTotal += experienceEarned;
            }

            BattleMessage.TurnMessageSpecial += "\nRemaining health: " + Target.Attribute.CurrentHealth;

            // Check for alive
            if (Target.Alive == false)
            {
                // Check if zombies setting is on
                if (GameGlobals.EnableZombies && !Target.HasBeenZombie)
                {
                    var chance = 20 - ((GameGlobals.ZombieChance / 100) * 20);
                    var roll = HelperEngine.RollDice(1, 20);

                    // and roll to turn monster to zombie
                    if (roll >= chance)
                    {
                        BattleMessage.TurnMessageSpecial += "\n" + Target.Name + " dies but returns as a zombie.";
                        Target.isZombie("Zombie " + Target.Name);
                        Target.Alive = true;
                    }
                }
                // Otherwise, remove monster and items 
                else
                {
                    // Remove target from list...
                    MonsterList.Remove(Target);

                    // Mark Status in output
                    BattleMessage.TurnMessageSpecial += Target.Name + " dies.\n";

                    // Add one to the monsters killd count...
                    BattleScore.MonsterSlainNumber++;

                    // Add the monster to the killed list
                    BattleScore.AddMonsterToKillList(Target);

                    // Drop Items to item Pool
                    var myItemList = Target.DropAllItems();

                    // If Random drops are enabled, then add some....
                    myItemList.AddRange(GetRandomMonsterItemDrops(BattleScore.RoundCount));

                    // Add to Score
                    foreach (var item in myItemList)
                    {
                        BattleScore.ItemsDroppedList += item.FormatOutput() + "\n\n";
                        BattleMessage.TurnMessageSpecial += " Item " + item.Name + " dropped\n";
                    }

                    ItemPool.AddRange(myItemList);
                }

            }
            
            Debug.WriteLine(BattleMessage.TurnMessage + "\n");
            
            return true;
        }

        // Determine if rebound occurs
        public bool ReboundDamage()
        {
            // Get rebound chance value
            var rebChance = 20 - ((GameGlobals.ReboundChance / 100) * 20);

            // Roll to see if rebound happens.
            var roll = HelperEngine.RollDice(1, 20);

            if (roll >= rebChance)
            {
                return true;
            }
            else
            {
                return false;
            }

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
                if(GameGlobals.EnableCriticalMissProblems)
                {
                    BattleMessage.HitStatus = HitStatusEnum.CriticalMiss;
                    return BattleMessage.HitStatus;

                }
                BattleMessage.HitStatus = HitStatusEnum.Miss;

                return BattleMessage.HitStatus;
            }

            if (d20 == 20)
            {
                // Force Hit
                if (GameGlobals.EnableCriticalHitDamage)
                {
                    BattleMessage.HitStatus = HitStatusEnum.CriticalHit;
                    return BattleMessage.HitStatus;
                }

                BattleMessage.HitStatus = HitStatusEnum.Hit;

                return BattleMessage.HitStatus;
            }

            var ToHitScore = d20 + AttackScore;

            // If tohitscore is not greater than defense score, you miss
            if (ToHitScore <= DefenseScore)
            {
                BattleMessage.AttackStatus = " misses ";
                // Set hitstatus to miss
                BattleMessage.HitStatus = HitStatusEnum.Miss;
                BattleMessage.DamageAmount = 0;
            }
            else
            {
                // Set hit status to hit
                BattleMessage.HitStatus = HitStatusEnum.Hit;
            }

            return BattleMessage.HitStatus;
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
                        myItemsViewModel.AddItem_Sync(item);
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

        // Allows user to consume an item
        public void ConsumeItem(Character c)
        {
            // If no character is populated do nothing.
            if(c == null)
            {
                return;
            }
            // If nothing consumable is equipped, do nothing
            if(c.Bag == null)
            {
                BattleMessage.TurnMessageSpecial = "No items to use.";
            }
            // Else use item
            else
            {
                Debug.WriteLine("Character: " + c.Name);
                Debug.WriteLine("Turn: " + BattleScore.TurnCount);
                
                // Get consumable item
                var consumable = c.GetItemByLocation(ItemLocationEnum.Bag);

                Debug.WriteLine("Bag: " + c.Bag);
                Debug.WriteLine("Consumable: " + consumable);

                // If health is full, do not use item
                if (c.GetHealthCurrent() == c.GetHealthMax())
                {
                    BattleMessage.TurnMessageSpecial = c.Name + " has full health!";
                }
                // Use item
                else
                {
                    BattleMessage.TurnMessageSpecial = consumable.Name + " was Used. +" + consumable.Value + " " + consumable.Attribute;
                    c.UseItem(consumable);

                }

            }

            
        }
    }
}
