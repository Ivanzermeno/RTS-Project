using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BuildableSite : MonoBehaviour 
{
        [SerializeField] GameObject buildingPrefab;
        [SerializeField] Transform source;
		public Player player;

        Renderer rend;
        Color Red = new Color(1, 0, 0, 0.5f);
        Color Green = new Color(0, 1, 0, 0.5f);
	
        void Start()
        {
                MouseManager.Current.enabled = false;
                rend = GetComponent<Renderer>();
        }

	void Update () 
        {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                        Vector3 tempTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, hit.distance));
                        gameObject.transform.position = tempTarget;
                }

                if (SafeToPlace(gameObject))
                {
                        rend.material.color = Green;

                        if (Input.GetMouseButtonDown(0))
                        {
                                GameObject go = Instantiate(buildingPrefab, gameObject.transform.position, gameObject.transform.rotation);
                                Destroy(this.gameObject);
                        }
                }
                else
                {
                        rend.material.color = Red;
                }
	}

        bool SafeToPlace(GameObject go)
        {
                Vector3[] verts = go.GetComponent<MeshFilter>().mesh.vertices;

                NavMeshObstacle[] obstacles = GameObject.FindObjectsOfType<NavMeshObstacle>();
                List<Collider> cols = new List<Collider>();

                foreach (NavMeshObstacle o in obstacles)
                {
                        if (o.gameObject != go || o.gameObject != GameObject.FindGameObjectWithTag("Map"))
                        {
                                cols.Add(o.gameObject.GetComponent<Collider>());
                        }
                }

                foreach (Vector3 v in verts)
                {
                        NavMeshHit hit;
                        Vector3 vPos = go.transform.TransformPoint(v);
                        NavMesh.SamplePosition(vPos, out hit, go.GetComponent<CapsuleCollider>().radius, NavMesh.GetAreaFromName("Walkable"));

                        bool onXAxis = Mathf.Abs(hit.position.x - vPos.x) < 0.5f;
                        bool onZAxis = Mathf.Abs(hit.position.z - vPos.z) < 0.5f;
                        bool hitCollider = cols.Any(c => c.bounds.Contains(vPos));

                        if (!onXAxis || !onZAxis || hitCollider)
                        {
                                return false;
                        }
                }

                return true;
        }

        void OnDestroy()
        {
                MouseManager.Current.enabled = true;
        }

        public GameObject BuildingPrefab
        {
                get { return buildingPrefab; }
                set { buildingPrefab = value; }
        }

        public Transform Source
        {
                get { return source; }
                set { source = value; }
        }
}
