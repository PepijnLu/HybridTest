using UnityEngine;

public class ClapBoard : MonoBehaviour
{
    public GameObject clapBoard;
    public Transform hingePoint; // Hinge point (pivot) for rotation
    public Transform targetPivot; // Target rotation for the hinge
    public float rotationSpeed = 20.0f; // Speed of the rotation

    private bool shouldClose = false;
    private Quaternion initialRotation; // Original rotation of the hinge

    private void Start()
    {
        // Store the initial rotation of the hinge point
        if (hingePoint != null)
        {
            initialRotation = hingePoint.rotation;
        }
    }

    public void Poke(bool _bool)
    {
        // Trigger rotation if the boolean is true
        shouldClose = _bool;
    }

    private void Update()
    {
        if (shouldClose && hingePoint != null)
        {
            // Smoothly rotate the hinge point to align with the target pivot
            hingePoint.rotation = Quaternion.RotateTowards(
                hingePoint.rotation,
                targetPivot.rotation,
                rotationSpeed * Time.deltaTime
            );

            // Check if the rotation is complete
            if (Quaternion.Angle(hingePoint.rotation, targetPivot.rotation) < 0.1f)
            {
                hingePoint.rotation = targetPivot.rotation; // Snap to final rotation
                shouldClose = false; // Stop rotating
            }
        }
    }

    public void ResetClapboard()
    {
        // Reset the hinge back to its original rotation
        if (hingePoint != null)
        {
            hingePoint.rotation = initialRotation;
        }
    }
}
