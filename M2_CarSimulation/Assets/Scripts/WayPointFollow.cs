using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointFollow : MonoBehaviour
{
    //public GameObject[] WayPoints;

    public UnityStandardAssets.Utility.WaypointCircuit circuit;
    int currWaypointIndex = 0;

    float speed = 5;
    float rotSpeed = 3;
    float accuracy = 1;

    // Start is called before the first frame update
    void Start()
    {
        //WayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (circuit.Waypoints.Length == 0) return; // Checks if no waypoints --> exit

        GameObject currWaypoint = circuit.Waypoints[currWaypointIndex].gameObject;

        Vector3 lookAtGoal = new Vector3(currWaypoint.transform.position.x,
                                         this.transform.position.y,
                                         currWaypoint.transform.position.z);

        Vector3 direction = lookAtGoal - this.transform.position;

        if (direction.magnitude < 1.0f) 
        {
            currWaypointIndex++;

            if (currWaypointIndex >= circuit.Waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                   Quaternion.LookRotation(direction),
                                                   Time.deltaTime * rotSpeed);

        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
