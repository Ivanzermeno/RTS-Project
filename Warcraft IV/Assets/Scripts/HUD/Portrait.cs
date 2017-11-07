using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour 
{
        [SerializeField] List<Interactive> selections = new List<Interactive>();
        [SerializeField] RawImage portraitImage;
        Camera cam;
        Transform unit;
	
        void Awake ()
        {
                cam = gameObject.GetComponent<Camera>();
        }

	void LateUpdate () 
        {
                selections = MouseManager.Current.Selections;
               
                if (selections.Count == 0)
                {
                        if (unit != null)
                        {
                                unit.gameObject.layer = 0;
                                unit.GetChild(0).gameObject.layer = 0;
                        }

                        portraitImage.enabled = false;
                }
                else if (selections.Count == 1)
                {
                        if (unit != null)
                        {
                                unit.gameObject.layer = 0;
                                unit.GetChild(0).gameObject.layer = 0;
                        }

                        CameraPositioning();
                }
                else if (selections.Count > 1)
                {
                        unit = selections[0].transform;
                        CapsuleCollider collider = selections[0].GetComponent<CapsuleCollider>();
                        Player info = selections[0].gameObject.GetComponent<Player>();
                        selections[0].gameObject.layer = 8;
                        selections[0].transform.GetChild(0).gameObject.layer = 8;
                        portraitImage.enabled = true;
                        cam.backgroundColor = info.Info.AccentColor;
                        gameObject.transform.position = selections[0].transform.position + ((collider.center.y + collider.height) * Vector3.up) + (selections[0].transform.forward * (collider.height + collider.radius));
                        gameObject.transform.LookAt(selections[0].transform.position + (Vector3.up * collider.center.y));
                }
	}

        void CameraPositioning()
        {
                int randomUnit = Random.Range(0, selections.Count);
                unit = selections[randomUnit].transform;
                CapsuleCollider collider = selections[randomUnit].GetComponent<CapsuleCollider>();
                Player info = selections[randomUnit].gameObject.GetComponent<Player>();
                selections[randomUnit].gameObject.layer = 8;
                selections[randomUnit].transform.GetChild(0).gameObject.layer = 8; 
                portraitImage.enabled = true;
                cam.backgroundColor = info.Info.AccentColor;
                gameObject.transform.position = selections[randomUnit].transform.position + ((collider.center.y + collider.height) * Vector3.up) + (selections[randomUnit].transform.forward * (collider.height + collider.radius));
                gameObject.transform.LookAt(selections[randomUnit].transform.position + (Vector3.up * collider.center.y));
        }
}
