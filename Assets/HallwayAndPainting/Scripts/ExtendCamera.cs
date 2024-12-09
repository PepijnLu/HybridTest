using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendCamera : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject player;
    public Transform camPos1, camPos2;
    public Camera usableWindowCam, originalPlayerCam;
    bool isCameraExtended, isCameraPortaled, isMoving, isRotated;
    //public float duration = 0.2f, moveDistance = 1f;
    public float camMoveSpeed = 5f, camSmoothTime = 0.1f, camMoveTime = 0.5f;
    Vector3 windowCamStartLoc;

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
       else
       {
            transform.position = player.transform.position;
            isCameraExtended = false;

            if(isRotated)
            {
                Quaternion currentRotation = transform.rotation;
                Quaternion yRotation = Quaternion.Euler(0, 180, 0);

                transform.rotation = yRotation * currentRotation;
                isRotated = false;
            }

            // usableWindowCam.enabled = false;
            // originalPlayerCam.enabled = true;
            playerController.playerCamera = originalPlayerCam.transform;
            StopCoroutine(MoveForwardCoroutine());
       } 
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
            transform.position = camPos2.transform.position;

            Quaternion currentRotation = transform.rotation;
            Quaternion yRotation = Quaternion.Euler(0, 180, 0);

            transform.rotation = yRotation * currentRotation;
            isRotated = true;

            // usableWindowCam.enabled = true;
            // originalPlayerCam.enabled = false;
            // playerController.playerCamera = usableWindowCam.transform;

        }

    }

    private IEnumerator MoveForwardCoroutine()
    {
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
