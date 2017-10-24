using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelect : Interaction 
{
        public override void Deselect ()
        {
                ActionsManager.Current.ClearButtons();
        }

        public override void Select ()
        {
                ActionsManager.Current.ClearButtons();
                foreach (ActionBehavior ab in GetComponents<ActionBehavior>())
                {
                        ActionsManager.Current.AddButton(ab.buttonPic, ab.GetClickAction(), ab.indexAction);
                }
        }
	
}
