using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSManager : MonoBehaviour 
{
        public static RTSManager Current;

        [SerializeField] List<PlayerSetup> players = new List<PlayerSetup>();

        [SerializeField] TerrainCollider MapCollider;

        public RTSManager ()
        {
                Current = this;
        }

        void Awake ()
        {
                foreach (PlayerSetup playerSetup in players)
                {
                        int count = playerSetup.ActiveUnits.Count;
                        foreach (GameObject activeUnit in playerSetup.ActiveUnits)
                        {
                                Player player = activeUnit.AddComponent<Player>();
                                player.Info = playerSetup;
                        }
                        playerSetup.ActiveUnits.RemoveRange(0, count);

                        foreach (GameObject startingUnit in playerSetup.StartingUnits)
                        {
                                GameObject go = (GameObject)GameObject.Instantiate(startingUnit, playerSetup.Location.position, playerSetup.Location.rotation);
                                Player player = go.AddComponent<Player>();
                                player.Info = playerSetup;
                        }
                }
        }

        public List<PlayerSetup> Players
        {
                get{ return players;}
        }
}
