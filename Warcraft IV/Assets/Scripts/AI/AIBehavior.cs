using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehavior : MonoBehaviour 
{
        public abstract float Priority();

        public abstract IEnumerator Execute();
}
