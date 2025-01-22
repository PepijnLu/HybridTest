// using UnityEngine;

// public class BodyMovementDetection : MonoBehaviour
// {
//     public Transform vrHeadset; // Reference to the VR headset (camera)
//     public Transform xrRig; // Reference to the XR Rig (player object)
//     public Transform rightController; // Reference to the right controller

//     // public float speedThreshold = 0.5f; // Minimum speed to detect motion
//     public float VRspeedThreshold = 0.05f, controllerSpeedThreshold = 0.05f; // Speed thresholds

//     public float directionAlignmentThreshold = 0.95f; // Cosine similarity threshold for direction alignment (1 = perfectly aligned)
//     public float additionalSpeed = 2f; // Additional movement speed multiplier
//     public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

//     public float controllerSpeed;
//     public float headsetSpeed;

//     private Vector3 headsetPreviousPosition;
//     private Vector3 controllerPreviousPosition;
//     private Vector3 currentVelocity = Vector3.zero; // For smooth movement
//     private Vector3 targetVelocity = Vector3.zero;

//     // private float motionConsistencyTime = 0f;
//     // private const float requiredConsistencyDuration = 0.1f;

//     private Vector3 lastValidMovementDirection = Vector3.zero; // Last valid movement direction


//     void Start()
//     {
//         // Initialize previous positions
//         if (vrHeadset != null)
//             headsetPreviousPosition = vrHeadset.position;

//         if (rightController != null)
//             controllerPreviousPosition = rightController.position;
//     }
    
//     private float timer = 0f;

//     void Update()
//     {
//         if (vrHeadset == null || xrRig == null || rightController == null)
//             return;

//         // Calculate velocities
//         Vector3 headsetVelocity = (vrHeadset.position - headsetPreviousPosition) / Time.deltaTime;
//         Vector3 controllerVelocity = (rightController.position - controllerPreviousPosition) / Time.deltaTime;

//         // Ignore Y-axis for planar movement
//         headsetVelocity.y = 0;
//         controllerVelocity.y = 0;

//         // Calculate speeds
//         headsetSpeed = headsetVelocity.magnitude;
//         controllerSpeed = controllerVelocity.magnitude;

//         if (headsetSpeed < VRspeedThreshold) headsetSpeed = 0f;
//         if (controllerSpeed < controllerSpeedThreshold) controllerSpeed = 0f;
        
//         // Debug.Log($"HeadsetSpeed: {headsetSpeed}.... | .... ControllerSpeed {controllerSpeed}");

//         // Calculate direction alignment using dot product
//         float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

//         // Check if both velocities exceed the speed threshold and are aligned
//         if (headsetSpeed >= VRspeedThreshold && controllerSpeed >= controllerSpeedThreshold && directionAlignment >= directionAlignmentThreshold)
//         {
//             // Calculate the movement direction based on the headset's motion
//             Vector3 movementDirection = headsetVelocity.normalized;

//             lastValidMovementDirection = movementDirection;

//             // Apply additional speed
//             targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;

//             // Smooth movement to avoid jitter
//             xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);
//             // targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * movementDirection;

//         }
//         else 
//         {
//             // Decay movement direction to stop smoothly
//             lastValidMovementDirection = Vector3.Lerp(lastValidMovementDirection, Vector3.zero, smoothingFactor);
//             targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;
//         }
        
//         // xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);

//         // Update previous positions
//         headsetPreviousPosition = vrHeadset.position;
//         controllerPreviousPosition = rightController.position;
//     }
// }

// using UnityEngine;

// public class BodyMovementDetection : MonoBehaviour
// {
//     public Transform vrHeadset; // Reference to the VR headset (camera)
//     public Transform xrRig; // Reference to the XR Rig (player object)
//     public Transform rightController; // Reference to the right controller

//     public float VRspeedThreshold = 0.25f, controllerSpeedThreshold = 0.35f; // Speed thresholds
//     public float directionAlignmentThreshold = 0.95f; // Cosine similarity threshold for direction alignment
//     public float additionalSpeed = 20f; // Additional movement speed multiplier
//     public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

//     public float rotationThreshold = 10f; // Threshold for detecting significant rotation (in degrees per frame)
//     public float controllerSpeed;
//     public float headsetSpeed;

//     private Vector3 headsetPreviousPosition;
//     private Vector3 controllerPreviousPosition;
//     private Quaternion headsetPreviousRotation; // Store the previous rotation of the headset
//     private Vector3 currentVelocity = Vector3.zero;
//     private Vector3 targetVelocity = Vector3.zero;

//     private Vector3 lastValidMovementDirection = Vector3.zero;

//     void Start()
//     {
//         if (vrHeadset != null)
//         {
//             headsetPreviousPosition = vrHeadset.position;
//             headsetPreviousRotation = vrHeadset.rotation; // Initialize the previous rotation
//         }

//         if (rightController != null)
//             controllerPreviousPosition = rightController.position;
//     }

//     void Update()
//     {
//         if (vrHeadset == null || xrRig == null || rightController == null)
//             return;

