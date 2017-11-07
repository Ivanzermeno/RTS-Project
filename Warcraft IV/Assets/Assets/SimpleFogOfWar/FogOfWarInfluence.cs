using UnityEngine;

namespace SimpleFogOfWar
{
    public class FogOfWarInfluence : MonoBehaviour
    {
        float viewDistance;
        [SerializeField] bool suspended;

        void Start ()
        {
                Unit unit = gameObject.GetComponent<Unit>();
                viewDistance = unit.VisionRange;

                Player player = gameObject.GetComponent<Player>();
                if (player.Info.Team != RTSManager.Current.Players[0].Team)
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
