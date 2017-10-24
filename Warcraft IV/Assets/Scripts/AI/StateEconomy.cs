using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateEconomy : AIBehavior 
{
        public abstract override float Priority();

        public abstract override IEnumerator Execute();
}
