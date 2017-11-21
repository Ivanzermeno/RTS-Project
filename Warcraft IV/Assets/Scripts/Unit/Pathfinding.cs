using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour 
{
        NavMeshAgent agent;
        NavMeshObstacle obstacle;
        Coroutine routine;
        float maxMovementSpeed;

        public void SendToTarget (Vector3 target, Vector3 offset)
        {
                offset = gameObject.transform.position - offset;

                gameObject.GetComponent<Combat>().Stop();

                routine = StartCoroutine(Moving(target + offset));
        }

        public void SendToTarget (Vector3 target)
        {
                gameObject.GetComponent<Combat>().Stop();

                routine = StartCoroutine(Moving(target));
        }

        public float Speed
        {
                get { return agent.speed; }
                set 
                { 
                        agent.speed = value; 
                        agent.acceleration = value;
                }
        }
                
	void Awake () 
        {
                Unit unit = gameObject.GetComponent<Unit>();
                obstacle = gameObject.GetComponent<NavMeshObstacle>();
                agent = gameObject.GetComponent<NavMeshAgent>();
                CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
                maxMovementSpeed = unit.MovementSpeed;
                agent.angularSpeed = 50000.0f;
                agent.speed = unit.MovementSpeed;
                agent.acceleration = 10.0f;
                agent.stoppingDistance = 1.0f;
                agent.radius = collider.radius;
                agent.avoidancePriority = 99;
                obstacle.shape = NavMeshObstacleShape.Capsule;
                obstacle.radius = collider.radius;
                obstacle.height = collider.height;
                obstacle.center = collider.center;
	}

        IEnumerator Moving (Vector3 target)
        {
                obstacle.enabled = false;
                agent.enabled = true;

                Animator animation = gameObject.GetComponent<Animator>();

                animation.SetBool("moving", true);

                target = new Vector3(target.x, gameObject.transform.position.y, target.z);

                agent.SetDestination(target);

                while(Vector3.Distance(gameObject.transform.position, target) > agent.stoppingDistance && gameObject.GetComponent<Combat>().IsAttacking == false)
                {
                        if (gameObject.GetComponent<Combat>().IsAttacking == true)
                        {
                                animation.SetBool("moving", false);

                                agent.enabled = false;
                                obstacle.enabled = true;

                                Stop();
                        }

                        yield return null;
                }

                animation.SetBool("moving", false);

                agent.enabled = false;
                obstacle.enabled = true;
        }

        public void Stop()
        {
                if (routine != null)
                {
                        StopCoroutine(routine);
                }
                else
                {
                        routine = null;
                }
        }
}
