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

        public void SendToTarget (Vector3 target)
        {
                if (routine != null)
                {
                        StopCoroutine(routine);
                }

                obstacle.enabled = false;
                agent.enabled = true;

                agent.SetDestination(target);

                float time = Vector3.Distance(gameObject.transform.position, target) / agent.speed;

                routine = StartCoroutine(Moving(time));
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
                agent.angularSpeed = Mathf.Pow(unit.MovementSpeed, unit.MovementSpeed);
                agent.speed = unit.MovementSpeed;
                agent.acceleration = unit.MovementSpeed;
                agent.radius = Mathf.Pow(collider.radius, collider.radius);
                agent.avoidancePriority = 99;
	}

        IEnumerator Moving(float time)
        {
                Animator animation = gameObject.GetComponent<Animator>();
                if (agent.speed <= maxMovementSpeed)
                {
                        animation.SetBool("walking", true);
                        animation.SetBool("running", false);
                }
                else if (agent.speed > maxMovementSpeed)
                {
                        animation.SetBool("walking", false);
                        animation.SetBool("running", true);
                }   

                yield return new WaitForSeconds(time);

                animation.SetBool("walking", false);
                animation.SetBool("running", false);

                agent.enabled = false;
                obstacle.enabled = true;
        }
}
