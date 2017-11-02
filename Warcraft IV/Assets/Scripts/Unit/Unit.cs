using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using SimpleFogOfWar;

[System.Serializable]
public class Unit : MonoBehaviour 
{
        [SerializeField] string unitName, description;
        [SerializeField] bool isSelectable, isVulnerable, isMovable, isBuildable, hasAttack, hasVision, hasMana, isFlying;
        [SerializeField] int hitPointRecoveryTime, armorPoint, costGold, costLumber, costPopulation, buildTime, attackGround, attackAir, manaPool, teamNumber;
        [SerializeField] float hitPoint, hitPointRegeneration, movementSpeed, attackRange, attackRangeAir, attackSpeed, attackSpeedAir, visionRange, manaRegeneration;
        [SerializeField] Armor armorType;
        [SerializeField] Attack attackType;

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

        [CanEditMultipleObjects]
        [CustomEditor(typeof(Unit))]
        public class UnitEditor : Editor
        {
                public override void OnInspectorGUI()
                {
                        Unit unitClass = target as Unit;
                        unitClass.unitName = EditorGUILayout.TextField(unitClass.unitName);
                        unitClass.description = EditorGUILayout.TextField(unitClass.description);

                        unitClass.isSelectable = EditorGUILayout.Toggle("Selectable", unitClass.isSelectable);
                        if (unitClass.isSelectable == true)
                        {
                                ActionSelect aS = unitClass.gameObject.GetComponent<ActionSelect>();
                                Highlight hl = unitClass.gameObject.GetComponent<Highlight>();
                                Interactive ia = unitClass.gameObject.GetComponent<Interactive>();
                                MapBlip mb = unitClass.gameObject.GetComponent<MapBlip>();
                                NavMeshObstacle nmo = unitClass.gameObject.GetComponent<NavMeshObstacle>();
                                CapsuleCollider cc = unitClass.gameObject.GetComponent<CapsuleCollider>();

                                if (aS == null)
                                {
                                        aS = unitClass.gameObject.AddComponent<ActionSelect>();
                                }
                                if (hl == null)
                                {
                                        hl = unitClass.gameObject.AddComponent<Highlight>();
                                }
                                if (ia == null)
                                {
                                        ia = unitClass.gameObject.AddComponent<Interactive>();
                                }
                                if (mb == null)
                                {
                                        mb = unitClass.gameObject.AddComponent<MapBlip>();
                                }
                                if (nmo == null)
                                {
                                        nmo = unitClass.gameObject.AddComponent<NavMeshObstacle>();
                                }
                                if (cc == null)
                                {
                                        cc = unitClass.gameObject.AddComponent<CapsuleCollider>();
                                }
                        }
                        else
                        {
                                DestroyImmediate(unitClass.gameObject.GetComponent<ActionSelect>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<Highlight>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<Interactive>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<MapBlip>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<NavMeshObstacle>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<CapsuleCollider>());
                        }
                       
                        unitClass.isVulnerable = EditorGUILayout.Toggle("Vulnerable", unitClass.isVulnerable);
                        if (unitClass.isVulnerable == true)
                        {
                                EditorGUI.indentLevel += 1;
                                unitClass.hitPoint = EditorGUILayout.FloatField("Hitpoints", unitClass.hitPoint);
                                unitClass.hitPointRegeneration = EditorGUILayout.FloatField("Hitpoint Regeneration:", unitClass.hitPointRegeneration);
                                unitClass.hitPointRecoveryTime = EditorGUILayout.IntField("Recovery Time:", unitClass.hitPointRecoveryTime);
                                unitClass.armorPoint = EditorGUILayout.IntField("Armor:", unitClass.armorPoint);
                                unitClass.armorType = (Armor) EditorGUILayout.EnumPopup("Armor Type:", unitClass.armorType);
                                EditorGUI.indentLevel -= 1;

                                Health h = unitClass.gameObject.GetComponent<Health>();
                                if (h == null)
                                {
                                        h = unitClass.gameObject.AddComponent<Health>();
                                }
                        }
                        else
                        {
                                DestroyImmediate(unitClass.gameObject.GetComponent<Health>());
                        }

                        unitClass.isMovable = EditorGUILayout.Toggle("Movable", unitClass.isMovable);
                        if (unitClass.isMovable == true)
                        {
                                EditorGUI.indentLevel += 1;
                                unitClass.movementSpeed = EditorGUILayout.FloatField("Movement Speed:", unitClass.movementSpeed);
                                unitClass.isFlying = EditorGUILayout.Toggle("Flying", unitClass.isFlying);
                                EditorGUI.indentLevel -= 1;

                                NavMeshObstacle nmo = unitClass.gameObject.GetComponent<NavMeshObstacle>();
                                nmo.enabled = false;

                                NavMeshAgent nma = unitClass.gameObject.GetComponent<NavMeshAgent>();
                                Pathfinding p = unitClass.gameObject.GetComponent<Pathfinding>();

                                if (nma == null)
                                {
                                        nma = unitClass.gameObject.AddComponent<NavMeshAgent>();
                                }
                                if (p == null)
                                {
                                        p = unitClass.gameObject.AddComponent<Pathfinding>();
                                }
                        }
                        else
                        {
                                DestroyImmediate(unitClass.gameObject.GetComponent<NavMeshAgent>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<Pathfinding>());
                        }

                        unitClass.isBuildable = EditorGUILayout.Toggle("Buildable", unitClass.isBuildable);
                        if (unitClass.isBuildable == true)
                        {
                                EditorGUI.indentLevel += 1;
                                unitClass.costGold = EditorGUILayout.IntField("Gold Cost:", unitClass.costGold);
                                unitClass.costLumber = EditorGUILayout.IntField("Lumber Cost:", unitClass.costLumber);
                                unitClass.costPopulation = EditorGUILayout.IntField("Population Cost:", unitClass.costPopulation);
                                unitClass.buildTime = EditorGUILayout.IntField("Build Time:", unitClass.buildTime);
                                EditorGUI.indentLevel -= 1;
                        }

                        unitClass.hasAttack = EditorGUILayout.Toggle("Attacker", unitClass.hasAttack);
                        if (unitClass.hasAttack == true)
                        {
                                EditorGUI.indentLevel += 1;
                                unitClass.attackGround = EditorGUILayout.IntField("Ground Attack:", unitClass.attackGround);
                                unitClass.attackAir = EditorGUILayout.IntField("Air Attack:", unitClass.attackAir);
                                unitClass.attackRange = EditorGUILayout.FloatField("Range:", unitClass.attackRange);
                                unitClass.attackRangeAir = EditorGUILayout.FloatField("Air Range:", unitClass.attackRangeAir);
                                unitClass.attackSpeed = EditorGUILayout.FloatField("Attack Speed:", unitClass.attackSpeed);
                                unitClass.attackSpeedAir = EditorGUILayout.FloatField("Air Attack Speed:", unitClass.attackSpeedAir);
                                unitClass.attackType = (Attack) EditorGUILayout.EnumPopup("Attack Type:", unitClass.attackType);
                                EditorGUI.indentLevel -= 1;

                                Combat c = unitClass.GetComponent<Combat>();
                                SphereCollider sc = unitClass.GetComponent<SphereCollider>();

                                if (c == null)
                                {
                                        c = unitClass.gameObject.AddComponent<Combat>();
                                }
                                if (sc == null)
                                {
                                        sc = unitClass.gameObject.AddComponent<SphereCollider>();
                                }

                                sc.isTrigger = true;
                        }
                        else
                        {
                                DestroyImmediate(unitClass.gameObject.GetComponent<Combat>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<SphereCollider>());
                        }

                        unitClass.hasVision = EditorGUILayout.Toggle("Vision", unitClass.hasVision);
                        if (unitClass.hasVision == true)
                        {
                                EditorGUI.indentLevel += 1;
                                unitClass.visionRange = EditorGUILayout.FloatField("Vision Range:", unitClass.visionRange);
                                EditorGUI.indentLevel -= 1;

                                FogOfWarInfluence fowi = unitClass.gameObject.GetComponent<FogOfWarInfluence>();

                                if (fowi == null)
                                {
                                        unitClass.gameObject.AddComponent<FogOfWarInfluence>();
                                }
                        }
                        else
                        {
                                DestroyImmediate(unitClass.gameObject.GetComponent<FogOfWarInfluence>());
                                DestroyImmediate(unitClass.gameObject.GetComponent<SphereCollider>());
                        }

                        unitClass.hasMana = EditorGUILayout.Toggle("Mana", unitClass.hasMana);
                        if (unitClass.hasMana == true)
                        {
                                EditorGUI.indentLevel += 1;
                                unitClass.manaPool = EditorGUILayout.IntField("Mana Pool:", unitClass.manaPool);
                                unitClass.manaRegeneration = EditorGUILayout.FloatField("Mana Regeneration:", unitClass.manaRegeneration);
                                EditorGUI.indentLevel -= 1;
                        }

                        if (GUI.changed)
                        {
                                EditorUtility.SetDirty(target);
                        }
                }
        }

