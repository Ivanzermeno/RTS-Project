using UnityEngine;
using UnityEngine.AI;
using SimpleFogOfWar;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor 
{
        public override void OnInspectorGUI()
        {
                Unit unitClass = target as Unit;
                unitClass.UnitName = EditorGUILayout.TextField(unitClass.UnitName);
                unitClass.Description = EditorGUILayout.TextField(unitClass.Description);

                unitClass.IsSelectable = EditorGUILayout.Toggle("Selectable", unitClass.IsSelectable);
                if (unitClass.IsSelectable == true)
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

                unitClass.IsVulnerable = EditorGUILayout.Toggle("Vulnerable", unitClass.IsVulnerable);
                if (unitClass.IsVulnerable == true)
                {
                        EditorGUI.indentLevel += 1;
                        unitClass.HitPoint = EditorGUILayout.FloatField("Hitpoints", unitClass.HitPoint);
                        unitClass.HitPointRegeneration = EditorGUILayout.FloatField("Hitpoint Regeneration:", unitClass.HitPointRegeneration);
                        unitClass.HitPointRecoveryTime = EditorGUILayout.IntField("Recovery Time:", unitClass.HitPointRecoveryTime);
                        unitClass.ArmorPoint = EditorGUILayout.IntField("Armor:", unitClass.ArmorPoint);
                        unitClass.ArmorType = (Unit.Armor) EditorGUILayout.EnumPopup("Armor Type:", unitClass.ArmorType);
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

                unitClass.IsMovable = EditorGUILayout.Toggle("Movable", unitClass.IsMovable);
                if (unitClass.IsMovable == true)
                {
                        EditorGUI.indentLevel += 1;
                        unitClass.MovementSpeed = EditorGUILayout.FloatField("Movement Speed:", unitClass.MovementSpeed);
                        unitClass.IsFlying = EditorGUILayout.Toggle("Flying", unitClass.IsFlying);
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

                unitClass.IsBuildable = EditorGUILayout.Toggle("Buildable", unitClass.IsBuildable);
                if (unitClass.IsBuildable == true)
                {
                        EditorGUI.indentLevel += 1;
                        unitClass.CostGold = EditorGUILayout.IntField("Gold Cost:", unitClass.CostGold);
                        unitClass.CostLumber = EditorGUILayout.IntField("Lumber Cost:", unitClass.CostLumber);
                        unitClass.CostPopulation = EditorGUILayout.IntField("Population Cost:", unitClass.CostPopulation);
                        unitClass.BuildTime = EditorGUILayout.IntField("Build Time:", unitClass.BuildTime);
                        EditorGUI.indentLevel -= 1;
                }

                unitClass.HasAttack = EditorGUILayout.Toggle("Attacker", unitClass.HasAttack);
                if (unitClass.HasAttack == true)
                {
                        EditorGUI.indentLevel += 1;
                        unitClass.AttackGround = EditorGUILayout.IntField("Ground Attack:", unitClass.AttackGround);
                        unitClass.AttackAir = EditorGUILayout.IntField("Air Attack:", unitClass.AttackAir);
                        unitClass.AttackRange = EditorGUILayout.FloatField("Range:", unitClass.AttackRange);
                        unitClass.AttackRangeAir = EditorGUILayout.FloatField("Air Range:", unitClass.AttackRangeAir);
                        unitClass.AttackSpeed = EditorGUILayout.FloatField("Attack Speed:", unitClass.AttackSpeed);
                        unitClass.AttackSpeedAir = EditorGUILayout.FloatField("Air Attack Speed:", unitClass.AttackSpeedAir);
                        unitClass.AttackType = (Unit.Attack) EditorGUILayout.EnumPopup("Attack Type:", unitClass.AttackType);
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

                unitClass.HasVision = EditorGUILayout.Toggle("Vision", unitClass.HasVision);
                if (unitClass.HasVision == true)
                {
                        EditorGUI.indentLevel += 1;
                        unitClass.VisionRange = EditorGUILayout.FloatField("Vision Range:", unitClass.VisionRange);
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

                unitClass.HasMana = EditorGUILayout.Toggle("Mana", unitClass.HasMana);
                if (unitClass.HasMana == true)
                {
                        EditorGUI.indentLevel += 1;
                        unitClass.ManaPool = EditorGUILayout.IntField("Mana Pool:", unitClass.ManaPool);
                        unitClass.ManaRegeneration = EditorGUILayout.FloatField("Mana Regeneration:", unitClass.ManaRegeneration);
                        EditorGUI.indentLevel -= 1;
                }

                if (GUI.changed)
                {
                        EditorUtility.SetDirty(target);
                }
        }
}