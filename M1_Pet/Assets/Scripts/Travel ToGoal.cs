using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToGoal : MonoBehaviour
{
    public Transform goal;
    public float movementSpeed = 5;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = goal.position - this.transform.position;

        transform.LookAt(goal);
        if (direction.magnitude > 2)
        {
            
            transform.Translate(direction.normalized * movementSpeed * Time.deltaTime, Space.World);
        }
        
    }
}
