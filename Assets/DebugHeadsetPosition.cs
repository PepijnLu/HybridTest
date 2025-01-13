using UnityEngine;

public class DebugHeadsetPosition : MonoBehaviour
{
    public Transform headset;

    void Update()
    {
        if (headset == null)
        {
            Debug.LogError("Headset not assigned.");
            return;
        }

        Debug.Log($"Headset World Position: {headset.position}");
        Debug.Log($"Headset Local Position: {headset.localPosition}");
    }
}
