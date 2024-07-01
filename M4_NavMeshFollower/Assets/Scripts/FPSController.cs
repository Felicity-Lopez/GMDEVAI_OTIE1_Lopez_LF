using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float speed = 6f;
    public float jumpForce = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 180f;
    public float lookYLimit = 45f;

    Vector3 moveDir = Vector3.zero;
    float rotX = 0;
    float rotY = 0;

    public bool canMove = true;

    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float speedX = Input.GetAxis("Vertical") * speed;
        float speedY = Input.GetAxis("Horizontal") * speed;

        Vector3 move = (forward * speedX) + (right * speedY);
        #endregion

        #region Handles Jumping
        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDir.y = jumpForce;
            }
            else
            {
                moveDir.y = -1f; // Slight downward force to ensure character stays grounded
            }
        }
        else
        {
            moveDir.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Combine Movement and Jump
        moveDir.x = move.x;
        moveDir.z = move.z;
        characterController.Move(moveDir * Time.deltaTime);
        #endregion

        #region Handles Rotation
        characterController.Move(moveDir * Time.deltaTime);

        if (canMove)
        {
            rotX += Input.GetAxis("Mouse X") * lookSpeed;
            rotY += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotY = Mathf.Clamp(rotY, -lookYLimit, lookYLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotY, rotX, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        playerCamera.transform.position = transform.position + new Vector3(0, characterController.height / 2, 0);
    }
}
