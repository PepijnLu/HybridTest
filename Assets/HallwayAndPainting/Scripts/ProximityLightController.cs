using UnityEngine;

[System.Serializable]
public class LightTargetPair
{
    public Light light; // The light to control
    public Transform target; // The GameObject the light reacts to
    public float minDistance = 0f; // Distance where intensity is max
    public float maxDistance = 10f; // Distance where intensity is zero
    public float maxIntensity = 5f; // Maximum light intensity
    public float minIntensity = 0f; // Minimum light intensity
}

public class ProximityLightController : MonoBehaviour
{
    [Header("Light and Target Settings")]
    public LightTargetPair[] lightTargetPairs; // Array of light-target configurations

    void Update()
    {
        foreach (var pair in lightTargetPairs)
        {
            if (pair.light != null && pair.target != null)
            {
                float distance = Vector3.Distance(pair.light.transform.position, pair.target.position);
                float normalizedDistance = Mathf.Clamp01((distance - pair.minDistance) / (pair.maxDistance - pair.minDistance));
                pair.light.intensity = Mathf.Lerp(pair.maxIntensity, pair.minIntensity, normalizedDistance);
            }
        }
    }
}
