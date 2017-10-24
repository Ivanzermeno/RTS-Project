using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class ActionBehavior : MonoBehaviour 
{
        public abstract Action GetClickAction();

        public Sprite buttonPic;

        public int indexAction;
}
