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
            GameConstants.TILE_TYPE[,] creation_tile = new GameConstants.TILE_TYPE[Width, Height];
            float xStart = Random.Range(0.0f, 10.0f);
            float yStart = Random.Range(0.0f, 10.0f);
            //base map
		    for (int y = 0;  y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    //Creation de la map, affectation de données stat à chaque case
                    float perlinValue = Mathf.PerlinNoise(xStart + x * Scale, yStart + y * Scale);
                    if (perlinValue > 0.7f){
                        creation_tile[x,y]=GameConstants.TILE_TYPE.MOUNTAIN;
                    }
                    else if (perlinValue > 0.3f)
                    {
                        creation_tile[x, y] = GameConstants.TILE_TYPE.FOREST;
                    }
                    else
                    {
                        creation_tile[x, y] = GameConstants.TILE_TYPE.OCEAN;
                    }
                    
                }
            }
            //city creation
            int cityNumber = 4 + Random.Range(1,4);
            bool end;
            for(int i = 0; i < cityNumber; i++)
            {
                end = false;
                while (!end)
                {
                    int x = Random.Range(0, Width), y = Random.Range(0, Height);
                    if(creation_tile[x,y] != GameConstants.TILE_TYPE.MOUNTAIN)
                    {
                        end = true;
                        creation_tile[x, y] = GameConstants.TILE_TYPE.CITY;
                        bool end2 = false;
                        while (!end2)
                        {
                            int direction = Random.Range(0, 4);
                            if(direction==0 && x != 0)
                            {
                                creation_tile[x - 1, y] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }
                            else if (direction == 1 && y != Height - 1)
                            {
                                creation_tile[x, y + 1] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }
                            else if (direction == 2 && x != Width - 1)
                            {
                                creation_tile[x + 1, y] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }
                            else if (direction == 3 && y != 0)
                            {
                                creation_tile[x, y - 1] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }

                        }
                        
                    }
                }
            }

            //creation of the objects
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (creation_tile[x,y] == GameConstants.TILE_TYPE.MOUNTAIN)
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.MOUNTAIN);
                    }
                    else if (creation_tile[x, y] == GameConstants.TILE_TYPE.FOREST)
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.FOREST);
                    }
                    else if(creation_tile[x, y] == GameConstants.TILE_TYPE.CITY)
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.CITY);
                    }
                    else if (creation_tile[x, y] == GameConstants.TILE_TYPE.FACTORY)
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.FACTORY);
                    }
                    else
                    {
                        AddTile(x, y, GameConstants.TILE_TYPE.OCEAN);
                    }

                }
            }


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
            tile.gameObject.name = x + " " + y + " " + type.ToString();
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
                if (tile.IsWalkable)
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
