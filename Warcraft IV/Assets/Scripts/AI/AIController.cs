using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour 
{
        private List<AIBehavior> states = new List<AIBehavior>();
        private PlayerSetup info;

        void Start ()
        {
                foreach (AIBehavior behavior in GetComponents<AIBehavior>())
                {
                        states.Add(behavior);
                }
        }
}
