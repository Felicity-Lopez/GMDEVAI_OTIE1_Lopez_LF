using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal;

    public float speed = 5.0f;
    float accuracy = 1.0f;
    public float rotSpeed = 2.0f;

    public GameObject wpManager;
    GameObject[] wps;
    GameObject currNode;
    int currWaypointIndex = 0;
    Graph graph;

    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WaypointManager>().waypoints;
        graph = wpManager.GetComponent<WaypointManager>().graph;
        currNode = wps[0];
    }

    void LateUpdate() // Movement while physics is in Update()
    {
        if (graph.getPathLength() == 0 || currWaypointIndex == graph.getPathLength())
        {
            return;
        }

        // Node we are closest to at the moment
        currNode = graph.getPathPoint(currWaypointIndex);

        // If we are close enough to the current waypoint, move to the next waypoint
        if (Vector3.Distance(graph.getPathPoint(currWaypointIndex).transform.position,
                             transform.position) < accuracy)
        {
            currWaypointIndex++;
        }

        // If we aren't at the end of the path
        if (currWaypointIndex < graph.getPathLength())
        {
            goal = graph.getPathPoint(currWaypointIndex).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                       Quaternion.LookRotation(direction),
                                                       Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    public void SetDestination(int waypointIndex)
    {
        graph.AStar(currNode, wps[waypointIndex]);
        currWaypointIndex = 0;
    }
}
