using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour 
{
        NavMeshAgent agent;
        NavMeshObstacle obstacle;
        Vector3 unitPosition;
        Coroutine routine;
        float maxMovementSpeed;

        public void SendToTarget (Vector3 target, Vector3 offset)
        {
                offset = gameObject.transform.position - offset;

                if (routine != null)
                {
                        StopCoroutine(routine);
                }

                obstacle.enabled = false;
                agent.enabled = true;

                routine = StartCoroutine(Moving(target + offset));
        }

        public void SendToTarget (Vector3 target)
        {
                if (routine != null)
                {
                        StopCoroutine(routine);
                }

                obstacle.enabled = false;
                agent.enabled = true;

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
                unitPosition = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
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
                Animator animation = gameObject.GetComponent<Animator>();
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

                animation.SetBool("moving", true);
                rigidbody.isKinematic = false;

                while(Vector3.Distance(unitPosition, target) > agent.stoppingDistance)
                {
                        unitPosition = new Vector3(gameObject.transform.position.x, target.y, gameObject.transform.position.z);
                        agent.SetDestination(target);

                        if(Vector3.Distance(unitPosition, target) <= agent.stoppingDistance)
                        {
                                animation.SetBool("moving", false);
                                rigidbody.isKinematic = true;

                                agent.enabled = false;
                                obstacle.enabled = true;
                        }

                        yield return null;
                }
        }

        public void Stop()
        {
                StopCoroutine(routine);
        }
}
