﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public ParticleSystem ExplosionPrefab;
        public Transform ExplosionPosition;

        [Space(10)]
        public List<GameObject> Ups;

        Coroutine FollowPathCoroutine;
        Coroutine OpenTabCoroutine;

        public RectTransform EnergyBar;
        public int EnergyBarHeight;

        private Coroutine AttackTileCoroutine;

        private void Awake()
        {
            AStar = new AStar();
            AttackTileCoroutine = null;
        }

        void Start()
        {
            CurrentTile = Map.tiles[(int)transform.position.x, (int)transform.position.z];
            Colonie.Instance.MapUI.SetActive(true);
            Colonie.Instance.ColonieUI.SetActive(false);

            CameraController.Instance.DisplayBubble("Clique ici pour voir ta colonie", TooltipBubble.TipPosition.RIGHT, EnergyBar.transform.position.x - 240.0f, EnergyBar.transform.position.y + 100.0f);
        }

        void Update()
        {
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

            if (EnergyBar != null)
            {
                EnergyBar.sizeDelta = new Vector2(EnergyBar.sizeDelta.x, EnergyBarHeight * (Colonie.Instance.energie / Colonie.Instance.energieMax));
            }

            // Attaque/Défense
            if (CurrentTile && Map.IsAttackable(CurrentTile))
            {
                if (AttackTileCoroutine == null)
                {
                    AttackTileCoroutine = StartCoroutine(AttackTile());
                }
            }
            else if (AttackTileCoroutine != null)
            {
                StopCoroutine(AttackTileCoroutine);
                AttackTileCoroutine = null;
            }
        }

        void MakeExplosion()
        {
            ParticleSystem explosion = Instantiate<ParticleSystem>(ExplosionPrefab);
            explosion.transform.position = ExplosionPosition.position;
            explosion.transform.localScale = ExplosionPosition.localScale;
            explosion.transform.SetParent(transform);
            explosion.gameObject.layer = LayerMask.NameToLayer("Creature");
            explosion.Emit(1);
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        IEnumerator AttackTile()
        {
            bool yourTurn = Random.Range(0, 2) == 1;
            yield return new WaitForSeconds(1.0f);
            while (true)
            {
                if (yourTurn)
                {
                    Animator.SetTrigger("Attack");
                }
                else
                {
                    MakeExplosion();
                    Animator.SetTrigger("Hit");
                }

                yourTurn = !yourTurn; // c'est au tour de l'autre
                yield return new WaitForSeconds(3.0f + Random.Range(0, 0.5f));
            }
        }

        public void Move(float x, float y)
        {
            if (FollowPathCoroutine != null)
            {
                StopCoroutine(FollowPathCoroutine);
                FollowPathCoroutine = null;
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
            else
            {
                Vector3 tooltipPosition = Camera.main.WorldToScreenPoint(this.transform.position);
                CameraController.Instance.DisplayBubble("Parfois la génération procédurale n'est pas parfaite, tu ne peux pas accéder à cet endroit, tu peux redémarrer une nouvelle partie, tu auras plus de chance la prochaine fois !", TooltipBubble.TipPosition.TOP, tooltipPosition.x, tooltipPosition.y - 100.0f, 720.0f);
                Animator.SetBool("IsWalking", false);
            }

            watch.Stop();
            Debug.Log("Path found in " + watch.ElapsedMilliseconds + " millisecond(s).");
            
        }

        public void OnPreviewClick()
        {
            if (OpenTabCoroutine != null)
            {
                StopCoroutine(OpenTabCoroutine);
                OpenTabCoroutine = null;
                CameraController.Instance.StartFollowCreature();
            }
            else
            {

                OpenTabCoroutine = StartCoroutine(OpenTab());
            }
        }

        public Tile GetCurrentTile()
        {
            return Map.tiles[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z)]; 
        }

        IEnumerator OpenTab()
        {
            yield return new WaitForSeconds(0.250f);
            Map.Instance.SwitchTab();
            OpenTabCoroutine = null;
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
