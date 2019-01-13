using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public AudioClip ExplosionAudioClip;

        public AudioClip FootStepAudioClip;
        public float FootstepInterval = 0.75f;

        [Space(10)]
        public List<GameObject> Ups;

        Coroutine FollowPathCoroutine;
        Coroutine OpenTabCoroutine;
        Coroutine GameOverCoroutine;

        public RectTransform EnergyBar;
        public int EnergyBarHeight;

        private Coroutine AttackTileCoroutine;

        private void Awake()
        {
            AStar = new AStar();
            AttackTileCoroutine = null;
            GameOverCoroutine = null;
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
            if (CurrentTile && Map.IsAttackable(CurrentTile) && GameOverCoroutine == null)
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

            if (Colonie.Instance.energie <= 0.0f)
            {
                if (GameOverCoroutine == null)
                {
                    StopAllCoroutines();
                    GameOverCoroutine = StartCoroutine(GameOver());
                }
            }
        }

        public static void PlaySound(AudioClip clip)
        {
            GameObject go = new GameObject();
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            go.name = System.DateTime.Now.ToString();
            GameObject.Destroy(go, audioSource.clip.length + 0.1f);
        }

        IEnumerator GameOver()
        {
            CameraController.Instance.MoveCameraToCreature();
            yield return new WaitForSeconds(1.0f);
            Animator.SetTrigger("Die");
            yield return new WaitForSeconds(5.0f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameOver");
            while (!asyncLoad.isDone)
            {
                yield return null;
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
            PlaySound(ExplosionAudioClip);
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        IEnumerator AttackTile()
        {
            bool yourTurn = Random.Range(0, 2) == 1;
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                if (CurrentTile.population <= 0.0f)
                {
                    AttackTileCoroutine = null;
                    break;
                }

                if (yourTurn)
                {
                    Animator.SetTrigger("Attack");
                    CurrentTile.population = Mathf.Clamp(CurrentTile.population - Colonie.Instance.listCapaciteCreature[(int)GameConstants.CapaciteCreature.ATK] * Random.Range(750.0f, 1000.0f), 0.0f, CurrentTile.population);
                }
                else
                {
                    MakeExplosion();
                    Animator.SetTrigger("Hit");
                    Colonie.Instance.energie = Mathf.Clamp(Colonie.Instance.energie - (CurrentTile.attaque - Colonie.Instance.listCapaciteCreature[(int)GameConstants.CapaciteCreature.DEF]) * 100.0f, 0.0f, Colonie.Instance.energie);
                }

                

                yourTurn = !yourTurn; // c'est au tour de l'autre
                yield return new WaitForSeconds(3.0f + Random.Range(0, 1.0f));
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
            
            float timeBeforefootstep = FootstepInterval;
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

                    timeBeforefootstep -= Time.deltaTime;
                    if (timeBeforefootstep <= 0.0f)
                    {
                        PlaySound(FootStepAudioClip);
                        timeBeforefootstep = FootstepInterval;
                    }

                    yield return new WaitForEndOfFrame();
                }
                currWaypoint++;
            }

            CurrentTile = GetCurrentTile();
            Animator.SetBool("IsWalking", false);
        }
    }
}
