// using UnityEngine;

// public class CarMovement : MonoBehaviour
// {
//     public Transform vrHeadset; // Reference to the VR headset (camera)
//     public Transform car; // Reference to the XR Rig (player object)
//     public Transform rightController; // Reference to the right controller

//     public float VRspeedThreshold = 0.25f, controllerSpeedThreshold = 0.35f; // Speed thresholds
//     public float directionAlignmentThreshold = 0.95f; // Cosine similarity threshold for direction alignment
//     public float additionalSpeed = 20f; // Additional movement speed multiplier
//     public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

//     // public float rotationThreshold = 10f; // Threshold for detecting significant rotation (in degrees per frame)
//     public float rotationSensitivity = 0.1f; // Sensitivity for detecting small rotations
//     public float controllerSpeed;
//     public float headsetSpeed;

//     private Vector3 headsetPreviousPosition;
//     private Vector3 controllerPreviousPosition;
//     private Quaternion headsetPreviousRotation; // Store the previous rotation of the headset
//     private Vector3 currentVelocity = Vector3.zero;
//     private Vector3 targetVelocity = Vector3.zero;

//     private Vector3 lastValidMovementDirection = Vector3.zero;

//     public bool isMoving;
//     public Transform cameraOffset, cameraPositionInsideCar;
//     public float offset;
//     public Vector3 carOffset;

//     void Start()
//     {
//         if (vrHeadset != null)
//         {
//             headsetPreviousPosition = vrHeadset.position;
//             headsetPreviousRotation = vrHeadset.rotation; // Initialize the previous rotation
//         }

//         if (rightController != null)
//             controllerPreviousPosition = rightController.position;

//         cameraOffset.SetPositionAndRotation(
//             new Vector3(cameraPositionInsideCar.position.x, cameraPositionInsideCar.position.y + offset, cameraPositionInsideCar.position.z), 
//             cameraPositionInsideCar.rotation
//         );
//     }

//     void Update()
//     {
//         if (vrHeadset == null || car == null || rightController == null)
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


//         // Calculate direction alignment using dot product
//         float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

//         // Check if both velocities exceed the speed threshold and are aligned
//         if (headsetSpeed >= VRspeedThreshold && controllerSpeed >= controllerSpeedThreshold && directionAlignment >= directionAlignmentThreshold)
//         {
//             // Calculate the movement direction based on the headset's motion
//             Vector3 movementDirection = headsetVelocity.normalized;

//             lastValidMovementDirection = movementDirection;

//             // Apply additional speed
//             targetVelocity = car.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;

//             // Smooth movement to avoid jitter
//             car.position = Vector3.SmoothDamp(car.position, targetVelocity, ref currentVelocity, smoothingFactor);
        
//             car.SetPositionAndRotation(vrHeadset.position + carOffset, Quaternion.Slerp(car.rotation, vrHeadset.rotation, Time.deltaTime * rotationSensitivity));
//             isMoving = true;
//         }
//         else
//         {
//             // Decay movement direction to stop smoothly
//             lastValidMovementDirection = Vector3.Lerp(lastValidMovementDirection, Vector3.zero, smoothingFactor);
//             targetVelocity = car.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;
//             car.position = Vector3.SmoothDamp(car.position, targetVelocity, ref currentVelocity, smoothingFactor);

//             car.SetPositionAndRotation(vrHeadset.position + carOffset, Quaternion.Slerp(car.rotation, vrHeadset.rotation, Time.deltaTime * rotationSensitivity));
//             isMoving = false;
//         }

//         // Update previous positions and rotations
//         headsetPreviousPosition = vrHeadset.position;
//         controllerPreviousPosition = rightController.position;
//         headsetPreviousRotation = vrHeadset.rotation;
//     }
// }



using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Transform vrHeadset; // Reference to the VR headset (camera)
    public Transform car; // Reference to the XR Rig (player object)
    public Transform rightController; // Reference to the right controller
    public Transform xr_rig;

    public float VRspeedThreshold = 0.25f, controllerSpeedThreshold = 0.35f; // Speed thresholds
    public float directionAlignmentThreshold = 0.95f; // Cosine similarity threshold for direction alignment
    public float additionalSpeed = 20f; // Additional movement speed multiplier
    public float smoothingFactor = 0.1f; // Smoothing factor to avoid jitter

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
    public Transform cameraOffset, cameraPositionInsideCar;
    public float offset;
    public Vector3 carOffset;

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
        if (vrHeadset == null || car == null || rightController == null)
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

        // Calculate direction alignment using dot product
        float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

        // Check if both velocities exceed the speed threshold and are aligned
        if (headsetSpeed >= VRspeedThreshold && controllerSpeed >= controllerSpeedThreshold && directionAlignment >= directionAlignmentThreshold)
        {
            // Calculate the movement direction based on the headset's motion
            Vector3 movementDirection = headsetVelocity.normalized;

            lastValidMovementDirection = movementDirection;

            // Apply additional speed
            targetVelocity = car.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;

            // Smooth movement to avoid jitter
            car.position = Vector3.SmoothDamp(car.position, targetVelocity, ref currentVelocity, smoothingFactor);

            // Update the car position based on headset's position + the car offset
            car.position = vrHeadset.position + carOffset;

            // Only update the Y-axis of the car's rotation to match the headset's Y-axis rotation
            Vector3 headsetEulerAngles = vrHeadset.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0f, headsetEulerAngles.y, 0f); // Only use Y-axis rotation
            car.rotation = Quaternion.Slerp(car.rotation, targetRotation, Time.deltaTime * rotationSensitivity);

            isMoving = true;
        }
        else
        {
            // Decay movement direction to stop smoothly
            lastValidMovementDirection = Vector3.Lerp(lastValidMovementDirection, Vector3.zero, smoothingFactor);
            targetVelocity = car.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;
            car.position = Vector3.SmoothDamp(car.position, targetVelocity, ref currentVelocity, smoothingFactor);

            // Update the car position based on headset's position + the car offset
            car.position = vrHeadset.position + carOffset;

            // Only update the Y-axis of the car's rotation to match the headset's Y-axis rotation
            Vector3 headsetEulerAngles = vrHeadset.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0f, headsetEulerAngles.y, 0f); // Only use Y-axis rotation
            car.rotation = Quaternion.Slerp(car.rotation, targetRotation, Time.deltaTime * rotationSensitivity);

            isMoving = false;
        }

        // Update previous positions and rotations
        headsetPreviousPosition = vrHeadset.position;
        controllerPreviousPosition = rightController.position;
        headsetPreviousRotation = vrHeadset.rotation;
    }
}
