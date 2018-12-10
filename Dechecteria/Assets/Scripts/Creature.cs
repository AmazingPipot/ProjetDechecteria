using System.Collections;
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

        Coroutine FollowPathCoroutine;

        private void Awake()
        {
            AStar = new AStar();
        }

        // Use this for initialization
        void Start() {
            CurrentTile = Map.tiles[(int)transform.position.x, (int)transform.position.z];
        }

        // Update is called once per frame
        void Update() {

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
                FollowPathCoroutine = StartCoroutine(FollowPath());
            }

            watch.Stop();
            Debug.Log("Path found in " + watch.ElapsedMilliseconds + " millisecond(s).");
            
        }

        Tile GetCurrentTile()
        {
            return Map.tiles[(int)transform.position.x, (int)transform.position.z]; 
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

            Animator.SetBool("IsWalking", false);
        }
    }
}
