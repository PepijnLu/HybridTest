using Unity.Mathematics;
using UnityEngine;

public class XRHead : MonoBehaviour
{
    public Vector3 boxHalfExtents = new(1f, 1f, 1f);
    private readonly XRHand[] hands = new XRHand[2];
    private readonly Collider[] results = new Collider[2];

    void Update()
    {
        CheckCollision();

        if(hands.Length == 2) 
        {
            Debug.Log("Found both hands, do something...");
        }  
    }

    private void CheckCollision() 
    {
        int count = Physics.OverlapBoxNonAlloc
        (
            transform.position,
            boxHalfExtents,
            results,
            quaternion.identity
        );

        for (int i = 0; i < count; i++)
        {
            Debug.Log($"Detected: {results[i].gameObject.name}");

            if(results[i].gameObject.TryGetComponent<XRHand>(out var xrHand)) 
            {
                // hands.Add(xrHand);
                AddHandToArray(xrHand);
            }
        }
    }

    private void AddHandToArray(XRHand xrHand)
    {
        for (int i = 0; i < hands.Length; i++)
        {
            if(hands[i] == xrHand) return;
        }

        for (int i = 0; i < hands.Length; i++)
        {
            if(hands[i] == null) 
            {
                hands[i] = xrHand;
                Debug.Log($"Added hand: {xrHand.name} to slot {i}");
                return;
            }
        }

        Debug.LogWarning("Hand array is full, could not add hand.");
    }
}
