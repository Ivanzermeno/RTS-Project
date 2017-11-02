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

                        Destroy(gameObject);
                }
        }

        IEnumerator Target()
        {
                while(Vector3.Distance(gameObject.transform.position, enemy.transform.position) > 0.05f)
                {
                        transform.LookAt(enemy.transform);
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemy.transform.position, speed * Time.deltaTime);

                        yield return null;
                }

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
