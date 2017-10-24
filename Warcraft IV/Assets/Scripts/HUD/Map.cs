using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour 
{
        [SerializeField] RectTransform viewPort;
        [SerializeField] Transform corner1;
        [SerializeField] Transform corner2;
        [SerializeField] GameObject blipPrefab;
        public static Map Current;

        Vector2 terrainSize;
        RectTransform mapRect;

        public Map ()
        {
                Current = this;
        }

        void Awake ()
        {
                terrainSize = new Vector2(corner2.position.x - corner1.position.x, corner2.position.z - corner1.position.z);
                mapRect = GetComponent<RectTransform>();
        }

        public Vector2 WorldPositionToMap(Vector3 point)
        {
                Vector2 mapPos = new Vector2(point.x / terrainSize.x * mapRect.rect.width, point.z / terrainSize.y * mapRect.rect.height);
                return mapPos;
        }

        void Update ()
        {
                viewPort.position = WorldPositionToMap(Camera.main.transform.position);
        }

        public RectTransform ViewPort
        {
                get{ return viewPort; }
        }

        public Transform Corner1
        {
                get { return corner1; }
        }

        public Transform Corner2
        {
                get{ return corner2; }
        }

        public GameObject BlipPrefab
        {
                get{ return blipPrefab; }
        }
}
