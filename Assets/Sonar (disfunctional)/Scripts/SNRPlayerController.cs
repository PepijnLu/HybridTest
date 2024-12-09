using System.Collections;
using UnityEngine;

public class SNRPlayerController : MonoBehaviour
{
    // Variables
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    public Transform cameraTransform;
    private float xRotation = 0f;

    SimpleSonarShader_Object sonarShader;

    bool ePressed;
    public bool loudSoundMade;

    public float activeIntensity, testVolume;
    public AudioSource testSource;

    Collision lastCollisionPoint;
    [SerializeField] Transform startLoc;
    [SerializeField] GameObject playerCam;

    void Start()
    {
        // Getting the CharacterController component
        characterController = GetComponent<CharacterController>();

        // Locking the cursor to the center of the screen and hiding it
        Cursor.lockState = CursorLockMode.Locked;
        startLoc = GameObject.Find("startLoc").transform;
        transform.position = startLoc.position;
        sonarShader = GetComponent<SimpleSonarShader_Object>();
        testSource = GetComponent<AudioSource>();
        Debug.Log(testSource);  
    }

    void Update()
    {

        // Handle movement
        Move();

        // Handle camera look
        Look();

        // Handle jumping
        Jump();
    }

    void Move()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the direction based on the player's input
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the character
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Adjust the camera rotation for up and down (vertical)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the camera rotation
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player left and right (horizontal)
        transform.Rotate(Vector3.up * mouseX);
    }
    public void TriggerSonar(float volume)
    {
        if (lastCollisionPoint != null)
        {
            AudioManager.instance.PlaySound(testSource, volume, lastCollisionPoint);
        }
    }
    void Jump()
    {
        // Check if the player is grounded
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            // Reset the downward velocity when on the ground
            velocity.y = -2f;
        }

        // Jump when the spacebar is pressed
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //Debug.Log("collision itself: " + gameObject.name);
        //Debug.Log("Collision with: " + collision.gameObject.name);
        lastCollisionPoint = collision;
        // Start sonar ring from the contact point
        //if (collision.gameObject.layer == 6)
        //{
            
        //}
    }

    IEnumerator ECooldown()
    {
        yield return new WaitForSeconds(0.5f);
        ePressed = false;
    }
}
