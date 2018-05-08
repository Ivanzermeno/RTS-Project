using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : Interaction 
{
        GameObject displaySelectionCircle;
		public Player player;

        public override void Deselect ()
        {
                displaySelectionCircle.SetActive(false);
        }

        public override void Select ()
        {
                displaySelectionCircle.SetActive(true);
        }
	
		void Start () 
        {
                CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
                displaySelectionCircle = new GameObject("Highlight", typeof(Projector));
                displaySelectionCircle.transform.parent = gameObject.transform;
                displaySelectionCircle.transform.position = gameObject.transform.position + Vector3.up;
                displaySelectionCircle.transform.Rotate(90.0f, 0.0f, 0.0f);

                Projector projector = displaySelectionCircle.GetComponent<Projector>();
                projector.orthographic = true;
                projector.nearClipPlane = 0.0f;
                projector.farClipPlane = 25.0f;
                projector.orthographicSize = collider.radius + collider.height;
                projector.material = Resources.Load("Neutral", typeof(Material)) as Material;

                if (player.Team == 1)
                {
                        projector.material = Resources.Load("Friendly", typeof(Material)) as Material;
                }
                else if (player.Faction != Player.Factions.Neutral)
                {
                        projector.material = Resources.Load("Hostile", typeof(Material)) as Material;
                }

                displaySelectionCircle.SetActive(false);
	}

        public IEnumerator Targeted ()
        {
                if (SimpleFogOfWar.FogOfWarSystem.Current.GetVisibility(gameObject.transform.position) == SimpleFogOfWar.FogOfWarSystem.FogVisibility.Visible)
                {
                        displaySelectionCircle.SetActive(true);

                        yield return new WaitForSeconds(.5f);

                        displaySelectionCircle.SetActive(false);

                        yield return new WaitForSeconds(.5f);

                        displaySelectionCircle.SetActive(true);

                        yield return new WaitForSeconds(.5f);

                        displaySelectionCircle.SetActive(false);
                }
        }
}
