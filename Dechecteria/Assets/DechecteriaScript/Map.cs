using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    /*
     * Variable propre à la ville pour identifier, les données de chaque case etc.... 
    */
    public int W = 1024;
    public int H = 768;
    public CaseTerrain[,] mapTerrain;
	// Use this for initialization
	void Start () {
		for (int i = 0;  i < W; i++)
        {
            for (int j = 0; j < H; j++)
            {
                //Creation de la map, affectation de données stat à chaque case
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
