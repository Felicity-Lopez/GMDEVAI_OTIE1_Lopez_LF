using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 0.03f;
    public float rotSpeed = 3;

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(xDir, 0.0f, zDir);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                   Quaternion.LookRotation(direction), 
                                                   Time.deltaTime * rotSpeed);

        transform.position += direction * movementSpeed;
    }
}
