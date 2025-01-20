using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtendCamera : MonoBehaviour
{
    public Transform xrOriginRig, camPos2;
    bool isCameraExtended, isCameraPortaled, isMoving, isRotated;
    //public float duration = 0.2f, moveDistance = 1f;
    public float camMoveSpeed = 5f, camSmoothTime = 0.1f, camMoveTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        // windowCamStartLoc = usableWindowCam.transform.position;
        // usableWindowCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKey(KeyCode.E))
       {
            ExtendPlayerCamera();
       }
    //    else
    //    {
    //         transform.position = player.transform.position;
    //         isCameraExtended = false;

    //         if(isRotated)
    //         {
    //             Quaternion currentRotation = transform.rotation;
    //             Quaternion yRotation = Quaternion.Euler(0, 90, 0);

    //             transform.rotation = yRotation * currentRotation;
    //             gameObject.transform.SetParent(player.transform);
    //             isRotated = false;
    //         }

    //         // usableWindowCam.enabled = false;
    //         // originalPlayerCam.enabled = true;
    //         playerController.playerCamera = originalPlayerCam.transform;
    //         StopCoroutine(MoveForwardCoroutine());
    //    } 
    }

    void ExtendPlayerCamera()
    {
        if(!isCameraExtended && !isMoving)
        {
            // playerCam.transform.position += transform.forward * 1;
            StartCoroutine(MoveForwardCoroutine());
            isCameraExtended = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "WindowPortal")
        {
            Debug.Log("Collision");

            Vector3 offset = transform.position - player.transform.position;
            gameObject.transform.SetParent(null);

            transform.position = camPos2.transform.position;

            Quaternion currentRotation = transform.rotation;
            Quaternion yRotation = Quaternion.Euler(0, 0, 0);

            if(SceneManager.GetActiveScene().name != "Museum") {yRotation = Quaternion.Euler(0, 180, 0);}
            else yRotation = Quaternion.Euler(0, -90, 0);

            //transform.rotation = yRotation * currentRotation;
            isRotated = true;

            Vector3 targetDestination = transform.position - offset;
            player.transform.position = targetDestination;
            gameObject.transform.SetParent(player.transform);

            // usableWindowCam.enabled = true;
            // originalPlayerCam.enabled = false;
            // playerController.playerCamera = usableWindowCam.transform;

        }

    }

    private IEnumerator MoveForwardCoroutine()
    {
        //gameObject.transform.SetParent(null);
        isMoving = true;
        float duration = 0f;
        while(duration < camMoveTime)
        {
            Vector3 targetPosition = transform.position + transform.forward * camMoveSpeed;
            Vector3 velocity = Vector3.zero;

            // Smoothly move the object to the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, camSmoothTime);
            isMoving = false;
            duration += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isMoving = false;
        isCameraExtended = true;
    }
}
