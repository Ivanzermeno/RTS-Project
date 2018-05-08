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
		public Unit unitInfo;

        void Awake ()
        {
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
                        hitPoints -= value;
                        if (value > 0 && gameObject.tag == "Unit")
                        {
                                Animator animation = gameObject.GetComponent<Animator>();

                                if (hitPoints > 0)
                                {
                                        animation.SetTrigger("getHit");
                                }
                        }
                        if (hitPoints > maxHitPoints)
                        {
                                hitPoints = maxHitPoints;
                        }
                        else if (hitPoints <= 0)
                        {
                                Animator animation = gameObject.GetComponent<Animator>();

                                gameObject.GetComponent<Combat>().enabled = false;
                                gameObject.GetComponent<Rigidbody>().useGravity = true;
                                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                                gameObject.GetComponent<Highlight>().enabled = false;

                                MouseManager.Current.Selections.Remove(gameObject.GetComponent<Interactive>());

                                animation.SetTrigger("dead");

                                routine = StartCoroutine(Death(5.0f));
                        }
                }
        }

        IEnumerator RegenerateHitPoints()
        {
                HitPoints = hitPointRegeneration;
                yield return new WaitForSeconds(hitPointRecoveryTime);
                routine = StartCoroutine(RegenerateHitPoints());
        }

        IEnumerator Death(float timeToDie)
        {
                yield return new WaitForSeconds(timeToDie);

                Destroy(gameObject);
        }

        void Drop()
        {
                
        }

        public void Stop()
        {
                StopCoroutine(routine);
        }
}
