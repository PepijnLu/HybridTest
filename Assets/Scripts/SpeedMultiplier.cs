using UnityEngine;

public class SpeedMultiplier : MonoBehaviour
{
    [Range(1f, 100f)]
    public float speedMultiplier;
    private CharacterController characterController;
    private Vector3 lastPostion;
    private float currentSpeed;



    void Start()
    {
        lastPostion = transform.position;
        if(characterController == null)
            characterController = gameObject.GetComponent<CharacterController>();    
    }

    void Update()
    {
        Vector3 movement = transform.position - lastPostion;
        currentSpeed = movement.magnitude / Time.deltaTime;

        Debug.Log($"Current Speed: {currentSpeed}");

        // Apply speed modifier
        ApplySpeedModifier(movement);

        // Update last position
        lastPostion = transform.position;
    }

    private void ApplySpeedModifier(Vector3 movement) 
    {
        float additionalSpeed = currentSpeed * (speedMultiplier - 1f);
        characterController.Move(movement * additionalSpeed);
    }
}
