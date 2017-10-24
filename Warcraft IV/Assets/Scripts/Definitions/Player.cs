using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
        [SerializeField] private PlayerSetup info;

        public static PlayerSetup Default;

        void Start ()
        {
                AddActiveUnit();
        }

        public PlayerSetup Info
        {
                get{ return info; }
                set{ info = value; }
        }

        public void AddActiveUnit ()
        {
                info.ActiveUnits.Add(this.gameObject);
        }

        void OnDestroy ()
        {
                info.ActiveUnits.Remove(this.gameObject);
        }
}
