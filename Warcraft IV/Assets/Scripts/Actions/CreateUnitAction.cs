using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnitAction : ActionBehavior
{
        [SerializeField] GameObject prefab;
        Unit unit;
        PlayerSetup player;
        bool isTraining;

	void Start () 
        {
                unit = prefab.GetComponent<Unit>();
                player = GetComponent<Player>().Info;
	}
	
        public override System.Action GetClickAction ()
        {
                return delegate()
                {
                        if (player.Gold < unit.CostGold || player.Lumber < unit.CostLumber || isTraining == true)
                        {
                                return;
                        }

                        isTraining = true;
                        player.Gold = -unit.CostGold;
                        player.Lumber = -unit.CostLumber;
                        player.Population = unit.CostPopulation;
                        StartCoroutine(TrainUnit());
                };
        }

        IEnumerator TrainUnit ()
        {
                yield return new WaitForSeconds(unit.BuildTime);
                CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
                GameObject go = GameObject.Instantiate(prefab, transform.position + new Vector3(0.0f, 0.0f, collider.radius), Quaternion.identity);
                go.AddComponent<Player>().Info = player;
                isTraining = false;
        }
}
