using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour 
{
        Player player;
        bool isAttacking;
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
                isAttacking = false;
        }

        void Start ()
        {
                player = gameObject.GetComponent<Player>();
        }

        public void Aggression(GameObject unit)
        {
                target = unit;

                if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= range)
                {
                        routine = StartCoroutine(Attacking());
                }
                else
                {
                        isAttacking = false;
                        gameObject.GetComponent<Pathfinding>().SendToTarget(target.transform.position);
                }
        }

        IEnumerator Attacking()
        {
                Unit otherUnit = target.GetComponent<Unit>();

                if (otherUnit.IsFlying && damageAir == 0)
                {
                        Stop();

                        yield break;
                }
                else if (gameObject.GetComponent<Unit>().AttackType == Unit.Attack.Siege && otherUnit.tag != "Building")
                {
                        Stop();

                        yield break;
                }

                Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
                direction = new Vector3(direction.x, 0.0f, direction.z);
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                if (SimpleFogOfWar.FogOfWarSystem.Current.GetVisibility(target.transform.position) == SimpleFogOfWar.FogOfWarSystem.FogVisibility.Visible)
                {
                        while (gameObject.transform.rotation.eulerAngles != lookRotation.eulerAngles)
                        {
                                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, 10.0f * Time.deltaTime);

                                if (target == null)
                                {
                                        Stop();
                                        yield break;
                                }

                                yield return null;
                        }

                        isAttacking = true;

                        Animator animation = gameObject.GetComponent<Animator>();
                        if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= 15.0f)
                        {
                                animation.SetTrigger("attack");
                        }
                        else
                        {
                                animation.SetTrigger("attack1");
                        }

                        yield return new WaitForSeconds(rate);

                        if (target != null && target.GetComponent<Health>().HitPoints > 0)
                        {
                                routine = StartCoroutine(Attacking());
                        }
                        else
                        {
                                Stop();
                        }
                }
        }

        void OnTriggerEnter(Collider unit)
        {
                if (unit.gameObject == target && !unit.isTrigger)
                {
                        routine = StartCoroutine(Attacking());
                        gameObject.GetComponent<Pathfinding>().Stop();
                }
        }

        void OnTriggerExit(Collider unit)
        {
                if (unit.gameObject == target && !unit.isTrigger)
                {
                        isAttacking = false;
                        gameObject.GetComponent<Pathfinding>().SendToTarget(target.transform.position);
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
                target = null;
                isAttacking = false;
        }

        public int Attack
        {
                get { return damage; }
        }

        public int AttackAir
        {
                get { return damageAir; }
        }

        public bool IsAttacking
        {
                get { return isAttacking; }
        }
}
