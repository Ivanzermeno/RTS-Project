using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
        float hitPoints;
        float maxHitPoints;
        float hitPointRegeneration;
        float hitPointRecoveryTime;
        Coroutine routine;

        void Awake ()
        {
                Unit unitInfo = gameObject.GetComponent<Unit>();
                maxHitPoints = unitInfo.HitPoint;
                hitPoints = maxHitPoints;
                hitPointRegeneration = -unitInfo.HitPointRegeneration;
                hitPointRecoveryTime = unitInfo.HitPointRecoveryTime;

                if (hitPointRegeneration != 0)
                {
                        routine = StartCoroutine(RegenerateHitPoints());
                }
        }

        public float HitPoints
        {
                get { return hitPoints; }
                set
                {
                        if (value > 0 && gameObject.tag == "Unit")
                        {
                                Animator animation = gameObject.GetComponent<Animator>();
                                animation.SetTrigger("getHit");
                        }

                        hitPoints -= value;
                        if (hitPoints > maxHitPoints)
                        {
                                hitPoints = maxHitPoints;
                        }
                        else if (hitPoints <= 0)
                        {
                                StopCoroutine(routine);
                                Animator animation = gameObject.GetComponent<Animator>();
                                animation.SetTrigger("dead");
                                StartCoroutine(Death(5.0f));
                        }
                }
        }

        IEnumerator RegenerateHitPoints()
        {
                this.HitPoints = hitPointRegeneration;
                yield return new WaitForSeconds(hitPointRecoveryTime);
                routine = StartCoroutine(RegenerateHitPoints());
        }

        IEnumerator Death(float timeToDie)
        {
                yield return new WaitForSeconds(timeToDie);

                Destroy(gameObject);
        }
}
