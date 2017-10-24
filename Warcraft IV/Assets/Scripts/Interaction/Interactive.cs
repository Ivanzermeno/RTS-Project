using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour 
{
        bool selected = false;
        public bool Selected
        {
                get { return selected; }
        }
                
        public void Select ()
        {
                selected = true;
                foreach (Interaction selection in GetComponents<Interaction>())
                {
                        selection.Select();
                }
        }

        public void Deselect ()
        {
                selected = false;
                foreach (Interaction selection in GetComponents<Interaction>())
                {
                        selection.Deselect();
                }
        }
}
