using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSetup
{
        [SerializeField] string name;
        [SerializeField] Factions faction;
        [SerializeField] Transform location;
        [SerializeField] Color accentColor;
        [SerializeField] List<GameObject> startingUnits = new List<GameObject>();
        [SerializeField] List<GameObject> activeUnits = new List<GameObject>();
        [SerializeField] bool isAI;
        [SerializeField] [Range(0, 8)] int team;
        [SerializeField] float gold;
        [SerializeField] float lumber;
        [SerializeField] float population;
        [SerializeField] float maxPopulation;

        public enum Factions
        {
                Alliance,
                Horde,
                Neutral
        };

        public PlayerSetup()
        {
                name = "Neutral";
                accentColor = Color.yellow;
                faction = Factions.Neutral;
                team = 0;
        }

        public string Name
        {
                get{ return name; }
                set{ name = value; }
        }

        public Factions Faction
        {
                get { return faction; }
        }

        public Transform Location
        {
                get { return location; }
        }

        public Color AccentColor
        {
                get{ return accentColor; }
                set{ accentColor = value; }
        }

        public List<GameObject> StartingUnits
        {
                get{ return startingUnits; }
        }

        public List<GameObject> ActiveUnits
        {
                get{ return activeUnits; }
        }

        public bool IsAI
        {
                get{ return isAI; }
        }

        public int Team
        {
                get{ return team; }
        }

        public float Gold
        {
                get{ return gold; }
                set{ gold += value; }
        }

        public float Lumber
        {
                get{ return lumber; }
                set{ lumber += value; }
        }

        public float Population
        {
                get{ return population; }
                set
                { 
                        if (population < maxPopulation)
                        {
                                if ((population + value) > maxPopulation)
                                {
                                        population += value;
                                } 
                        }
                }
        }
}
