using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The pivot point around which the object rotates.")]
    public Transform pivotPoint1, pivotPoint2, door1, door2;

    [Range(270f, 360f)]
    [Tooltip("The rotation angle in degrees.")]
    public float sliderValue;

    private float currentAngle1 = 0f, currentAngle2 = 0f;

    private void Update()
    {
        if (pivotPoint1 != null && pivotPoint2 != null)
        {
            RotateToAngle();
        }
    }

    private void RotateToAngle()
    {
        // Calculate the target rotation based on the slider value
        float sliderValue1 = sliderValue;
        float sliderValue2 = 360 - sliderValue;

        float angleToRotate1 = sliderValue - currentAngle1;
        float angleToRotate2 = sliderValue2 - currentAngle2;

        // Apply the rotation incrementally
        Quaternion rotation1 = Quaternion.Euler(0f, angleToRotate1, 0f);
        Quaternion rotation2 = Quaternion.Euler(0f, angleToRotate2, 0f);

        door1.position = rotation1 * (door1.position - pivotPoint1.position) + pivotPoint1.position;
        door1.rotation = rotation1 * door1.rotation;

        door2.position = rotation2 * (door2.position - pivotPoint2.position) + pivotPoint2.position;
        door2.rotation = rotation2 * door2.rotation;

        // Update the current angle to match the slider value
        currentAngle1 = sliderValue1;
        currentAngle2 = sliderValue2;
    }
}