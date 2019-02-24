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

        // Character Attacks...
        public bool TakeTurn(Character Attacker)
        {
            return true;
        }

        // Monster Attacks...
        public bool TakeTurn(Monster Attacker)
        {
            return true;
        }

        // Monster Attacks Character
        public bool TurnAsAttack(Monster Attacker, int AttackScore, Character Target, int DefenseScore)
        {
            return true;
        }

        // Character attacks Monster
        public bool TurnAsAttack(Character Attacker, int AttackScore, Monster Target, int DefenseScore)
        {
            return true;
        }

        public HitStatusEnum RollToHitTarget(int AttackScore, int DefenseScore)
        {

            HitStatus = HitStatusEnum.Unknown;

            return HitStatus;
        }

        // Decide which to attack
        public Monster AttackChoice(Character data)
        {
            return null;
        }

        // Decide which to attack
        public Character AttackChoice(Monster data)
        {
            return null;
        }

        // Will drop between 1 and 4 items from the item set...
        public List<Item> GetRandomMonsterItemDrops(int round)
        {
            var myList = new List<Item>();

            return myList;
        }

        public string DetermineCriticalMissProblem(Character attacker)
        {
            return " Not Implemented ";
        }
    }
}
