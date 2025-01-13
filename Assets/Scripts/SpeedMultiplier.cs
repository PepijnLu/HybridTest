using UnityEngine;

public class BodyMovementDetection : MonoBehaviour
{
    public Transform vrHeadset; // Reference to the VR headset (camera)
    public Transform xrRig; // Reference to the XR Rig (player object)
    public Transform rightController; // Reference to the right controller

    public float headsetDistanceThreshold = 0.2f; // Minimum distance to detect movement
    public float controllerDistanceThreshold = .2f; 
    public float minCheckInterval = 0.5f; // Minimum interval for movement checks (in seconds)
    public float maxCheckInterval = 1.5f; // Maximum interval for movement checks (in seconds)
    public float additionalSpeed = 2f; // Additional movement speed multiplier
    public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

    private Vector3 headsetStartPosition;
    private Vector3 controllerStartPosition;
    private Vector3 currentVelocity = Vector3.zero; // For smooth movement
    Vector3 targetVelocity = Vector3.zero;
    private float elapsedTime = 0f;

    void Start()
    {
        // Initialize the starting position
        if (vrHeadset != null)
            headsetStartPosition = vrHeadset.position;

        if(rightController != null)
            controllerStartPosition = rightController.position;
    }

    void Update()
    {
        if (vrHeadset == null || xrRig == null)
            return;

        // Track elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the elapsed time is within the specified range
        if (elapsedTime >= minCheckInterval && elapsedTime <= maxCheckInterval)
        {
            // Calculate the distance moved by the headset (ignoring Y-axis)
            float headsetDistanceMoved = Vector2.Distance(
                new Vector2(headsetStartPosition.x, headsetStartPosition.z),
                new Vector2(vrHeadset.position.x, vrHeadset.position.z)
            );

            float controllerDistanceMoved = Vector2.Distance(
                new Vector2(controllerStartPosition.x, controllerStartPosition.z),
                new Vector2(rightController.position.x, rightController.position.z)
            );

            // Apply movement if the distance exceeds the threshold
            if (headsetDistanceMoved >= headsetDistanceThreshold && controllerDistanceMoved >= controllerDistanceThreshold)
            {
                // Calculate the movement direction
                Vector3 movementDirection = vrHeadset.position - headsetStartPosition;
                movementDirection.y = 0; // Ignore vertical movement

                // Apply additional speed
                targetVelocity = xrRig.position + movementDirection.normalized * additionalSpeed * Time.deltaTime;

                // Smooth movement to avoid jitter
                xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);
            }
        }

        // Reset for the next interval if the max interval is exceeded
        if (elapsedTime > maxCheckInterval)
        {
            headsetStartPosition = vrHeadset.position;
            controllerStartPosition = rightController.position;
            elapsedTime = 0f;
        }
    }
    
}

// Get the headset last position
// Get the headset current position
// Get the right controller last position
// Get the right controller current position
// Calculate the approximate distance from the last position to the current position of both the headset and the right controller
// Check if the distance of the headset and the right controller is approximately the same. Exclude the Y axis
// So for example if the distance for the headset is  0.5f and the right controller 0.75f then it's approximately the same. 
// Although I would like to have a threshold for that calculation. So let's say the threshold is 1f then as long as the two distances dont exceed the 1f range from each other, the condition is met
// If the condition is met, then that means 'movement' is detected and so add additional movement speed to the current movement speed
// So that also means that I have to calculate the current movement speed and add the additional movement speed to it
// I also need to make it so that it doesn't jitter when moving because of the applied additional movement speed



// using UnityEngine;

// public class SpeedMultiplier : MonoBehaviour
// {
//     public Transform xrRig;          // XR Rig root
//     public Transform headset;        // Main Camera (headset)
//     private Vector3 lastHeadsetWorldPosition;  // Last valid position in world space
//     private Quaternion lastHeadsetRotation;    // Last headset rotation

//     public float speedMultiplier = 2f;         // Multiplier for movement speed
//     public float velocityThreshold = 0.1f;     // Minimum velocity for movement
//     public float tiltRotationThreshold = 2f;  // Threshold for filtering tilt/rotation effects (degrees)

//     void Start()
//     {
//         if (xrRig == null || headset == null)
//         {
//             Debug.LogError("Assign the XR Rig and Headset in the Inspector.");
//             enabled = false;
//             return;
//         }

//         lastHeadsetWorldPosition = headset.position;
//         lastHeadsetRotation = headset.rotation;
//     }

//     void Update()
//     {
//         // Get current position and rotation
//         Vector3 currentHeadsetWorldPosition = headset.position;
//         Quaternion currentHeadsetRotation = headset.rotation;

//         // Displacement
//         Vector3 displacement = currentHeadsetWorldPosition - lastHeadsetWorldPosition;