//         // Calculate velocities
//         Vector3 headsetVelocity = (vrHeadset.position - headsetPreviousPosition) / Time.deltaTime;
//         Vector3 controllerVelocity = (rightController.position - controllerPreviousPosition) / Time.deltaTime;

//         // Ignore Y-axis for planar movement
//         headsetVelocity.y = 0;
//         controllerVelocity.y = 0;

//         // Calculate speeds
//         headsetSpeed = headsetVelocity.magnitude;
//         controllerSpeed = controllerVelocity.magnitude;

//         if (headsetSpeed < VRspeedThreshold) headsetSpeed = 0f;
//         if (controllerSpeed < controllerSpeedThreshold) controllerSpeed = 0f;

//         // Calculate rotation difference
//         float rotationDifference = Quaternion.Angle(vrHeadset.rotation, headsetPreviousRotation);

//         // Debugging rotation difference
//         Debug.Log($"Rotation Difference: {rotationDifference}");

//         // If rotation is below the threshold, allow movement calculation
//         if (rotationDifference < rotationThreshold)
//         {
//             // Calculate direction alignment using dot product
//             float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

//             // Check if both velocities exceed the speed threshold and are aligned
//             if (headsetSpeed >= VRspeedThreshold && controllerSpeed >= controllerSpeedThreshold && directionAlignment >= directionAlignmentThreshold)
//             {
//                 // Calculate the movement direction based on the headset's motion
//                 Vector3 movementDirection = headsetVelocity.normalized;

//                 lastValidMovementDirection = movementDirection;

//                 // Apply additional speed
//                 targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;

//                 // Smooth movement to avoid jitter
//                 xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);

//             }
//             else
//             {
//                 // Decay movement direction to stop smoothly
//                 lastValidMovementDirection = Vector3.Lerp(lastValidMovementDirection, Vector3.zero, smoothingFactor);
//                 targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;
//                 xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);
//             }
//         }

//         // Update previous positions and rotations
//         headsetPreviousPosition = vrHeadset.position;
//         controllerPreviousPosition = rightController.position;
//         headsetPreviousRotation = vrHeadset.rotation;
//     }
// }

using UnityEngine;

public class BodyMovementDetection : MonoBehaviour
{
    public Transform vrHeadset; // Reference to the VR headset (camera)
    public Transform xrRig; // Reference to the XR Rig (player object)
    public Transform rightController; // Reference to the right controller

    public float VRspeedThreshold = 0.25f, controllerSpeedThreshold = 0.35f; // Speed thresholds
    public float directionAlignmentThreshold = 0.95f; // Cosine similarity threshold for direction alignment
    public float additionalSpeed = 20f; // Additional movement speed multiplier
    public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

    // public float rotationThreshold = 10f; // Threshold for detecting significant rotation (in degrees per frame)
    public float rotationSensitivity = 0.1f; // Sensitivity for detecting small rotations
    public float controllerSpeed;
    public float headsetSpeed;

    private Vector3 headsetPreviousPosition;
    private Vector3 controllerPreviousPosition;
    private Quaternion headsetPreviousRotation; // Store the previous rotation of the headset
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;

    private Vector3 lastValidMovementDirection = Vector3.zero;

    public bool isMoving;

    void Start()
    {
        if (vrHeadset != null)
        {
            headsetPreviousPosition = vrHeadset.position;
            headsetPreviousRotation = vrHeadset.rotation; // Initialize the previous rotation
        }

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

        if (headsetSpeed < VRspeedThreshold) headsetSpeed = 0f;
        if (controllerSpeed < controllerSpeedThreshold) controllerSpeed = 0f;

        // Calculate rotation difference
        float rotationDifference = Quaternion.Angle(vrHeadset.rotation, headsetPreviousRotation);

        // Debugging rotation difference
        Debug.Log($"Rotation Difference: {rotationDifference}");

        // Check for small rotation changes
        if (rotationDifference > rotationSensitivity)
        {
            // If rotation is detected, ignore movement for this frame
            headsetPreviousPosition = vrHeadset.position;
            controllerPreviousPosition = rightController.position;
            headsetPreviousRotation = vrHeadset.rotation;
            return;
        }

        // Calculate direction alignment using dot product
        float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

        // Check if both velocities exceed the speed threshold and are aligned
        if (headsetSpeed >= VRspeedThreshold && controllerSpeed >= controllerSpeedThreshold && directionAlignment >= directionAlignmentThreshold)
        {
            // Calculate the movement direction based on the headset's motion
            Vector3 movementDirection = headsetVelocity.normalized;

            lastValidMovementDirection = movementDirection;

            // Apply additional speed
            targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;

            // Smooth movement to avoid jitter
            xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);

            isMoving = true;
        }
        else
        {
            // Decay movement direction to stop smoothly
            lastValidMovementDirection = Vector3.Lerp(lastValidMovementDirection, Vector3.zero, smoothingFactor);
            targetVelocity = xrRig.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;
            xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);

            isMoving = false;
        }

        // Update previous positions and rotations
        headsetPreviousPosition = vrHeadset.position;
        controllerPreviousPosition = rightController.position;
        headsetPreviousRotation = vrHeadset.rotation;
    }
}
