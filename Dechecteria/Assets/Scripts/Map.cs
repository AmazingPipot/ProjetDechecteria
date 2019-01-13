using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dechecteria
{
    class Map : MonoBehaviour
    {
        public GameObject Source;
        public AudioClip clip1;
        public AudioClip clip2;
        public AudioSource son1;
        public AudioSource son2;
        AudioSource son3;
        Creature posTile;
        public static Map Instance;
        /*
         * Variable propre à la ville pour identifier, les données de chaque case etc.... 
        */
        [Header("Map Generation")]
        public int Width;
        public int Height;
        public float Scale;
        public Tile[,] tiles;
        public GameConstants.TILE_TYPE[,] creation_tiles;
        public Transform TilesContainer;

        [Header("Planes")]
        public int MaxPlanes;
        public List<GameObject> Planes;
        public GameObject PlanePrefab;
        public Transform PlanesContainer;
        public float PlaneAltitude;

        [Header("Prefabs")]
        public List<Tile> tile_prefabs;
        
        [Space(10)] // 10 pixels of spacing here.
        public Creature Creature;

        private Vector3 MapCenter;

        public ParticleSystem MapPointerClick;
        [SerializeField]
        private int SwitchClicksCount;
        private bool firstTimeMove;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
                Debug.LogError("Une seule instance de Map est requise.");
            }

            SwitchClicksCount = 0;
            firstTimeMove = true;
        }

        void Start()
        {
            //son1 = GetComponent<AudioSource>();
            //son2 = GetComponent<AudioSource>();
            son1.clip = clip1;
            son2.clip = clip2;
            son3 = Source.GetComponent<AudioSource>();
            son1.Play();
            son2.Play();
            posTile = Creature.GetComponent<Creature>();

            GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("EditorOnly");
            foreach (GameObject go in gameobjects)
            {
                Destroy(go);
            }

            tiles = new Tile[Width, Height];
            creation_tiles = new GameConstants.TILE_TYPE[Width, Height];
            MapCenter = new Vector3(Width / 2.0f, 0.0f, Height / 2.0f);
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

                if (tiles[location.x, location.y].IsWalkable
                    && tiles[location.x, location.y].Type != GameConstants.TILE_TYPE.FACTORY
                    && tiles[location.x, location.y].Type != GameConstants.TILE_TYPE.NUCLEAR
                    && tiles[location.x, location.y].Type != GameConstants.TILE_TYPE.CITY)
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
            TilesContainer.transform.position = Vector3.zero;
            if (tile_prefabs[(int)type])
            {
                Tile tile = Instantiate<Tile>(tile_prefabs[(int)type], TilesContainer);
                tile.transform.position = new Vector3(x, 0, y);
                tile.enabled = true;
                tile.gameObject.name = x + " " + y + " " + type.ToString();
                tile.OnTileClickEvent += OnTileClick;
                tiles[x, y] = tile;
            }
        }

        public static bool IsAttackable(Tile tile)
        {
            return tile.Type.Equals(GameConstants.TILE_TYPE.CITY)
                || tile.Type.Equals(GameConstants.TILE_TYPE.FACTORY)
                || tile.Type.Equals(GameConstants.TILE_TYPE.NUCLEAR);
        }

        IEnumerator SpawnPlane()
        {
            GameObject newPlane = Instantiate(PlanePrefab, PlanesContainer);
            newPlane.SetActive(false);
            Planes.Add(newPlane);
            yield return new WaitForSeconds(Random.Range(0.0f, 15.0f));
            newPlane.SetActive(true);
            float distanceMax = Mathf.Max(Width, Height) * 2.0f;
            float angle = Random.Range(0.0f, 360.0f);
            float altitude = PlaneAltitude + Random.Range(-0.5f, 0.5f);
            newPlane.transform.position = new Vector3(MapCenter.x + Mathf.Cos(angle * Mathf.Deg2Rad) * (distanceMax * 0.98f), altitude, MapCenter.z + Mathf.Sin(angle * Mathf.Deg2Rad) * (distanceMax * 0.98f));
            Vector3 dir = new Vector3(MapCenter.x, altitude, MapCenter.z);
            newPlane.transform.LookAt(dir);
            newPlane.transform.Rotate(0.0f, Random.Range(-25.0f, 25.0f), 0.0f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Escape) && Colonie.Instance.ColonieUI.activeInHierarchy))
            {
                SwitchTab();
            }
            if (Creature != null)
            {
                if (posTile != null)
                {
                    if (posTile.CurrentTile != null)
                    {
                        if (IsAttackable(posTile.CurrentTile))
                        {
                            if (son2.volume < 0.5f)
                            {
                                son2.volume += 0.02f;
                            }
                            else
                                son2.volume = 0.5f;
                        }
                        else
                        {
                            if (son2.volume > 0.0f)
                            {
                                son2.volume -= 0.02f;
                            }
                            else
                                son2.volume = 0.0f;
                        }
                    }
                    else
                    {
                        son2.volume = 0.0f;
                    }
                }
                else
                {
                    posTile = Creature.GetComponent<Creature>();
                }
            }
            if (Colonie.Instance.ColonieUI.activeInHierarchy && !son3.isPlaying && son2.volume != 0.5f)
            {
                if (son1.volume < 0.5f)
                {
                    son1.volume += 0.01f;
                }
                else
                    son1.volume = 0.5f;
            }
            else
            {
                if (son1.volume > 0.2f)
                {
                    son1.volume -= 0.01f;
                }
                else
                    son1.volume = 0.2f;
            }

            int totalPopulation = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (IsAttackable(tiles[x, y]) && tiles[x, y].population > 0)
                    {
                        totalPopulation += Mathf.FloorToInt(tiles[x, y].population);
                    }
                }
            }

            if (totalPopulation == 0)
            {
                StartCoroutine(ChangeToVictoryScene());
            }

        }

        IEnumerator ChangeToVictoryScene()
        {
            yield return new WaitForSeconds(3.0f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SceneVictoire");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        void FixedUpdate()
        {
            while (Planes.Count < MaxPlanes)
            {
                StartCoroutine(SpawnPlane());
            }

            float distanceMax = Mathf.Max(Width, Height) * 2.0f;
            foreach (GameObject plane in Planes.ToArray())
            {
                if (Mathf.Pow(plane.transform.position.x - MapCenter.x, 2) + Mathf.Pow(plane.transform.position.z - MapCenter.z, 2) > distanceMax * distanceMax)
                {
                    Planes.Remove(plane);
                    Destroy(plane);
                }
            }
        }

        public void OnTileClick(Tile tile, Vector3 worldPosition)
        {
            Debug.Log("Player click on tile " + tile.Type.ToString() + " x: " + tile.transform.position.x + " y: " + tile.transform.position.z);
            if (Creature != null)
            {
                if (SwitchClicksCount > 0)
                {
                    ParticleSystem particleSystem = Instantiate<ParticleSystem>(MapPointerClick);
                    particleSystem.transform.position = new Vector3(tile.transform.position.x, 0.1f, tile.transform.position.z);
                    Destroy(particleSystem.gameObject, 2.0f);
                    if (tile.IsWalkable)
                    {
                        Creature.Move(tile.transform.position.x, tile.transform.position.z);
                        if (firstTimeMove)
                        {
                            firstTimeMove = false;
                            CameraController.Instance.DisplayBubble("Clique deux fois de suite ici pour suivre les mouvements de ta créature", TooltipBubble.TipPosition.RIGHT, Creature.EnergyBar.transform.position.x - 240.0f, Creature.EnergyBar.transform.position.y + 100.0f, 420.0f);
                        }
                    }
                    else
                    {
                        var mainModule = particleSystem.main;
                        mainModule.startColor = Color.red;
                    }
                }
            }
            else
            {
                Debug.LogError("Creature not found.");
            }
        }

        public void SwitchTab()
        {
            SwitchClicksCount++;
            Colonie.Instance.ColonieUI.SetActive(!Colonie.Instance.ColonieUI.activeInHierarchy);
            Colonie.Instance.MapUI.SetActive(!Colonie.Instance.MapUI.activeInHierarchy);

            
            if (SwitchClicksCount == 1)
            {
                
                CameraController.Instance.DisplayBubble("Clique ici pour retourner sur la carte à tout moment", TooltipBubble.TipPosition.RIGHT, Creature.EnergyBar.transform.position.x - 240.0f, Creature.EnergyBar.transform.position.y + 100.0f, 420.0f);
                CameraController.Instance.DisplayBubble("Clique sur Réserves pour créer une réserve", TooltipBubble.TipPosition.LEFT, InterfaceColonie.Instance.BoutonPoches.transform.position.x + 190.0f, InterfaceColonie.Instance.BoutonPoches.transform.position.y, 500.0f);
            }
            else if (SwitchClicksCount == 2)
            {
                
                Tile tile = null;
                // on regarde la case à droite
                Vector2Int creaturePos = new Vector2Int(Mathf.RoundToInt(Creature.transform.position.x), Mathf.RoundToInt(Creature.transform.position.z));
                if (tiles[creaturePos.x + 1, creaturePos.y].IsWalkable && !IsAttackable(tiles[creaturePos.x + 1, creaturePos.y]))
                {
                    tile = tiles[creaturePos.x + 1, creaturePos.y];
                }
                // on regarde la case au dessus
                else if (tiles[creaturePos.x, creaturePos.y + 1].IsWalkable && !IsAttackable(tiles[creaturePos.x, creaturePos.y + 1]))
                {
                    tile = tiles[creaturePos.x, creaturePos.y + 1];
                }
                if (tile != null)
                {
                    Debug.Log(tile.transform.position);
                    Debug.Log(Camera.main.name);
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(tile.transform.position);
                    Debug.Log(screenPosition);
                    CameraController.Instance.DisplayBubble("Clique droit sur cette case pour faire avancer ta créature", TooltipBubble.TipPosition.BOTTOM_LEFT, screenPosition.x, screenPosition.y + 80.0f, 450.0f);
                }
                
            }
        }
    }

}
