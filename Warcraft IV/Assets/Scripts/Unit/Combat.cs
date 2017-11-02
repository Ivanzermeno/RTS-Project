using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour 
{
        Player player;
        int damage;
        int damageAir;
        float range;
        float rangeAir;
        float rate;
        float rateAir;
        Coroutine routine; 
        GameObject target;
        [SerializeField] GameObject projectile;

        void Awake ()
        {
                Unit unit = gameObject.GetComponent<Unit>();
                damage = unit.AttackGround;
                damageAir = unit.AttackAir;
                range = unit.AttackRange;
                rangeAir = unit.AttackRangeAir;
                rate = unit.AttackSpeed;
                rateAir = unit.AttackSpeedAir;
                SphereCollider collider = gameObject.GetComponent<SphereCollider>();
                collider.radius = range;
                collider.center = gameObject.GetComponent<CapsuleCollider>().center;
        }

        void Start ()
        {
                player = gameObject.GetComponent<Player>();
        }

        public void Aggression(GameObject unit)
        {
                target = unit;

                if (routine == null)
                {
                        routine = StartCoroutine(Attacking(target));
                }
                else
                {
                        StopCoroutine(routine);
                        routine = StartCoroutine(Attacking(target));
                }
        }

        public IEnumerator Attacking(GameObject target)
        {
                Unit otherUnit = target.GetComponent<Unit>();
                Animator animation = gameObject.GetComponent<Animator>();

                Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, 100.0f);

                if (SimpleFogOfWar.FogOfWarSystem.Current.GetVisibility(target.transform.position) == SimpleFogOfWar.FogOfWarSystem.FogVisibility.Visible)
                {
                        if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= range * 2.0f)
                        {
                                gameObject.GetComponent<Pathfinding>().SendToTarget(gameObject.transform.position);

                                animation.SetBool("moving", false);

                                if (otherUnit.IsFlying && damageAir == 0)
                                {
                                        yield break;
                                }

                                animation.SetTrigger("attack");

                                yield return new WaitForSeconds(rate);

                                if (target != null && target.GetComponent<Health>().HitPoints > 0)
                                {
                                        routine = StartCoroutine(Attacking(target));
                                }
                                else
                                {
                                        StopCoroutine(routine);
                                        target = null;
                                }
                        }
                }
        }

        void OnTriggerEnter(Collider unit)
        {
                if (unit.gameObject.tag == "Unit")
                {
                        Player otherUnit;
                        otherUnit = unit.gameObject.GetComponent<Player>();

                        if (otherUnit.Info.Team != player.Info.Team && routine == null)
                        {
                                Aggression(unit.gameObject);
                        }
                }
        }

        public void Damage()
        {
                target.GetComponent<Health>().HitPoints = damage;
        }

        public void Fire()
        {
                GameObject go = Instantiate(projectile, gameObject.transform.position, projectile.transform.rotation);
                go.GetComponent<Projectile>().Damage = damage;
                go.GetComponent<Projectile>().Enemy = target;
        }

        public void Stop()
        {
                StopCoroutine(routine);
        }

        public int Attack
        {
                get { return damage; }
        }

        public int AttackAir
        {
                get { return damageAir; }
        }
}
