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
        public GameConstants.TILE_TYPE[,] creation_tiles;

        [Header("Prefabs")]
        public List<Tile> tile_prefabs;

        [Space(10)] // 10 pixels of spacing here.
        public Creature Creature;

        // Use this for initialization
        void Start()
        {
            GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("EditorOnly");
            foreach (GameObject go in gameobjects)
            {
                Destroy(go);
            }

            tiles = new Tile[Width, Height];
            creation_tiles = new GameConstants.TILE_TYPE[Width, Height];
            float xStart = Random.Range(0.0f, 10.0f);
            float yStart = Random.Range(0.0f, 10.0f);
            //base map
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    //Creation de la map, affectation de données stat à chaque case
                    float perlinValue = Mathf.PerlinNoise(xStart + x * Scale, yStart + y * Scale);
                    if (perlinValue > 0.75f)
                    {
                        creation_tiles[x, y] = GameConstants.TILE_TYPE.MOUNTAIN;
                    }
                    else if (perlinValue > 0.3f)
                    {
                        float rand = Random.value;
                        if (rand < 0.4f)
                        {
                            creation_tiles[x, y] = GameConstants.TILE_TYPE.PLAIN;
                        }
                        else
                        {
                            creation_tiles[x, y] = GameConstants.TILE_TYPE.FOREST;
                        }
                    }
                    else
                    {
                        creation_tiles[x, y] = GameConstants.TILE_TYPE.OCEAN;
                    }

                }
            }
            //city creation
            int cityNumber = 4 + Random.Range(1, 4);
            bool end;
            for (int i = 0; i < cityNumber; i++)
            {
                end = false;
                while (!end)
                {
                    int x = Random.Range(0, Width), y = Random.Range(0, Height);
                    if (creation_tiles[x, y] != GameConstants.TILE_TYPE.MOUNTAIN)
                    {
                        end = true;
                        creation_tiles[x, y] = GameConstants.TILE_TYPE.CITY;
                        bool end2 = false;
                        while (!end2)
                        {
                            int direction = Random.Range(0, 4);
                            if (direction == 0 && x != 0)
                            {
                                creation_tiles[x - 1, y] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }
                            else if (direction == 1 && y != Height - 1)
                            {
                                creation_tiles[x, y + 1] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }
                            else if (direction == 2 && x != Width - 1)
                            {
                                creation_tiles[x + 1, y] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }
                            else if (direction == 3 && y != 0)
                            {
                                creation_tiles[x, y - 1] = GameConstants.TILE_TYPE.FACTORY; end2 = true;
                            }

                        }

                    }
                }
            }

            // ajout d'une centrale nucléaire
            Vector2Int nuclearCenterLocation = GetNuclearCenterLocation(creation_tiles);
            creation_tiles[nuclearCenterLocation.x, nuclearCenterLocation.y] = GameConstants.TILE_TYPE.NUCLEAR;

            //creation of the objects
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    AddTile(x, y, creation_tiles[x, y]);
                }
            }


            // Monster spawn location
            Creature.transform.position = GetMonsterSpawnLocation();
        }

        Vector2Int GetNuclearCenterLocation(GameConstants.TILE_TYPE[,] creation_tiles)
        {
            bool isValidLocation = false;
            int x, y;
            bool hasOcean, hasGround;
            do
            {
                x = Random.Range(0, Width);
                y = Random.Range(0, Height);

                hasOcean = false;
                hasGround = false;

                List<Vector2Int> neighborsLocations = new List<Vector2Int>()
                {
                    new Vector2Int(x-1, y),
                    new Vector2Int(x+1, y),
                    new Vector2Int(x, y-1),
                    new Vector2Int(x, y+1)
                };

                foreach (Vector2Int neighborLocation in neighborsLocations)
                {
                    if (neighborLocation.x >= 0 && neighborLocation.x < creation_tiles.GetLength(0)
                        && neighborLocation.y >= 0 && neighborLocation.y < creation_tiles.GetLength(1))
                    {
                        if (creation_tiles[neighborLocation.x, neighborLocation.y] == GameConstants.TILE_TYPE.OCEAN)
                        {
                            hasOcean = true;
                        }

                        if (creation_tiles[neighborLocation.x, neighborLocation.y] == GameConstants.TILE_TYPE.FOREST ||
                            creation_tiles[neighborLocation.x, neighborLocation.y] == GameConstants.TILE_TYPE.PLAIN)
                        {
                            hasGround = true;
                        }
                    }
                }

                isValidLocation = hasOcean && hasGround;

            } while (!isValidLocation);
            return new Vector2Int(x, y);
        }

        Vector3 GetMonsterSpawnLocation()
        {
            List<Vector2Int> closedLocations = new List<Vector2Int>();
            List<Vector2Int> openLocations = new List<Vector2Int>();
            Vector2Int start = new Vector2Int(0, 0);
            openLocations.Add(start);

            while (openLocations.Count > 0)
            {
                Vector2Int location = openLocations[0];
                openLocations.RemoveAt(0);

                closedLocations.Add(location);

                if (tiles[location.x, location.y].IsWalkable)
                {
                    return new Vector3(location.x, Creature.transform.position.y, location.y);
                }

                if (location.x - 1 >= 0)
                {
                    Vector2Int newLocation = new Vector2Int(location.x - 1, location.y);
                    if (!closedLocations.Contains(newLocation))
                    {
                        openLocations.Add(newLocation);
                    }
                }

                if (location.x + 1 < this.Width)
                {
                    Vector2Int newLocation = new Vector2Int(location.x + 1, location.y);
                    if (!closedLocations.Contains(newLocation))
                    {
                        openLocations.Add(newLocation);
                    }
                }

                if (location.y - 1 >= 0)
                {
                    Vector2Int newLocation = new Vector2Int(location.x, location.y - 1);
                    if (!closedLocations.Contains(newLocation))
                    {
                        openLocations.Add(newLocation);
                    }
                }

                if (location.y + 1 < this.Height)
                {
                    Vector2Int newLocation = new Vector2Int(location.x, location.y + 1);
                    if (!closedLocations.Contains(newLocation))
                    {
                        openLocations.Add(newLocation);
                    }
                }
            }
            Debug.LogError("No spawn location found.");
            return Vector3.zero;
        }

        protected void RemoveTile(int x, int y)
        {
            tiles[x, y].OnTileClickEvent -= OnTileClick;
            Destroy(tiles[x, y].gameObject);
        }

        public void ChangeTile(int x, int y, GameConstants.TILE_TYPE newType)
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
        void Update()
        {

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
