using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
        [SerializeField] KeyCode panForward;
        [SerializeField] KeyCode panBackward;
        [SerializeField] KeyCode panLeft;
        [SerializeField] KeyCode panRight;
        [SerializeField] float panSpeed;
        [SerializeField] int panBorderThickness;

        void Update () 
        {
                if (Input.GetKey(panForward) || Input.mousePosition.y >= Screen.height - panBorderThickness)
                {
                        gameObject.transform.position += Vector3.forward * Time.deltaTime * panSpeed;
                }
                else if (Input.GetKey(panBackward) || Input.mousePosition.y <= panBorderThickness)
                {
                        gameObject.transform.position += Vector3.back * Time.deltaTime * panSpeed;
                }
                if (Input.GetKey(panLeft) || Input.mousePosition.x <= panBorderThickness)
                {
                        gameObject.transform.position += Vector3.left * Time.deltaTime * panSpeed;
                }
                else if (Input.GetKey(panRight) || Input.mousePosition.x >= Screen.width - panBorderThickness)
                {
                        gameObject.transform.position += Vector3.right * Time.deltaTime * panSpeed;
                }
	}
}
