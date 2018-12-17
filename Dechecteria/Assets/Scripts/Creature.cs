using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dechecteria
{
    class Creature : MonoBehaviour
    {

        public Map Map;
        public Animator Animator;
        public Tile CurrentTile;

        private AStar AStar;
        public List<Vector2> Path;

        public float Speed;
        public float SpeedRotation;

        [Space(10)]
        public List<GameObject> Ups;

        Coroutine FollowPathCoroutine;

        private void Awake()
        {
            AStar = new AStar();
        }

        void Start()
        {
            CurrentTile = Map.tiles[(int)transform.position.x, (int)transform.position.z];
            Colonie.Instance.MapUI.SetActive(true);
            Colonie.Instance.ColonieUI.SetActive(false);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                Colonie.Instance.ColonieUI.SetActive(!Colonie.Instance.ColonieUI.activeInHierarchy);
                Colonie.Instance.MapUI.SetActive(!Colonie.Instance.MapUI.activeInHierarchy);
            }

            GestionRoom orgaRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.ORGA];
            bool collectMatiereOrganique = CurrentTile != null && CurrentTile.matiereOrganique > 0.0f && orgaRoom.Level > 0;
            if (collectMatiereOrganique)
            {
                float collectedValue = Mathf.Clamp(orgaRoom.vitesseAbsorption * Time.deltaTime, 0.0f, Mathf.Min(CurrentTile.matiereOrganique, orgaRoom.MaxCapacity - orgaRoom.GetResourcesf()));
                orgaRoom.AddResources(collectedValue);
                CurrentTile.matiereOrganique -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.ORGA].SetActive(collectMatiereOrganique);

            GestionRoom mineralRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.MINERAL];
            bool collectMineral = CurrentTile != null && CurrentTile.mineral > 0.0f && mineralRoom.Level > 0;
            if (collectMineral)
            {
                float collectedValue = Mathf.Clamp(mineralRoom.vitesseAbsorption * Time.deltaTime, 0.0f, Mathf.Min(CurrentTile.mineral, mineralRoom.MaxCapacity - mineralRoom.GetResourcesf()));
                mineralRoom.AddResources(collectedValue);
                CurrentTile.mineral -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.MINERAL].SetActive(collectMineral);

            GestionRoom metalRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.METAL];
            bool collectMetal = CurrentTile != null && CurrentTile.metal > 0.0f && metalRoom.Level > 0;
            if (collectMetal)
            {
                float collectedValue = Mathf.Clamp(metalRoom.vitesseAbsorption * Time.deltaTime, 0.0f, Mathf.Min(CurrentTile.metal, metalRoom.MaxCapacity - metalRoom.GetResourcesf()));
                metalRoom.AddResources(collectedValue);
                CurrentTile.metal -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.METAL].SetActive(collectMetal);

            GestionRoom chimicRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.CHIMIC];
            bool collectChimique = CurrentTile != null && CurrentTile.chimique > 0.0f && chimicRoom.Level > 0;
            if (collectChimique)
            {
                float collectedValue = Mathf.Clamp(chimicRoom.vitesseAbsorption * Time.deltaTime, 0.0f, Mathf.Min(CurrentTile.chimique, chimicRoom.MaxCapacity - chimicRoom.GetResourcesf()));
                chimicRoom.AddResources(collectedValue);
                CurrentTile.chimique -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.CHIMIC].SetActive(collectChimique);

            GestionRoom petrolRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.PETROL];
            bool collectPetrol = CurrentTile != null && CurrentTile.petrole > 0.0f && petrolRoom.Level > 0;
            if (collectPetrol)
            {
                float collectedValue = Mathf.Clamp(petrolRoom.vitesseAbsorption * Time.deltaTime, 0.0f, Mathf.Min(CurrentTile.petrole, petrolRoom.MaxCapacity - petrolRoom.GetResourcesf()));
                petrolRoom.AddResources(collectedValue);
                CurrentTile.petrole -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.PETROL].SetActive(collectPetrol);

            GestionRoom nuclearRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.NUCLEAR];
            bool collectNuclear = CurrentTile != null && CurrentTile.nucleaire > 0.0f && nuclearRoom.Level > 0;
            if (collectNuclear)
            {
                float collectedValue = Mathf.Clamp(nuclearRoom.vitesseAbsorption * Time.deltaTime, 0.0f, Mathf.Min(CurrentTile.nucleaire, nuclearRoom.MaxCapacity - nuclearRoom.GetResourcesf()));
                nuclearRoom.AddResources(collectedValue);
                CurrentTile.nucleaire -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.NUCLEAR].SetActive(collectNuclear);

        }

        public void Move(float x, float y)
        {
            if (FollowPathCoroutine != null)
            {
                StopCoroutine(FollowPathCoroutine);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool[,] walkableTiles = new bool[Map.Width, Map.Height];
            for (int mx = 0; mx < Map.Width; mx++)
            {
                for (int my = 0; my < Map.Height; my++)
                {
                    walkableTiles[mx, my] = Map.tiles[mx, my].IsWalkable;
                }
            }

            if (AStar.FindPath(new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.z)), new Vector2(x, y), walkableTiles))
            {
                Path = AStar.Path;
                Animator.SetBool("IsWalking", true);
                CurrentTile = null;
                FollowPathCoroutine = StartCoroutine(FollowPath());
            }

            watch.Stop();
            Debug.Log("Path found in " + watch.ElapsedMilliseconds + " millisecond(s).");
            
        }

        Tile GetCurrentTile()
        {
            return Map.tiles[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z)]; 
        }

        IEnumerator FollowPath()
        {
            int currWaypoint = 0;
            while (currWaypoint < Path.Count)
            {
                Vector3 start = transform.position;
                Vector3 end = new Vector3(Path[currWaypoint].x, transform.position.y, Path[currWaypoint].y);
                Vector3 velocity = end - transform.position;
                velocity.Normalize();

                var rotation = Quaternion.LookRotation(velocity);

                while (Mathf.Pow(transform.position.x - end.x, 2) + Mathf.Pow(transform.position.z - end.z, 2) >= 0.10f * 0.10f)
                {
                    transform.position += velocity * Time.deltaTime * Speed;
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * SpeedRotation);
                    yield return new WaitForEndOfFrame();
                }
                currWaypoint++;
            }

            CurrentTile = GetCurrentTile();
            Animator.SetBool("IsWalking", false);
        }
    }
}
