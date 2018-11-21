using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    class Map : MonoBehaviour {

        /*
         * Variable propre à la ville pour identifier, les données de chaque case etc.... 
        */
        [Header("Map Generation")]
        public int Width;
        public int Height;
        public float Scale;
        public Tile[,] tiles;

        [Header("Prefabs")]
        public List<Tile> tile_prefabs;

	    // Use this for initialization
	    void Start ()
        {
            GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("EditorOnly");
            foreach (GameObject go in gameobjects)
            {
                Destroy(go);
            }

            tiles = new Tile[Width, Height];
            float xStart = Random.Range(0.0f, 10.0f);
            float yStart = Random.Range(0.0f, 10.0f);
		    for (int y = 0;  y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    //Creation de la map, affectation de données stat à chaque case
                    Tile tile;
                    float perlinValue = Mathf.PerlinNoise(xStart + x * Scale, yStart + y * Scale);
                    if (perlinValue > 0.5f)
                    {
                        tile = Instantiate<Tile>(tile_prefabs[0]);
                    }
                    else
                    {
                        tile = Instantiate<Tile>(tile_prefabs[1]);
                    }
                    tile.transform.position = new Vector3(x, 0, y);
                    tile.enabled = true;
                    tiles[x, y] = tile;
                }
            }
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }

}
