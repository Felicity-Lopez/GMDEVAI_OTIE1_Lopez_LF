using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Player's transform
    public Vector3 offset;         // Offset from the player
    public float smoothSpeed = 0.125f; // Smoothing speed
    public float rotationSpeed = 5.0f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset; // Calculate desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); // Smoothly interpolate to the desired position
                                                                                                                    // Update the camera position

        Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.rotation = smoothedRotation;
    }
}
