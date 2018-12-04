using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    class Map : MonoBehaviour
    {
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
        public Creature Creature;

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
                    float perlinValue = Mathf.PerlinNoise(xStart + x * Scale, yStart + y * Scale);
                    if (perlinValue > 0.3f)
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.FOREST);
                    }
                    else
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.OCEAN);
                    }
                    
                }
            }

            ChangeTile((int)(Width / 2), (int)(Height / 2), GameConstants.TILE_TYPE.CITY);

        }

        protected void RemoveTile(int x, int y)
        {
            tiles[x, y].OnTileClickEvent -= OnTileClick;
            Destroy(tiles[x, y].gameObject);
        }

        protected void ChangeTile(int x, int y, GameConstants.TILE_TYPE newType)
        {
            RemoveTile(x, y);
            AddTile(x, y, newType);
        }

        protected void AddTile(int x, int y, GameConstants.TILE_TYPE type)
        {
            Tile tile = Instantiate<Tile>(tile_prefabs[(int)type]);
            tile.transform.position = new Vector3(x, 0, y);
            tile.enabled = true;
            tile.OnTileClickEvent += OnTileClick;
            tiles[x, y] = tile;
        }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        public void OnTileClick(Tile tile)
        {
            Debug.Log("Player click on tile " + tile.Type.ToString() + " x: " + tile.transform.position.x + " y: " + tile.transform.position.z);
            if (Creature != null)
            {
                if (tile.Type != GameConstants.TILE_TYPE.OCEAN)
                {
                     Creature.Move(tile.transform.position.x, tile.transform.position.z);
                }
            }
            else
            {
                Debug.LogError("Creature not found.");
            }
        }
    }

}