        public bool IsSelectable { get{ return isSelectable;} set{ isSelectable = value;} }

        public bool IsVulnerable { get{ return isVulnerable;} set{ isVulnerable = value;} }
        
        public bool IsMovable { get{ return isMovable;} set{ isMovable = value;} }

        public bool IsBuildable { get{ return isBuildable;} set{ isBuildable = value;} }
       
        public bool HasAttack { get{ return hasAttack;} set{ hasAttack = value;} }

        public bool HasVision { get{ return hasVision;} set{ hasVision = value;} }

        public bool HasMana { get{ return hasMana;} set{ hasMana = value;} }

        public float HitPoint { get{ return hitPoint;} }

        public float HitPointRegeneration { get{ return hitPointRegeneration;} }
       
        public int HitPointRecoveryTime { get{ return hitPointRecoveryTime;} }

        public int ArmorPoint { get{ return ArmorPoint;} }

        public float MovementSpeed { get{ return movementSpeed;} }

        public bool IsFlying { get{ return isFlying;} }

        public int CostGold { get{ return costGold;} }

        public int CostLumber { get{ return costLumber;} }

        public int CostPopulation { get{ return costPopulation;} }

        public int BuildTime { get{ return buildTime;} }

        public int AttackGround { get{ return attackGround;} }
       
        public int AttackAir { get{ return attackAir;} }

        public float AttackRange { get{ return attackRange;} }

        public float AttackRangeAir { get{ return attackRangeAir;} }

        public float AttackSpeed { get{ return attackSpeed;} }

        public float AttackSpeedAir { get{ return attackSpeedAir;} }

        public float VisionRange { get{ return visionRange;} }
       
        public int ManaPool { get{ return manaPool;} }

        public float ManaRegeneration { get{ return manaRegeneration;} }
}