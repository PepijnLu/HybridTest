using UnityEngine;

public class BodyMovementDetection : MonoBehaviour
{
    public Transform vrHeadset; // Reference to the VR headset (camera)
    public Transform xrRig; // Reference to the XR Rig (player object)
    public Transform rightController; // Reference to the right controller

    public float speedThreshold = 0.5f; // Minimum speed to detect motion
    public float directionAlignmentThreshold = 0.9f; // Cosine similarity threshold for direction alignment (1 = perfectly aligned)
    public float additionalSpeed = 2f; // Additional movement speed multiplier
    public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

    public float controllerSpeed;
    public float headsetSpeed;

    private Vector3 headsetPreviousPosition;
    private Vector3 controllerPreviousPosition;
    private Vector3 currentVelocity = Vector3.zero; // For smooth movement
    private Vector3 targetVelocity = Vector3.zero;

    // private float motionConsistencyTime = 0f;
    // private const float requiredConsistencyDuration = 0.1f;

    void Start()
    {
        // Initialize previous positions
        if (vrHeadset != null)
            headsetPreviousPosition = vrHeadset.position;

        if (rightController != null)
            controllerPreviousPosition = rightController.position;
    }

    void Update()
    {
        if (vrHeadset == null || xrRig == null || rightController == null)
            return;

        // Calculate velocities
        Vector3 headsetVelocity = (vrHeadset.position - headsetPreviousPosition) / Time.deltaTime;
        Vector3 controllerVelocity = (rightController.position - controllerPreviousPosition) / Time.deltaTime;

        // Ignore Y-axis for planar movement
        headsetVelocity.y = 0;
        controllerVelocity.y = 0;

        // Calculate speeds
        headsetSpeed = headsetVelocity.magnitude;
        controllerSpeed = controllerVelocity.magnitude;

        if (headsetSpeed < 0.05f) headsetSpeed = 0f;
        if (controllerSpeed < 0.05f) controllerSpeed = 0f;
        
        // Debug.Log($"HeadsetSpeed: {headsetSpeed}.... | .... ControllerSpeed {controllerSpeed}");
    

        // Calculate direction alignment using dot product
        float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

        // Check if both velocities exceed the speed threshold and are aligned
        if (headsetSpeed >= speedThreshold && controllerSpeed >= speedThreshold && directionAlignment >= directionAlignmentThreshold)
        {

            // motionConsistencyTime += Time.deltaTime;
            // if (motionConsistencyTime >= requiredConsistencyDuration)
            // {
            //     Vector3 movementDirection = headsetVelocity.normalized;
            //     Vector3 targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * movementDirection;

            //     float dynamicSmoothing = Mathf.Lerp(0.1f, 0.3f, headsetSpeed / speedThreshold);
            //     xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, dynamicSmoothing);
            // }
            // Calculate the movement direction based on the headset's motion
            Vector3 movementDirection = headsetVelocity.normalized;

            // Apply additional speed
            targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * movementDirection;

            // Smooth movement to avoid jitter
            xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);
        }
        // else 
        // {
        //     motionConsistencyTime = 0f;
        // }

        // Update previous positions
        headsetPreviousPosition = vrHeadset.position;
        controllerPreviousPosition = rightController.position;
    }
}


/*-- OLD ONE -- */
// using UnityEngine;

// namespace Assets.Renato.Scripts 
// {
//     public class BodyMovementDetection : MonoBehaviour
//     {
//         public Transform vrHeadset; // Reference to the VR headset (camera)
//         public Transform xrRig; // Reference to the XR Rig (player object)
//         public Transform rightController; // Reference to the right controller

//         public float headsetDistanceThreshold = 0.2f; // Minimum distance to detect movement
//         public float controllerDistanceThreshold = .2f; 
//         public float minCheckInterval = 0.5f; // Minimum interval for movement checks (in seconds)
//         public float maxCheckInterval = 1.5f; // Maximum interval for movement checks (in seconds)
//         public float additionalSpeed = 2f; // Additional movement speed multiplier
//         public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

//         public float headsetDistanceMoved;
//         public float controllerDistanceMoved;

//         private Vector3 headsetStartPosition;
//         private Vector3 controllerStartPosition;
//         private Vector3 currentVelocity = Vector3.zero; // For smooth movement
//         Vector3 targetVelocity = Vector3.zero;
//         private float elapsedTime = 0f;

//         void Start()
//         {
//             // Initialize the starting position
//             if (vrHeadset != null)
//                 headsetStartPosition = vrHeadset.position;

//             if(rightController != null)
//                 controllerStartPosition = rightController.position;
//         }

//         void Update()
//         {
//             if (vrHeadset == null || xrRig == null)
//                 return;

//             // Track elapsed time
//             elapsedTime += Time.deltaTime;

//             // Check if the elapsed time is within the specified range
//             if (elapsedTime >= minCheckInterval && elapsedTime <= maxCheckInterval)
//             {
//                 // Calculate the distance moved by the headset (ignoring Y-axis)
//                 headsetDistanceMoved = Vector2.Distance(
//                     new Vector2(headsetStartPosition.x, headsetStartPosition.z),
//                     new Vector2(vrHeadset.position.x, vrHeadset.position.z)
//                 );

//                 controllerDistanceMoved = Vector2.Distance(
//                     new Vector2(controllerStartPosition.x, controllerStartPosition.z),
//                     new Vector2(rightController.position.x, rightController.position.z)
//                 );

//                 Debug.Log($"HeadsetDistanceMoved: {headsetDistanceMoved} ..|.. ControllerDistanceMoved: {controllerDistanceMoved}");

//                 // Apply movement if the distance exceeds the threshold
//                 if (headsetDistanceMoved >= headsetDistanceThreshold && controllerDistanceMoved >= controllerDistanceThreshold)
//                 {
//                     // Calculate the movement direction
//                     Vector3 movementDirection = vrHeadset.position - headsetStartPosition;
//                     movementDirection.y = 0; // Ignore vertical movement

//                     // Apply additional speed
//                     targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * movementDirection.normalized;
//                     // targetVelocity = vrHeadset.position + additionalSpeed * Time.deltaTime * movementDirection.normalized;

//                     // Smooth movement to avoid jitter
//                     xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);
//                 }
//             }

//             // Reset for the next interval if the max interval is exceeded
//             if (elapsedTime > maxCheckInterval)
//             {
//                 headsetStartPosition = vrHeadset.position;
//                 controllerStartPosition = rightController.position;
//                 elapsedTime = 0f;
//             }
//         }
        
//     }

// }
