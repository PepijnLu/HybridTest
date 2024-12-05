using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera, playerCamTransform, windowCam;

    private float xRotation = 0f;
    public bool justTeleported;

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = new Vector3(transform.position.x, 1.31f, transform.position.z);
    }

    void Update()
    {
        // Move();
        // LookAround();
    }

    void Move()
    {
        // Get the input axes for movement (WASD keys)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move the player in the direction they are facing
        Vector3 move = transform.right * x + transform.forward * z;
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }

    void LookAround()
    {
        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // // Get mouse input for rotation
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // // Calculate rotation around the camera
        // Vector3 rotation = new Vector3(-mouseY, mouseX, 0f); // Invert mouseY for intuitive movement

        // // Apply rotation to the camera
        // playerCamera.Rotate(rotation);

        // // Optional: Clamp the camera's vertical rotation to avoid flipping
        // Vector3 eulerAngles = playerCamera.eulerAngles;
        // eulerAngles.x = Mathf.Clamp(eulerAngles.x > 180f ? eulerAngles.x - 360f : eulerAngles.x, -90f, 90f);
        // playerCamera.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0f); // Set z to 0 to avoid roll
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "epic collider")
        {
            // PlayerController playerController = this;
            // Transform reciever = windowCam;
            // // Teleport him!
            // Vector3 portalToPlayer = playerController.gameObject.transform.position - transform.position;
            // float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
            // rotationDiff += 180;
            // playerController.gameObject.transform.Rotate(Vector3.up, rotationDiff);

            // Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;

            // Vector3 newPosition = reciever.position + positionOffset;
            // newPosition.y = playerController.gameObject.transform.position.y;

            // Debug.Log($"Teleported: from {playerController.gameObject.transform.position} to {newPosition}");
            // playerController.gameObject.transform.position = newPosition;

            // playerController.justTeleported = true;
            
            // PortalTeleporter recieverPortal = reciever.gameObject.GetComponent<PortalTeleporter>();

            //transform.position += new Vector3(0, 0, 25);

        }
            
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "epic collider 2")
        {
            //transform.position -= new Vector3(0, 0, 25);

        }
    }
}
