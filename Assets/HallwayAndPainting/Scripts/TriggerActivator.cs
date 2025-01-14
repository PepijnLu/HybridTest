using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject targetObject; // The specific GameObject to detect

    [Header("Activation Settings")]
    public GameObject[] objectsToActivate; // Array of GameObjects to activate
    public bool deactivateOnExit = false; // Option to deactivate the objects when exiting the trigger

    [Header("Sound Settings")]
    public AudioClip activationSound; // Sound to play on trigger enter
    public AudioClip deactivationSound; // Sound to play on trigger exit
    public AudioSource audioSource; // AudioSource to play the sound

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            // Activate all objects
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }

            // Play activation sound
            if (audioSource != null && activationSound != null)
            {
                audioSource.PlayOneShot(activationSound);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (deactivateOnExit && other.gameObject == targetObject)
        {
            // Deactivate all objects
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            // Play deactivation sound
            if (audioSource != null && deactivationSound != null)
            {
                audioSource.PlayOneShot(deactivationSound);
            }
        }
    }
}
