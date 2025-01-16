using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource muffledAudioSource, backgroundAudioSource;
    public Transform playerTransform;
    private AudioLowPassFilter lowPassFilter;
    public float cutoffFrequency = 500f;  // Lowpass cutoff frequency in Hz
    public float resonanceQ = 1f;         // Resonance (Q factor)
    public float audioPlayRange;
    bool audioSourcePlayed;

    [SerializeField] List<Vector3> paintingLocations;
    [SerializeField] List<AudioSource> paintingAudioSources;
    List<AudioSource> audiosPlayed = new();

    void Start()
    {
        lowPassFilter = muffledAudioSource.gameObject.AddComponent<AudioLowPassFilter>();

        // Apply the lowpass filter settings
        lowPassFilter.cutoffFrequency = cutoffFrequency;
        lowPassFilter.lowpassResonanceQ = resonanceQ;
    }

    void Update()
    {
        // You can dynamically change the cutoff frequency if needed
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cutoffFrequency += 10f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cutoffFrequency -= 10f;
        }

        // Apply the updated cutoff frequency
        lowPassFilter.cutoffFrequency = Mathf.Clamp(cutoffFrequency, 20f, 22000f);

        Vector3 differenceBetweenPaintingAndPlayer = muffledAudioSource.transform.position - playerTransform.position;
        //Debug.Log(differenceBetweenPaintingAndPlayer);

        for(int i = 0; i < paintingAudioSources.Count; i++)
        {
            float difference = (paintingLocations[i] - playerTransform.position).magnitude;
            if(i == 1) Debug.Log("Difference to painting 1 = " + difference);
            if(difference <= audioPlayRange)
            {
                if(!audiosPlayed.Contains(paintingAudioSources[i])) 
                {
                    Debug.Log($"Audio Playing: {paintingAudioSources[i].name}. Range: {difference}");
                    audiosPlayed.Add(paintingAudioSources[i]);
                    paintingAudioSources[i].Play();
                }

            }
        }
        muffledAudioSource.volume = (100 - (differenceBetweenPaintingAndPlayer.z * 33.34f)) / 100;
    }
}
