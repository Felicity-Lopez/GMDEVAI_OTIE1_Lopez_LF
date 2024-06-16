using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelForwardToGoal : MonoBehaviour
{
    public Transform goal;

    private float currSpeed = 0;
    public float maxSpeed = 3;
    public float acceleration = 2;
    public float deceleration = 4;


    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtGoal = new Vector3(goal.position.x,
                                         this.transform.position.y,
                                         goal.position.z);

        Vector3 direction = lookAtGoal - transform.position;

        this.transform.rotation = Quaternion.LookRotation(direction);

        float distanceToGoal = Vector3.Distance(lookAtGoal, transform.position);

        if (distanceToGoal > 2)
        {
            // Accelerate
            currSpeed = Mathf.Lerp(currSpeed, maxSpeed, Time.deltaTime * acceleration);
        }
        else
        {
            // Decelerate
            currSpeed = Mathf.Lerp(currSpeed, 0, Time.deltaTime * deceleration);
        }

        transform.Translate(0, 0, currSpeed * Time.deltaTime);
    }
}

