using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool enableVRMode;
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera playerCamera, vrCamera;

    // Start is called before the first frame update
    void Start()
    {
        if(enableVRMode)
        {
            playerController.enabled = false;
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
