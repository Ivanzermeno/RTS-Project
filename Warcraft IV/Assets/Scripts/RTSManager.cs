using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSManager : MonoBehaviour 
{
	public static RTSManager Current;

	public List<Player> players = new List<Player>();

	public RTSManager()
	{
		Current = this;
	}

	void OnEnable()
	{
		
	}

	void Awake() 
	{
		foreach (Player player in players)
		{
			foreach (GameObject startingUnit in player.StartingUnits)
			{
				GameObject go = (GameObject)GameObject.Instantiate (startingUnit, player.Location.position, player.Location.rotation);
			}
		}
	}
}
