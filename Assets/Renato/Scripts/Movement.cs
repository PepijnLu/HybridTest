using UnityEngine;

public class Movement : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Sensitivity for mouse input
    public Transform playerBody;         // Reference to the player body for rotation

    public float moveSpeed = 5f;         // Speed for player movement

    private float xRotation = 0f;        // Tracks up and down camera rotation

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera up and down
        playerBody.Rotate(Vector3.up * mouseX); // Rotate player body left and right

        // Handle movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        playerBody.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
