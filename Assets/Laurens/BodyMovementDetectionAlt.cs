using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BodyMovementDetectionAlt : MonoBehaviour
{
    public Transform vrHeadset; // Reference to the VR headset (camera)
    public Transform xrRig; // Reference to the XR Rig (player object)
    public Transform rightController; // Reference to the right controller
    public Transform leftController; // Reference to the left controller

    Vector3 velocity;
    public Vector3 axisIgnore;

    public float neckLength = .25f;
    public float extraSpeed;

    private Vector3 pivot;
    private Vector3 pivotPrevious;

    private Vector3 chainPosition;
    public float chainDistance = .5f;

    private Vector3 currentVelocity = Vector3.zero; // For smooth movement
    private Vector3 targetVelocity = Vector3.zero;

    private float motionConsistencyTimer = 0f;
    private const float motionConsistencyThreshold = 0.2f;

    void Start()
    {
        // Initialize previous positions
        if (vrHeadset == null || xrRig == null || rightController == null || leftController == null)
            return;
        pivot = (vrHeadset.position + rightController.position + leftController.position) / 3;
    }

    void Update()
    {
        if (vrHeadset == null || xrRig == null || rightController == null || leftController == null)
            return;


        // Calculate direction alignment using a smoothed dot product
        //float directionAlignment = Vector3.Dot(headsetVelocity.normalized, controllerVelocity.normalized);
        //float smoothedAlignment = Mathf.Lerp(0f, directionAlignment, 0.1f);

        // Check if both velocities exceed the speed threshold and are aligned
        /*if (headsetSpeed >= speedThreshold && controllerSpeed >= speedThreshold && smoothedAlignment >= directionAlignmentThreshold)
        {
            // Increment consistency timer
            motionConsistencyTimer += Time.deltaTime;

            if (motionConsistencyTimer >= motionConsistencyThreshold)
            {
                // Calculate the movement direction based on the headset's motion
                Vector3 movementDirection = headsetVelocity.normalized;

                // Apply additional speed and smooth movement
                Vector3 averagedVelocity = Vector3.Lerp(currentVelocity, movementDirection * additionalSpeed, smoothingFactor);
                targetVelocity = xrRig.position + averagedVelocity * Time.deltaTime;
                xrRig.position = Vector3.SmoothDamp(xrRig.position, targetVelocity, ref currentVelocity, smoothingFactor);
            }
        }
        else
        {
            // Reset the consistency timer if conditions are not met
            motionConsistencyTimer = 0f;
        }*/

        //xrPivot.position = xrPivot.position + (pivotVelocity.normalized * pivotSpeed) * pivotSpeed * additionalSpeed * Time.deltaTime;

        Vector3 headset = vrHeadset.localPosition;
        headset.y = 0;
        if (pivotPrevious == Vector3.zero) pivotPrevious = headset;

        if (Vector3.Distance(headset, chainPosition) > chainDistance)
        {
            //Speed of forward direction of vrHeadset.
            // Calculate the displacement vector
            Vector3 displacement = headset - pivotPrevious;

            // Project the displacement onto the forward direction of the VR headset
            Vector3 forwardDirection = vrHeadset.forward;
            forwardDirection.y = 0; // Ensure no vertical component
            forwardDirection.Normalize();

            // Calculate the forward velocity
            float forwardSpeed = Vector3.Dot(displacement, forwardDirection) / Time.deltaTime;

            // Apply the forward speed to the velocity vector
            velocity = forwardDirection * forwardSpeed;

            var offset = forwardSpeed * vrHeadset.forward;
            offset.y = 0;
            vrHeadset.parent.position += velocity * extraSpeed * Time.deltaTime;

            chainPosition = headset - (headset - chainPosition).normalized * chainDistance;

            Debug.Log(chainPosition);
        }

        pivotPrevious = headset;

        // Debug information for testing
        //Debug.Log($"Alignment: {smoothedAlignment}, HeadsetSpeed: {headsetSpeed}, ControllerSpeed: {controllerSpeed}");

        // Update previous positions
        //pivotPrevious = pivot;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(chainPosition, chainDistance);
    }
}
