using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TRP.Models;
using TRP.ViewModels;
using System.Linq;

namespace TRP.GameEngine
{
    public class TurnEngine
    {

        #region Properties
        // Holds the official score
        public Score BattleScore = new Score();

        // Hold the battle messages
        public BattleMessages BattleMessage = new BattleMessages();

        // Strings to store turn specific information
        public string AttackerName = string.Empty;
        public string TargetName = string.Empty;
        public string AttackStatus = string.Empty;

        // Messages for each turn
        public string TurnMessage = string.Empty;
        public string TurnMessageSpecial = string.Empty;
        public string LevelUpMessage = string.Empty;

        // Hitstatus is unknown at first
        public HitStatusEnum HitStatus = HitStatusEnum.Unknown;

        // Itempool list
        public List<Item> ItemPool = new List<Item>();

        // Lists to hold the characters and monsters
        public List<Monster> MonsterList = new List<Monster>();
        public List<Character> CharacterList = new List<Character>();

        #endregion Properties

        // Character Attacks...
        public bool TakeTurn(Character Attacker)
        {
            // Check to see if attack is null
            if (Attacker == null)
                return false;

            // If attacker is dead, don't attack
            if (!Attacker.Alive)
                return false;

            // For Attack, Choose Who
            var Target = AttackChoice(Attacker);

            // If target is null, don't attack
            if (Target == null)
            {
                return false;
            }

            // Get the attack score
            var AttackScore = Attacker.Level + Attacker.GetAttack();

            // Get the defense score
            var DefenseScore = Target.GetDefense() + Target.Level;

            // Do Attack
            TurnAsAttack(Attacker, AttackScore, Target, DefenseScore);

            return true;
        }

        // Monster Attacks...
        public bool TakeTurn(Monster Attacker)
        {
            // If attacker is null, stop turn
            if (Attacker == null)
                return false;

            // If attacker is dead, stop turn
            if (!Attacker.Alive)
                return false;

            // For Attack, Choose Who
            var Target = AttackChoice(Attacker);

            // If targe is null, stop turn
            if (Target == null)
            {
                return false;
            }

            // Get attack score
            var AttackScore = Attacker.Level + Attacker.GetAttack();

            // Get defense score
            var DefenseScore = Target.GetDefense() + Target.Level;

            // Do Attack
            TurnAsAttack(Attacker, AttackScore, Target, DefenseScore);

            return true;
        }

        // Monster Attacks Character
        public bool TurnAsAttack(Monster Attacker, int AttackScore, Character Target, int DefenseScore)
        {
            // Reset the battlemessages for the next turn
            BattleMessage.ResetBattleMessages();

            // If attacker is null, don't attack
            if (Attacker == null)
            {
                return false;
            }

            // If target is null, don't attack
            if (Target == null)
            {
                return false;
            }

            // Increment the turn count
            BattleScore.TurnCount++;

            // Set the target and attacker names
            BattleMessage.TargetName = Target.Name;
            BattleMessage.AttackerName = Attacker.Name;

            // Roll to see if the target will be hit
            var HitStatus = RollToHitTarget(AttackScore, DefenseScore);

            // It was a miss!
            if (HitStatus == HitStatusEnum.Miss)
            {
                BattleMessage.TurnMessage = Attacker.Name + " misses " + Target.Name;
                Debug.WriteLine(BattleMessage.TurnMessage);
                return true;
            }

            // It was a critical miss!
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

            
            // Calculate Damage
            BattleMessage.DamageAmount = Attacker.GetDamageRollValue();

            // Add The forced damage bonus
            BattleMessage.DamageAmount += GameGlobals.ForceMonsterDamangeBonusValue;  

            // It's a Hit
            if (HitStatus == HitStatusEnum.Hit)
            {
                Target.TakeDamage(BattleMessage.DamageAmount);
                BattleMessage.AttackStatus = string.Format(Attacker.Name + " hits for {0} damage on " + Target.Name, BattleMessage.DamageAmount);
            }

            // Check if critical hits are enabled
            if (GameGlobals.EnableCriticalHitDamage)
            {
                // It's a critical hit!
                if (BattleMessage.HitStatus == HitStatusEnum.CriticalHit)
                {
                    //2x damage
                    BattleMessage.DamageAmount += BattleMessage.DamageAmount;

                    // Give the damage to the target
                    Target.TakeDamage(BattleMessage.DamageAmount);
                    BattleMessage.AttackStatus = string.Format("CRITICAL HIT -- " + Attacker.Name + " hits really hard for {0} damage on " + Target.Name, BattleMessage.DamageAmount) + ".\n";
                }
            }

            // Set a message showing the remaining health of the target
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

                    // Add items dropped message
                    BattleMessage.TurnMessageSpecial += "\nItems dropped are (";

                    // List the items that were dropped
                    foreach (var item in myItemList)
                    {
                        BattleScore.ItemsDroppedList += item.FormatOutput() + "\n";
                        BattleMessage.TurnMessageSpecial += item.Name;
                    }
                    BattleMessage.TurnMessageSpecial += ")";

                    // Calculate chance for monster to steal item
                    if (GameGlobals.EnableMonsterStolenItem)
                    {
                        // Steal the item
                        var itemStolen = MonsterStealsItem(myItemList);
                    
                        // Show the message and remove the item from the list
                        if (itemStolen != null)
                        {
                            BattleMessage.TurnMessageSpecial += "\n" + itemStolen.Name + " was stolen! It's gone.\n";
                            myItemList.Remove(itemStolen);
                        }
                    }

                    // Add the dropped items to the item pool
                    ItemPool.AddRange(myItemList);
                }
            }
            
