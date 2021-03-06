﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBlip : MonoBehaviour 
{
        GameObject blip;
		public Player player;

		void Start () 
    	{
                blip = GameObject.Instantiate(Map.Current.BlipPrefab);
                blip.transform.SetParent(Map.Current.transform);
                CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
                blip.transform.localScale = new Vector3(collider.radius + 2.0f, collider.radius + 2.0f, collider.radius + 2.0f);
                Image sprite = blip.GetComponent<Image>(); 
				sprite.color = player.AccentColor; 
		}
	
		void FixedUpdate () 
        {
                if (SimpleFogOfWar.FogOfWarSystem.Current.GetVisibility(gameObject.transform.position) == SimpleFogOfWar.FogOfWarSystem.FogVisibility.Visible)
                {
                        blip.SetActive(true);
                }
                else
                {
                        blip.SetActive(false);
                }
                blip.transform.position = Map.Current.WorldPositionToMap(transform.position);
		}

        void OnDestroy ()
        {
                GameObject.Destroy(blip);
        }
}
