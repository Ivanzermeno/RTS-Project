using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleFogOfWar;

[CreateAssetMenu(fileName = "Unit")]
[System.Serializable]
public class Unit : ScriptableObject
{
        [SerializeField] protected string name, description;
        [SerializeField] protected bool isSelectable, isVulnerable, isMovable, isBuildable, hasAttack, hasVision, hasMana, isFlying;
	[SerializeField] protected int hitPointRecoveryTime, armorPoint, costGold, costLumber, costPopulation, buildTime, attackGround, attackAir, manaPool;
        [SerializeField] protected float hitPoint, hitPointRegeneration, movementSpeed, attackRange, attackRangeAir, attackSpeed, attackSpeedAir, visionRange, manaRegeneration;
        [SerializeField] protected Armor armorType;
        [SerializeField] protected Attack attackType;

        public enum Armor
        {
                Unarmored,
                Light,
                Medium,
                Heavy,
                Divine,
                Fortified
        };

        public enum Attack
        {
                Slash,
                Pierce,
                Bludgeon,
                Magic,
                Heroic,
                Siege,
                Chaos
        };

        public string Name { get{ return name;} set{ name = value;} } 

        public string Description { get{ return description;} set{ description = value;} }

        public bool IsSelectable { get{ return isSelectable;} set{ isSelectable = value;} }

        public bool IsVulnerable { get{ return isVulnerable;} set{ isVulnerable = value;} }
        
        public bool IsMovable { get{ return isMovable;} set{ isMovable = value;} }

        public bool IsBuildable { get{ return isBuildable;} set{ isBuildable = value;} }
       
        public bool HasAttack { get{ return hasAttack;} set{ hasAttack = value;} }

        public bool HasVision { get{ return hasVision;} set{ hasVision = value;} }

        public bool HasMana { get{ return hasMana;} set{ hasMana = value;} }

        public float HitPoint { get{ return hitPoint;} set{ hitPoint = value;} }

        public float HitPointRegeneration { get{ return hitPointRegeneration;} set{ hitPointRegeneration = value;} }
       
        public int HitPointRecoveryTime { get{ return hitPointRecoveryTime;} set{ hitPointRecoveryTime = value;} }

        public int ArmorPoint { get{ return armorPoint;} set{ armorPoint = value;} }

        public Armor ArmorType { get{ return armorType;} set{ armorType = value;} }

        public float MovementSpeed { get{ return movementSpeed;} set{ movementSpeed = value;} }

        public bool IsFlying { get{ return isFlying;} set{ isFlying = value;} }

        public int CostGold { get{ return costGold;} set{ costGold = value;} }

        public int CostLumber { get{ return costLumber;} set{ costLumber = value;} }

        public int CostPopulation { get{ return costPopulation;} set{ costPopulation = value;} }

        public int BuildTime { get{ return buildTime;} set{ buildTime = value;} }

        public int AttackGround { get{ return attackGround;} set{ attackGround = value;} }
       
        public int AttackAir { get{ return attackAir;} set{ attackAir = value;} }

        public float AttackRange { get{ return attackRange;} set{ attackRange = value;} }

        public float AttackRangeAir { get{ return attackRangeAir;} set{ attackRangeAir = value;} }

        public float AttackSpeed { get{ return attackSpeed;} set{ attackSpeed = value;} }

        public float AttackSpeedAir { get{ return attackSpeedAir;} set{ attackSpeedAir = value;} }

        public Attack AttackType { get{ return attackType;} set{ attackType = value;} }

        public float VisionRange { get{ return visionRange;} set{ visionRange = value;} }
       
        public int ManaPool { get{ return manaPool;} set{ manaPool = value;} }

        public float ManaRegeneration { get{ return manaRegeneration;} set{ manaRegeneration = value;} }
}