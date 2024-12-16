// using UnityEngine;
// using UnityEngine.XR;

// public class SpeedMultiplier : MonoBehaviour
// {
//     public Camera cam;
//     public Transform cameraOffset, playerCam;
//     public float moveSpeed, speedMult;

//     private Vector3 lastHeadsetPosition, headsetPosition; // The position of the object in the previous frame
//     private float calculatedMovementSpeed; // Calculated movement speed (units/second)

//     [Range(1f, 100f)]
//     public float speedMultiplier;

//     void Start()
//     {
//         lastHeadsetPosition = headsetPosition;
//     }

//     void Update()
//     {
//         headsetPosition = InputTracking.GetLocalPosition(XRNode.Head);

//         // Log the real-world relative position
//         Debug.Log($"Headset Position (Local): {headsetPosition}");

//         Vector3 displacement = headsetPosition - lastHeadsetPosition;

//         Vector3 forwardDirection = (headsetPosition - lastHeadsetPosition).normalized;

//         // Calculate the movement speed (distance divided by time)
//         calculatedMovementSpeed = displacement.magnitude / Time.deltaTime;

//         Debug.Log("Calculated MS: " + calculatedMovementSpeed);

//         // Update the last position for the next frame
//         lastHeadsetPosition = headsetPosition;

//         cameraOffset.position += forwardDirection * calculatedMovementSpeed * speedMult * Time.deltaTime;
//     }
// }

using UnityEngine;
using UnityEngine.XR;

public class SpeedMultiplier : MonoBehaviour
{
    public Transform cameraOffset; // Reference to XR Origin's offset transform
    public float speedMult = 1.5f; // Speed multiplier for movement
    private Vector3 lastHeadsetPosition; // Last frame's headset position
    private Quaternion lastHeadsetRotation; // Last frame's headset rotation
    private float calculatedMovementSpeed; // Movement speed in units/second

    void Start()
    {
        // Initialize the headset position and rotation
        lastHeadsetPosition = InputTracking.GetLocalPosition(XRNode.Head);
        lastHeadsetRotation = InputTracking.GetLocalRotation(XRNode.Head);
    }

    void Update()
    {
        // Get the current headset position in local space
        Vector3 currentHeadsetPosition = InputTracking.GetLocalPosition(XRNode.Head);

        // Calculate the movement direction based on displacement
        Vector3 displacement = currentHeadsetPosition - lastHeadsetPosition;

        // Neutralize rotational effects
        Vector3 neutralizedDisplacement = lastHeadsetRotation * displacement;

        // Ignore vertical movement (Y-axis)
        neutralizedDisplacement.y = 0;

        // Calculate movement speed (distance over time)
        calculatedMovementSpeed = neutralizedDisplacement.magnitude / Time.deltaTime;

        // Debug the movement
        Debug.Log($"Displacement: {neutralizedDisplacement}, Speed: {calculatedMovementSpeed}");

        // Apply the movement in the real-world direction
        if (neutralizedDisplacement.magnitude > 0.01f) // Ignore small jitter
        {
            cameraOffset.position += neutralizedDisplacement.normalized * calculatedMovementSpeed * speedMult * Time.deltaTime;
        }

        // Update the last position and rotation
        lastHeadsetPosition = currentHeadsetPosition;
        lastHeadsetRotation = InputTracking.GetLocalRotation(XRNode.Head);
    }
}

