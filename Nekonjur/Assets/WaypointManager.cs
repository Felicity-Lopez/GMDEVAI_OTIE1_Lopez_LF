using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public struct link
{
    public enum direction { UNI, BI}; // Undirected and Directed Graphs
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class WaypointManager : MonoBehaviour
{
    public GameObject[] waypoints;
    public link[] links; // Determines how our nodes will be connected
    public Graph graph = new Graph(); // Part of unity package used for A* Algorithm

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp); // Adds waypoints as node to our Graph Object
            }

            foreach (link l in links)
            {
                graph.AddEdge(l.node1, l.node2); // Adds links as edges to our Graph Object
                if (l.dir == link.direction.BI) // If link is bi-directional or two-way edge
                {
                    graph.AddEdge(l.node2, l.node1); // Then add reverse edge
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        graph.debugDraw(); // Draws the graph for you
    }
}
