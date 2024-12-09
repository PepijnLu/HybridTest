using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    Dictionary<AudioSource, SimpleSonarShader_Object> audioShaders = new Dictionary<AudioSource, SimpleSonarShader_Object> ();

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioSource audioSource, float volume, Collision collision)
    {
        if (audioSource)
        {
            if (audioSource.gameObject.layer != 3)
            {
                audioSource.volume = volume * 5;
                audioSource.Play();
            }
            
            if (audioShaders.ContainsKey(audioSource))
            { 
                if (collision.contacts.Length > 0)
                {
                    Debug.Log("array length: " + collision.contacts.Length);
                    SimpleSonarShader_Object shaderObj = audioShaders[audioSource];
                    shaderObj.StartSonarRing(collision.contacts[0].point, volume);
                }
                else
                {
                    Debug.Log("Collision array is 0");
                }
            }
            else
            {
                if (audioSource.gameObject.TryGetComponent(out SimpleSonarShader_Object shaderObj))
                {
                    audioShaders.Add(audioSource, shaderObj);
                    PlaySound(audioSource, volume, collision);
                    Debug.Log(audioSource.gameObject.name + " added to dictionary");
                }
                else
                {
                    Debug.Log("No shader component found on: " + audioSource.gameObject.name);
                }

            }
        }
    }

}
