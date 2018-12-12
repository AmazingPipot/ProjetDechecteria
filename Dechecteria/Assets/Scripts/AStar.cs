using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    class AStar
    {
        class Node
        {
            public Vector2 Position;
            public Node Parent;
            public float G;
            public float H;
            public float F()
            {
                return G + H;
            }
            public Node (Vector2 position, Node parent)
            {
                this.Position = position;
                this.Parent = parent;
            }
            public int CompareTo(object obj)
            {
                return (int)(F() - ((Node)obj).F());
            }

            public override bool Equals(object obj)
            {
                return this.Position == ((Node)obj).Position;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public bool[,] Map;
        public List<Vector2> Path;
        public Vector2 Start;
        public Vector2 Destination;

	    public bool FindPath(Vector2 start, Vector2 destination, bool[,] map)
        {
            Destination = destination;
            Map = map;

            Path = new List<Vector2>();

            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node>();

            Node startNode = new Node(start, null)
            {
                G = 0.0f
            };
            startNode.H = ComputeHeuristic(startNode);
            openList.Add(startNode);

            Debug.Log("Starting AStar at " + start.x + " " + start.y + ", destination: " + destination.x + " " + destination.y);

            while (openList.Count > 0)
            {
                // Debug.Log("openList count: " + openList.Count);
                openList.Sort((node1, node2) => node1.CompareTo(node2));
                Node fromNode = openList[0];
                openList.RemoveAt(0);
                // Debug.Log("Get and remove first element: " + fromNode.Position.x + " " + fromNode.Position.y);

                if (fromNode.Position == Destination)
                {
                    // reconstituer le chemin
                    Node parent = fromNode;
                    while (parent != null)
                    {
                        Path.Add(parent.Position);
                        /*
                        GameObject go = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                        go.transform.position = new Vector3(parent.Position.x, 0.25f, parent.Position.y);
                        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        */
                        parent = parent.Parent;
                    }
                    Path.Reverse();
                    return true;
                }

                List<Vector2> nextPositions = GetAdjacentPositions(fromNode.Position);
                foreach(Vector2 position in nextPositions)
                {
                    int x = (int)position.x;
                    int y = (int)position.y;
                    // Stay within the grid's boundaries
                    if (!IsValidCoordinate(x, y))
                        continue;

                    // Ignore non-walkable nodes
                    if (!Map[x, y])
                        continue;

                    Node newNode = new Node(position, fromNode)
                    {
                        G = fromNode.G + 1.0f
                    };

                    // Ignore already-closed nodes
                    bool inClosedList = closedList.Contains(newNode);
                    bool inOpenList = openList.Contains(newNode);
                    if (inClosedList || inOpenList)
                    {
                        if (inClosedList)
                        {
                            int index = closedList.IndexOf(newNode);
                            if (newNode.G < closedList[index].G)
                            {
                                Debug.Log("closedList: " + position.x + " " + position.y + " old: " + closedList[index].G + " new: " + fromNode.G + 1);
                                closedList[index].G = newNode.G;
                                closedList[index].Parent = fromNode;
                            }
                        }
                        else
                        {
                            int index = openList.IndexOf(newNode);
                            if (newNode.G < openList[index].G)
                            {
                                Debug.Log("indexList: " + position.x + " " + position.y + " old: " + openList[index].G + " new: " + newNode.G);
                                openList[index].G = newNode.G;
                                openList[index].Parent = fromNode;
                            }
                        }
                        
                    }
                    else
                    {
                        // Debug.Log("Adding " + position.x + " " + position.y + ", fromNode: " + fromNode.Position.x + " " + fromNode.Position.y);
                        newNode.H = ComputeHeuristic(newNode);
                        openList.Add(newNode);
                    }

                }
                closedList.Add(fromNode);
            }

            Debug.Log("No path found...");
            return false;

        }

        private List<Vector2> GetAdjacentPositions(Vector2 position)
        {
            return new List<Vector2> {
                new Vector2(position.x - 1, position.y), // left 
                new Vector2(position.x + 1, position.y), // right
                new Vector2(position.x, position.y - 1), // bottom
                new Vector2(position.x, position.y + 1), // top
            };
        }

        float ComputeHeuristic(Node node)
        {
            return Mathf.Pow(node.Position.x - Destination.x, 2) + Mathf.Pow(node.Position.y - Destination.y, 2);
        }

        bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < Map.GetLength(0)
                && y >= 0 && y < Map.GetLength(1);
        }
    }

}