            // Write the turn message to the output window
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
            // Reset the battle messages
            BattleMessage.ResetBattleMessages();

            // Show what items there are in the characters bag to the output window
            Debug.WriteLine("Item: " + Attacker.GetItemByLocation(ItemLocationEnum.Bag));

            // If attacker is null, don't attack
            if (Attacker == null)
            {
                return false;
            }

            // If the target is null, don't attack
            if (Target == null)
            {
                return false;
            }

            // Increment the turncount
            BattleScore.TurnCount++;

            // Set the target name
            BattleMessage.TargetName = Target.Name;

            // Set the attacker name
            BattleMessage.AttackerName = Attacker.Name;

            // Get hit status
            var HitSuccess = RollToHitTarget(AttackScore, DefenseScore);

            // missed bool is used for mulligan
            bool missed = false;

            // Logic for a miss
            if (BattleMessage.HitStatus == HitStatusEnum.Miss)
            {
                // If mulligan is enabled, character can retry their attack
                if (GameGlobals.EnableMulligan)
                {
                    // Miss message
                    BattleMessage.TurnMessage += BattleMessage.AttackerName + " misses " + BattleMessage.TargetName + "\n";

                    // Chance for mulligan
                    var chance = 20 - ((GameGlobals.MulliganChance / 100) * 20);

                    // Determine the roll value
                    var roll = HelperEngine.RollDice(1, 20);

                    // If roll is greater than or equal to chance, set hit status to hit
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
                    // Miss message
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
                    // Miss message
                    BattleMessage.TurnMessage += BattleMessage.AttackerName + " misses " + BattleMessage.TargetName + "\n";

                    // Chance for mulligan
                    var chance = 20 - ((GameGlobals.MulliganChance / 100) * 20);

                    // Determine the rell
                    var roll = HelperEngine.RollDice(1, 20);

                    // Determine if mulligan occurs
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
                    // Critical miss message
                    BattleMessage.TurnMessage += "CRITICAL MISS-- " + Attacker.Name + " swings and critically misses " +
                                                Target.Name;
                    Debug.WriteLine(BattleMessage.TurnMessage);

                    // Critical miss problems
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

                // If mulligan occurred
                if (missed)
                {
                    Debug.WriteLine("Mulligan occured ");
                    BattleMessage.TurnMessageSpecial += "Mulligan occured.\n";
                    BattleMessage.DamageAmount /= 2;
                    BattleMessage.DamageAmount += 1;
                    missed = false;
                }

                // Add forced damange
                BattleMessage.DamageAmount += GameGlobals.ForceCharacterDamangeBonusValue;   // Add the Forced Damage Bonus (used for testing...)

                // Normal hit message
                if (BattleMessage.HitStatus == HitStatusEnum.Hit)
                {
                    BattleMessage.AttackStatus = string.Format(Attacker.Name + " hits for {0} damage on " + Target.Name, BattleMessage.DamageAmount);
                }

                // Check if critical hits are enabled
                if (GameGlobals.EnableCriticalHitDamage)
                {
                    // Its a critical hit!
                    if (BattleMessage.HitStatus == HitStatusEnum.CriticalHit)
                    {
                        //2x damage
                        BattleMessage.DamageAmount += BattleMessage.DamageAmount;
                        BattleMessage.AttackStatus = string.Format("CRITICAL HIT -- " + Attacker.Name + " hits really hard for {0} damage on " + Target.Name, BattleMessage.DamageAmount) + ".\n";
                    }
                }

                // Give the damage to the target
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

                // Calculate the amount of experience
                var experienceEarned = Target.CalculateExperienceEarned(BattleMessage.DamageAmount);

                // Check if level up occurs
                var LevelUp = Attacker.AddExperience(experienceEarned);

                // If level up occured
                if (LevelUp)
                {
                    // Level up message
                    BattleMessage.LevelUpMessage = BattleMessage.AttackerName + " is leveled up and to " + Attacker.Level + " with max health of " + Attacker.GetHealthMax();
                    Debug.WriteLine(BattleMessage.LevelUpMessage);
                }

                // Add expereience to total experience
                BattleScore.ExperienceGainedTotal += experienceEarned;
            }

            // Message for remaining health
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

                    // Add items to item pool
                    ItemPool.AddRange(myItemList);
                }
            }
            
            // Debug output
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

            // If roll is greaterthan or equal to change, rebound happens
            if (roll >= rebChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Rolls to see if there will be a hit or miss, otherwise it's base on attackscore and defense score
        public HitStatusEnum RollToHitTarget(int AttackScore, int DefenseScore)
        {
            // Roll the dice
            var d20 = HelperEngine.RollDice(1, 20);

            // Turn On UnitTestingSetRoll
            if (GameGlobals.ForceRollsToNotRandom)
            {
                // Don't let it be 0, if it was not initialized...
                if (GameGlobals.ForceToHitValue < 1)
                {
                    GameGlobals.ForceToHitValue = 1;
                }

                // Set to hit value
                d20 = GameGlobals.ForceToHitValue;
            }

            // 1 is an automatic miss
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

            // 20 is an automatic hit
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

            // Otherwise, add roll to attack score
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
            // If monsterlist is null, no monsters to attack
            if (MonsterList == null)
            {
                return null;
            }

            // If monsterlist is less than 1, no monsters to attack
            if (MonsterList.Count < 1)
            {
                return null;
            }

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
            // If characterlist is null, no characters to attack
            if (CharacterList == null)
            {
                return null;
            }

            // If characterslist is less than 1, no characters to attack
            if (CharacterList.Count < 1)
            {
                return null;
            }

            // For now, just use a simple selection of the first in the list.
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

            // Return empty list if monsters are not allowed to drop items
            if (!GameGlobals.AllowMonsterDropItems)
            {
                return myList;
            }

            var myItemsViewModel = ItemsViewModel.Instance;

            // Check to make sure dataset has items
            if (myItemsViewModel.Dataset.Count > 0)
            {
                // Random is enabled so build up a list of items dropped...
                var ItemCount = HelperEngine.RollDice(1, 4);

                // Get up to 4 items
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

        // Hackathon rule.  If critical miss happens, a problem may occur...
        public string DetermineCriticalMissProblem(Character attacker)
        {
            // No such character
            if (attacker == null)
            {
                return " Invalid Character ";
            }

            // Default message
            var myReturn = " Nothing Bad Happened ";

            // To hold the dropped item
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
