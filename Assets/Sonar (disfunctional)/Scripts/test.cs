using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Microphone mic;
    // Start is called before the first frame update
     private AudioSource audioSource;
     string micDevice = "Microphone Array (Intel� Smart Sound Technology for Digital Microphones)";
     public int sampleWindow = 128;  // The number of samples to analyze at once
    public SNRPlayerController playerController;
    bool cooldown;
    public static test instance;

    public float micCooldown, micCutoff, inputMin, inputMax, outputMin, outputMax, intensityMultiplier = 1;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        micDevice = Microphone.devices[0];
        Debug.Log("Default Microphone: " + micDevice);

        // Get the attached AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Check if the device has any microphones connected
        if (Microphone.devices.Length > 0)
        {
            // Start recording from the default microphone
            audioSource.clip = Microphone.Start(micDevice, false, 3000, 44100);

            // Wait until the microphone starts recording
            while (!(Microphone.GetPosition(micDevice) > 0)) {}

            // Play the audio that was recorded
            //StartCoroutine(MonitorRecording());
            StartCoroutine(MonitorLoudness());
        }
        else
        {
            Debug.LogWarning("No microphone connected!");
        }
    }

    System.Collections.IEnumerator MonitorRecording()
    {
        while (Microphone.IsRecording(micDevice))
        {
            // Log real-time information about the audio clip
            LogClipInfo(audioSource.clip);

            // Wait for a second before logging again
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Recording stopped.");
    }

    void LogClipInfo(AudioClip clip)
    {
        if (clip != null)
        {
            int position = Microphone.GetPosition(micDevice);  // Get the current position in the recording buffer

            Debug.Log("Real-Time Clip Information:");
            Debug.Log("Frequency (Hertz): " + clip.frequency);
            Debug.Log("Channels: " + clip.channels);
            Debug.Log("Length (seconds): " + clip.length);
            Debug.Log("Samples: " + clip.samples);
            Debug.Log("Current Recording Position: " + position + " samples");
            Debug.Log("Recording Time (seconds): " + (float)position / clip.frequency);
        }
        else
        {
            Debug.LogWarning("AudioClip is null!");
        }
    }

    System.Collections.IEnumerator MonitorLoudness()
    {
        while (Microphone.IsRecording(micDevice))
        {
            // Log the loudness (volume level)
            float loudness = GetLoudnessFromMicrophone();
            float mult = intensityMultiplier;
            //Debug.Log("Loudness: " + loudness);
            if ((loudness > micCutoff) && !cooldown)
            {
                Debug.Log("Loud sound: " + loudness);
                float micVolume = MapRange(loudness, inputMin, inputMax, outputMin, outputMax);
                Debug.Log("mic volume = " + micVolume);

                if(loudness > -5f)
                {
                    mult *= 5;
                }

                playerController.TriggerSonar(micVolume * mult);
                cooldown = true;
                StartCoroutine(LoudSoundCooldown());
            }

            // Wait for a short time before analyzing again
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Recording stopped.");
    }

    float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // Map the value from the original range [fromMin, fromMax] to the target range [toMin, toMax]
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    float GetLoudnessFromMicrophone()
    {
        // Create a sample buffer
        float[] samples = new float[sampleWindow];

        // Get data from the audio source's clip (currently recording)
        audioSource.clip.GetData(samples, Microphone.GetPosition(micDevice) - sampleWindow);

        // Calculate the RMS (Root Mean Square) value of the samples to get loudness
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += samples[i] * samples[i];  // Square each sample
        }
        float rms = Mathf.Sqrt(sum / sampleWindow);  // Calculate the root mean square

        // Convert RMS to a decibel-like scale (logarithmic)
        float loudness = 20 * Mathf.Log10(rms / 0.1f);  // Normalize and scale to decibels

        return Mathf.Clamp(loudness, -80f, 0f);  // Clamp between -80 and 0 dB to avoid negative infinity
    }

    IEnumerator LoudSoundCooldown()
    {
        yield return new WaitForSeconds(micCooldown);
        cooldown = false;
    }
}