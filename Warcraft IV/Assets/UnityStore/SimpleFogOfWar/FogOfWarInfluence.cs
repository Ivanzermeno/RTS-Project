using UnityEngine;

namespace SimpleFogOfWar
{
    public class FogOfWarInfluence : MonoBehaviour
    {
        float viewDistance;
        [SerializeField] bool suspended;
		[SerializeField] Unit unitInfo;
		public Player player;

        void Start ()
        {
                viewDistance = unitInfo.VisionRange;
				
				if (player.Team != RTSManager.Current.players[0].Team)
                {
                        suspended = true;
                }

                FogOfWarSystem.RegisterInfluence(this);
        }

        void OnDestroy ()
        {
                FogOfWarSystem.UnregisterInfluence(this);
        }

        public float ViewDistance { get{ return viewDistance;} }

        public bool Suspended { get{ return suspended;} }
    }
}
