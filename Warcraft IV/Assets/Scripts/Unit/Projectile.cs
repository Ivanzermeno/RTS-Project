using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
        int damage;
        [SerializeField][Range(1.0f, 50.0f)] float speed;
        GameObject enemy;

        void Start ()
        {
                StartCoroutine(Target());
        }

        void OnCollisionEnter (Collision col)
        {
                if (col.gameObject == enemy)
                {
                        col.gameObject.GetComponent<Health>().HitPoints = damage;

                        StartCoroutine(Destroy());
                }
        }

        IEnumerator Target ()
        {
                if (enemy != null)
                {
                        while(Vector3.Distance(gameObject.transform.position, enemy.transform.position) > 0.5f)
                        {
                                gameObject.transform.LookAt(enemy.transform);
                                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemy.transform.position, speed * Time.deltaTime);

                                if (enemy == null)
                                {
                                        StartCoroutine(Destroy());
                                        yield break;
                                }
                                yield return null;
                        }
                }

                StartCoroutine(Destroy());
        }

        IEnumerator Destroy ()
        {
                yield return new WaitForSeconds(0.5f);

                Destroy(gameObject);
        }

        public int Damage
        {
                set { damage = value; }
        }

        public GameObject Enemy
        {
                set { enemy = value; }
        }


}
