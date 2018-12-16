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

            bool collectMatiereOrganique = CurrentTile != null && CurrentTile.matiereOrganique > 0.0f;
            if (collectMatiereOrganique)
            {
                GestionRoom orgaRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.ORGA];
                float collectedValue = Mathf.Clamp(orgaRoom.vitesseAbsorption * Time.deltaTime, 0.0f, CurrentTile.matiereOrganique);
                orgaRoom.AddResources(collectedValue);
                CurrentTile.matiereOrganique -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.ORGA].SetActive(collectMatiereOrganique);

            bool collectMineral = CurrentTile != null && CurrentTile.mineral > 0.0f;
            if (collectMineral)
            {
                GestionRoom mineralRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.MINERAL];
                float collectedValue = Mathf.Clamp(mineralRoom.vitesseAbsorption * Time.deltaTime, 0.0f, CurrentTile.mineral);
                mineralRoom.AddResources(collectedValue);
                CurrentTile.mineral -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.MINERAL].SetActive(collectMineral);

            bool collectMetal = CurrentTile != null && CurrentTile.metal > 0.0f;
            if (collectMetal)
            {
                GestionRoom metalRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.METAL];
                float collectedValue = Mathf.Clamp(metalRoom.vitesseAbsorption * Time.deltaTime, 0.0f, CurrentTile.metal);
                metalRoom.AddResources(collectedValue);
                CurrentTile.metal -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.METAL].SetActive(collectMetal);

            bool collectChimique = CurrentTile != null && CurrentTile.chimique > 0.0f;
            if (collectChimique)
            {
                GestionRoom chimicRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.CHIMIC];
                float collectedValue = Mathf.Clamp(chimicRoom.vitesseAbsorption * Time.deltaTime, 0.0f, CurrentTile.chimique);
                chimicRoom.AddResources(collectedValue);
                CurrentTile.chimique -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.CHIMIC].SetActive(collectChimique);

            bool collectPetrol = CurrentTile != null && CurrentTile.petrole > 0.0f;
            if (collectPetrol)
            {
                GestionRoom petrolRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.PETROL];
                float collectedValue = Mathf.Clamp(petrolRoom.vitesseAbsorption * Time.deltaTime, 0.0f, CurrentTile.petrole);
                petrolRoom.AddResources(collectedValue);
                CurrentTile.petrole -= collectedValue;
            }
            Ups[(int)GameConstants.GestionRoomType.PETROL].SetActive(collectPetrol);

            bool collectNuclear = CurrentTile != null && CurrentTile.nucleaire > 0.0f;
            if (collectNuclear)
            {
                GestionRoom nuclearRoom = Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.NUCLEAR];
                float collectedValue = Mathf.Clamp(nuclearRoom.vitesseAbsorption * Time.deltaTime, 0.0f, CurrentTile.nucleaire);
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
