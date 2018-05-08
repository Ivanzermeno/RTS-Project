using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuildingAction : ActionBehavior 
{
        [SerializeField] GameObject buildingPrefab;

        [SerializeField] GameObject GhostBuildingPrefab;
        GameObject active = null;

        public override System.Action GetClickAction ()
        {
                return delegate ()
                {
                        GameObject go = Instantiate(GhostBuildingPrefab);
                        BuildableSite finder = go.AddComponent<BuildableSite>();
                        finder.BuildingPrefab = buildingPrefab;
                        finder.Source = gameObject.transform;
                        active = go;
                };
        }

        void Update()
        {
                if (active == null)
                {
                        return;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                        Destroy(active);
                }
        }

        void OnDestroy()
        {
                if (active == null)
                {
                        return;
                }

                Destroy(active);
        }
}
