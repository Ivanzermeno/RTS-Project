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
        int randomUnit;
		public Color color;
	
        void Awake ()
        {
                cam = gameObject.GetComponent<Camera>();
        }

		void LateUpdate () 
        {
                selections = MouseManager.Current.Selections;
               
                if (selections.Count == 0)
                {
                        UnitLayering();

                        portraitImage.enabled = false;
                }
                else if (selections.Count == 1)
                {
                        UnitLayering();

                        CameraSetting();

                        if (portraitImage.enabled == true)
                        {
                                CameraPositioning();
                        }
                }
                else if (selections.Count > 1 && portraitImage.enabled == false)
                {
                        CameraSetting();
                }
                else
                {
                        CameraPositioning();
                }
	}

        void CameraPositioning()
        {
                CapsuleCollider collider = selections[randomUnit].GetComponent<CapsuleCollider>();
                gameObject.transform.position = selections[randomUnit].transform.position + ((collider.center.y + collider.height) * Vector3.up) + (selections[randomUnit].transform.forward * (collider.height + collider.radius));
                gameObject.transform.LookAt(selections[randomUnit].transform.position + (Vector3.up * collider.center.y));
        }

        void CameraSetting()
        {
                randomUnit = Random.Range(0, selections.Count);
				color = selections [randomUnit].GetComponent<Highlight>().player.AccentColor;
                unit = selections[randomUnit].transform;
                selections[randomUnit].gameObject.layer = 8;
                selections[randomUnit].transform.GetChild(0).gameObject.layer = 8; 
                portraitImage.enabled = true;
				cam.backgroundColor = color;
        }

        void UnitLayering()
        {
                if (unit != null)
                {
                        unit.gameObject.layer = 0;
                        unit.GetChild(0).gameObject.layer = 0;
                }
        }
}