//         // Ignore vertical movement
//         displacement.y = 0;

//         // Calculate headset velocity
//         float headsetSpeed = displacement.magnitude / Time.deltaTime;

//         // Calculate rotational difference
//         float rotationDifference = Quaternion.Angle(lastHeadsetRotation, currentHeadsetRotation);

//         // Filter out displacement if caused by head rotation
//         if (rotationDifference > tiltRotationThreshold)
//         {
//             Debug.Log("Rotation-based displacement ignored.");
//             displacement = Vector3.zero;
//         }

//         // Apply movement if velocity exceeds the threshold
//         if (headsetSpeed > velocityThreshold)
//         {
//             Debug.Log($"Movement detected. Applying speed multiplier. Velocity: {headsetSpeed}");
//             xrRig.position += displacement.normalized * speedMultiplier * Time.deltaTime;
//         }
//         else
//         {
//             Debug.Log("Movement below threshold. No speed multiplier applied.");
//         }

//         // Update last position and rotation
//         lastHeadsetWorldPosition = currentHeadsetWorldPosition;
//         lastHeadsetRotation = currentHeadsetRotation;
//     }
// }


// using UnityEngine;

// public class SpeedMultiplier : MonoBehaviour
// {
//     public Transform xrRig;          // XR Rig root
//     public Transform headset;        // Main Camera (headset)
    
//     private Vector3 lastHeadsetWorldPosition;  // Last valid position in world space
//     private Vector3 smoothedVelocity;         // Smoothed velocity to avoid sudden spikes
//     private float smoothingFactor = 0.1f;     // Smoothing factor for velocity calculations
    
//     private Quaternion lastHeadsetRotation;   // Last headset rotation

//     public float speedMultiplier = 2f;        // Multiplier for movement speed
//     public float velocityThreshold = 0.02f;   // Minimum velocity for movement
//     public float tiltRotationThreshold = 5f; // Threshold to filter rotation effects (degrees)
//     public float stopThreshold = 0.01f;       // Threshold to ensure precise stops

//     void Start()
//     {
//         if (xrRig == null || headset == null)
//         {
//             Debug.LogError("Assign the XR Rig and Headset in the Inspector.");
//             enabled = false;
//             return;
//         }

//         lastHeadsetWorldPosition = headset.position;
//         lastHeadsetRotation = headset.rotation;
//     }

//     void Update()
//     {
//         // Get current position and rotation
//         Vector3 currentHeadsetWorldPosition = headset.position;
//         Quaternion currentHeadsetRotation = headset.rotation;

//         // Compute displacement
//         Vector3 displacement = currentHeadsetWorldPosition - lastHeadsetWorldPosition;

//         // Ignore vertical movement
//         displacement.y = 0;

//         // Calculate raw velocity
//         float rawVelocity = displacement.magnitude / Time.deltaTime;

//         // Smooth velocity for more stable results
//         smoothedVelocity = Vector3.Lerp(smoothedVelocity, displacement / Time.deltaTime, smoothingFactor);

//         // Ignore motion caused by head tilts or rotations
//         float rotationDifference = Quaternion.Angle(lastHeadsetRotation, currentHeadsetRotation);
//         if (rotationDifference > tiltRotationThreshold)
//         {
//             Debug.Log("Rotation-based motion ignored.");
//             displacement = Vector3.zero;
//         }

//         // Check if movement exceeds the stop threshold
//         if (smoothedVelocity.magnitude > velocityThreshold)
//         {
//             Debug.Log($"Movement detected. Applying speed multiplier. Velocity: {smoothedVelocity.magnitude}");
//             xrRig.position += smoothedVelocity.normalized * speedMultiplier * Time.deltaTime;
//         }
//         else if (rawVelocity < stopThreshold)
//         {
//             Debug.Log("Character stopped.");
//         }
//         else
//         {
//             Debug.Log("Movement below velocity threshold. Ignored.");
//         }

//         // Update last position and rotation
//         lastHeadsetWorldPosition = currentHeadsetWorldPosition;
//         lastHeadsetRotation = currentHeadsetRotation;
//     }
// }




/*

A star algorithm

Steps:
- create an open and closed list
- create a starting node
- update list
- find the lowest node
- update lists
- check for neighbour cells (not node yet)
- get the direction from each cell
- create walls
- map the directions
- store the cell's position
- calculate G score
- fetch the neighbour node or create one if it doesn't exist yet
- if it exist, update the node and the G score

How it work:
Start first with creating 2 lists, an open and closed list. You need this because every node that has been traversed needs to be added into the closed list and every node that you are currently on has to be added into the open list

After initializing the lists, a starting node has to be created at the start position. 
*/