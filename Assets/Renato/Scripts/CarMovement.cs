using System.Collections;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Transform vrHeadset; // Reference to the VR headset (camera)
    public Transform car; // Reference to the car
    public Transform rightController; // Reference to the right controller
    public Transform cameraOffset; // Parent of the VR headset
    public Transform fixedPosition; // A fixed reference position in the scene

    public float VRspeedThreshold = 0.25f, controllerSpeedThreshold = 0.35f;
    public float directionAlignmentThreshold = 0.95f;
    public float additionalSpeed = 20f;
    public float smoothingFactor = 0.1f;
    public float rotationSensitivity = 0.1f;

    public float controllerSpeed;
    public float headsetSpeed;

    private Vector3 headsetPreviousPosition;
    private Vector3 controllerPreviousPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 lastValidMovementDirection = Vector3.zero;
    public Vector3 carOffset; //Values without rotating mechanic: (2.92000008, -1.13999999, 0.409999996) // Values with rotating mechanic: (-2.83999991,-1.13999999,-1.29999995) 

    // public int rotateAngle_car, rotateAngle_CamOffset, offset;
    private bool isCoroutineRunning;
    // public Transform frontBumperPivot; // The pivot point at the front bumper

    void Start()
    {
        if (vrHeadset != null)
        {
            headsetPreviousPosition = vrHeadset.position;
        }

        if (rightController != null)
        {
            controllerPreviousPosition = rightController.position;
        }

        // Align the car to the cameraOffset at the start
        AlignCarToCameraOffset();

        // Ensure cameraOffset starts at the correct position based on fixedPosition + offset
        SetCameraPosition();

        StartCoroutine(SetCameraAtRightPosition());
    }

    private void SetCameraPosition()
    {
        // Ensure cameraOffset position is set correctly
        cameraOffset.position = fixedPosition.position;
    }

    private IEnumerator SetCameraAtRightPosition()
    {
        isCoroutineRunning = true;

        // Align the cameraOffset rotation if needed
        cameraOffset.rotation = fixedPosition.rotation;

        yield return new WaitForFixedUpdate();

        isCoroutineRunning = false;
    }

    private void AlignCarToCameraOffset()
    {
        // Align the car to the cameraOffset's rotation
        car.position = vrHeadset.position + carOffset;

        // Align the car’s rotation with the cameraOffset
        Quaternion targetRotation = Quaternion.Euler(0f, cameraOffset.rotation.y, 0f);
        car.rotation = targetRotation;

        Debug.Log($"Initial Alignment -> Car Rotation: {car.rotation.eulerAngles}, CameraOffset Rotation: {cameraOffset.rotation.eulerAngles}");
    }

    void Update()
    {
        if (vrHeadset == null || car == null || rightController == null || isCoroutineRunning)
            return;

        // Calculate movement velocities
        Vector3 headsetVelocity = (vrHeadset.position - headsetPreviousPosition) / Time.deltaTime;
        Vector3 controllerVelocity = (rightController.position - controllerPreviousPosition) / Time.deltaTime;

        headsetVelocity.y = 0;
        controllerVelocity.y = 0;

        headsetSpeed = headsetVelocity.magnitude;
        controllerSpeed = controllerVelocity.magnitude;

        if (headsetSpeed < VRspeedThreshold) headsetSpeed = 0f;
        if (controllerSpeed < controllerSpeedThreshold) controllerSpeed = 0f;

        float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);

        if (headsetSpeed >= VRspeedThreshold && controllerSpeed >= controllerSpeedThreshold && directionAlignment >= directionAlignmentThreshold)
        {
            Vector3 movementDirection = headsetVelocity.normalized;
            lastValidMovementDirection = movementDirection;

            targetVelocity = car.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;

            car.position = Vector3.SmoothDamp(car.position, targetVelocity, ref currentVelocity, smoothingFactor);
            car.position = vrHeadset.position + carOffset;

            // Align the car's rotation to the VR headset (camera)
            // Vector3 headsetEulerAngles = vrHeadset.localEulerAngles;
            // Quaternion targetRotation = Quaternion.Euler(0f, headsetEulerAngles.y, 0f);
            // car.rotation = Quaternion.Slerp(car.rotation, targetRotation, Time.deltaTime * rotationSensitivity);
        }
        else
        {
            lastValidMovementDirection = Vector3.Lerp(lastValidMovementDirection, Vector3.zero, smoothingFactor);
            targetVelocity = car.position + additionalSpeed * Time.deltaTime * lastValidMovementDirection;
            car.position = Vector3.SmoothDamp(car.position, targetVelocity, ref currentVelocity, smoothingFactor);

            car.position = vrHeadset.position + carOffset;

            // Maintain car rotation based on VR headset rotation
            // Vector3 headsetEulerAngles = vrHeadset.localEulerAngles;
            // Quaternion targetRotation = Quaternion.Euler(0f, headsetEulerAngles.y, 0f);
            // car.rotation = Quaternion.Slerp(car.rotation, targetRotation, Time.deltaTime * rotationSensitivity);
        }

        headsetPreviousPosition = vrHeadset.position;
        controllerPreviousPosition = rightController.position;
    }
}
