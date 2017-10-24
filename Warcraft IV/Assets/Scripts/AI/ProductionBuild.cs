using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProductionBuild : StateProduction 
{
        public abstract override float Priority();

        public abstract override IEnumerator Execute();
}
