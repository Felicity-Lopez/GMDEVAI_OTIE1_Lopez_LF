using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 5.0f;
    public float currSpeed;
    public float rotationSpeed = 100.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()  // Using Update for smoother input handling
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A and D keys
        float moveVertical = Input.GetAxis("Vertical");     // W and S keys

        // Create a movement vector that is relative to the player's orientation
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement);

        // Move the player by translating its transform
        transform.position += movement * speed * Time.deltaTime;

        currSpeed = movement.magnitude;

        // Update the animator's Speed parameter based on the magnitude of the input vector
        animator.SetFloat("Speed", currSpeed);
    }

    void RotatePlayer()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");

        // Apply rotation directly to the transform
        transform.Rotate(0, horizontalRotation * rotationSpeed * Time.deltaTime, 0);
    }
}
