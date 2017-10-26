using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour 
{
        int damage;
        int damageAir;
        float range;
        float rangeAir;
        float rate;
        float rateAir;
        Coroutine routine; 

        void Awake ()
        {
                Unit unit = gameObject.GetComponent<Unit>();
                damage = unit.AttackGround;
                damageAir = unit.AttackAir;
                range = unit.AttackRange;
                rangeAir = unit.AttackRangeAir;
                rate = unit.AttackSpeed;
                rateAir = unit.AttackSpeedAir;
        }

        public void Aggression(GameObject target)
        {
                Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 180.0f);

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

        IEnumerator Attacking(GameObject target)
        {
                Unit otherUnit = target.GetComponent<Unit>();
                Animator animation = gameObject.GetComponent<Animator>();

                if (otherUnit.IsFlying && damageAir == 0)
                {
                        yield break;
                }

                Health otherHealth = target.GetComponent<Health>();

                if (Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position) <= range)
                {
                        animation.SetTrigger("attack");

                        otherHealth.HitPoints = damage;

                        yield return new WaitForSeconds(rate);

                        if (target != null)
                        {
                                StartCoroutine(Attacking(target));
                        }
                }
        }
}
