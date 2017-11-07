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
                        routine = StartCoroutine(Attacking());
                }
                else
                {
                        StopCoroutine(routine);
                        routine = StartCoroutine(Attacking());
                }
        }

        IEnumerator Attacking()
        {
                Unit otherUnit = target.GetComponent<Unit>();
                Animator animation = gameObject.GetComponent<Animator>();

                Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                if (SimpleFogOfWar.FogOfWarSystem.Current.GetVisibility(target.transform.position) == SimpleFogOfWar.FogOfWarSystem.FogVisibility.Visible)
                {
                        if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= range)
                        {
                                gameObject.GetComponent<Pathfinding>().SendToTarget(gameObject.transform.position);
                                animation.SetBool("moving", false);

                                if (otherUnit.IsFlying && damageAir == 0)
                                {
                                        animation.SetTrigger("attack1");
                                        yield break;
                                }
                                else if (otherUnit.IsFlying == false)
                                {
                                        animation.SetTrigger("attack");
                                }

                                while (gameObject.transform.rotation.eulerAngles != lookRotation.eulerAngles)
                                {
                                        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, 25.0f * Time.deltaTime);

                                        if (target == null)
                                        {
                                                yield break;
                                        }

                                        yield return null;
                                }

                                yield return new WaitForSeconds(rate);

                                if (target != null && target.GetComponent<Health>().HitPoints > 0)
                                {
                                        routine = StartCoroutine(Attacking());
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
                if (unit.gameObject == target)
                {
                        routine = StartCoroutine(Attacking());
                }
        }

        void Damage()
        {
                target.GetComponent<Health>().HitPoints = damage;
        }

        void Fire()
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
