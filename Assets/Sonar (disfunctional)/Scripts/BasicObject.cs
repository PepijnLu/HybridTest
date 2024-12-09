using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BasicObject : MonoBehaviour, IThrowable
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionImpulse = collision.impulse;
        // if (collisionImpulse.magnitude > 5)
        // {
            Debug.Log("ball collision");
            AudioManager.instance.PlaySound(audioSource, collisionImpulse.magnitude / 50, collision);
        //}
    
        // Output the impulse vector magnitude
        Debug.Log("Collision impulse: " + collisionImpulse.magnitude);
    }
}
