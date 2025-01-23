using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

namespace Assets.Renato.Scripts 
{
    public class Chair : MonoBehaviour
    {
        public XROrigin xROrigin;
        public GameObject painting;
        public float distanceFromPainting = 2f;
        
        [SerializeField] private Camera cam;
        [SerializeField] private Transform childObj;
        [SerializeField] TrackedPoseDriver trackedPoseDriver;
        

        [SerializeField] private MeshRenderer fadeQuadRenderer;
        [SerializeField] private float fadeDuration = 1f;
        private Material fadeMaterial;	
        
        public bool isCloseBy;

        private Collision collisionScript;
        public float cameraZoomDuration = 2f;

        public Management management;
        private BoxCollider boxCollider;
        
        void Awake()
        {
            if(childObj == null) 
            {
                childObj = transform.GetChild(0);
            }

            collisionScript = GetComponentInChildren<Collision>();
            Destroy(collisionScript);

            // Turn on the xr stuff
        
            boxCollider = GetComponent<BoxCollider>();
        }

        void Start() 
        {
            // Get the material from the quad
            fadeMaterial = fadeQuadRenderer.material;

            // Ensure the quad starts fully transparent
            SetQuadAlpha(0f);
        }

        void Update()
        {
            if(collisionScript.playerSitDown) 
            {
                // Store the world position of the xr rig
                management.lastWorldPosition = cam.transform.position;

                // Disable the collider
                boxCollider.enabled = false;

                StartCoroutine(Transition(1f, cameraZoomDuration, painting));

                Destroy(collisionScript);
            }
        }

        void OnTriggerEnter(Collider collider) 
        {
            if(collider.CompareTag("Player")) 
            {
                if(!isCloseBy) 
                {
                    isCloseBy = true;
                    Debug.Log("IsCloseBy");
                    
                    // Visual effect...
                    StartCoroutine(SomeVisualEffect());
                }
            }
        }

        void OnTriggerExit(Collider collider) 
        {
            if(collider.CompareTag("Player")) 
            {
                if(isCloseBy) 
                {
                    isCloseBy = false;

                    // Stop the visual effect coroutine if it's still running
                    StopCoroutine(SomeVisualEffect());
                }
            }
        }

        private IEnumerator SomeVisualEffect() 
        {
            Debug.Log("Some visual effect...");
            yield break;
        } 

        
        private IEnumerator FixedCamPosition(GameObject painting)
        {
            Debug.Log("FixedCameraPosition Coroutine Started...");

            // Stop the movement of the rig
            xROrigin.enabled = false;

            // Disable the tracking component on the cam
            trackedPoseDriver.enabled = false;

            float rotationSpeed = 1f;

            // Fetch the position of the painting
            Vector3 directionToPainting = painting.transform.position - cam.transform.position;

            // Get the desired rotation to face the painting
            Quaternion targetRotation = Quaternion.LookRotation(directionToPainting);

            // While the camera isn't facing the painting
            while (Quaternion.Angle(cam.transform.rotation, targetRotation) > 0.1f)
            {
                // Smoothly rotate the camera to face the painting
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Fetch the updated position and target rotation to ensure accuracy during movement
                directionToPainting = painting.transform.position - cam.transform.position;
                targetRotation = Quaternion.LookRotation(directionToPainting);

                yield return null; // Wait for the next frame
            }

            Debug.Log("FixedCameraPosition Coroutine Completed.");
            
            yield break;
        }
 
        private IEnumerator CameraZoomIn(float duration)
        {
            Debug.Log("CameraZoomIn Coroutine Started...");

            Vector3 startPosition = cam.transform.position;
            Vector3 paintingPosition = painting.transform.position;

            // Direction toward the painting (adjusting for camera's orientation)
            Vector3 directionToPainting = (paintingPosition - startPosition).normalized;

            // Target position: stop at a specific distance from the painting
            Vector3 targetPosition = paintingPosition - directionToPainting * distanceFromPainting;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // Interpolation factor
                float t = elapsedTime / duration;

                // Interpolate the camera's position using its forward direction
                cam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                // Debug log to verify the camera's movement
                Debug.Log($"Camera Position: {cam.transform.position}, Target: {targetPosition}");

                // Check if the camera is close enough to stop
                float distanceToTarget = Vector3.Distance(cam.transform.position, targetPosition);
                if (distanceToTarget <= 0.01f) // Small threshold to prevent overshooting
                {
                    Debug.Log("Camera has reached the target position.");
                    cam.transform.position = targetPosition; // Ensure it snaps to the exact position

                    yield return new WaitForSeconds(1f);
                    
                    xROrigin.enabled = true;
                    trackedPoseDriver.enabled = true;

                    yield break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the camera reaches the exact target position at the end
            cam.transform.position = targetPosition;
            Debug.Log("CameraZoomIn Coroutine Completed.");
        }

        private IEnumerator Transition(float waitDuration, float transitionDuration, GameObject painting) 
        {
            Debug.Log("Transition Coroutine Started...");
        
            yield return StartCoroutine(FixedCamPosition(painting));

            yield return new WaitForSeconds(waitDuration);

            yield return StartCoroutine(CameraZoomIn(transitionDuration));

            // Delete the chair
            // Destroy(gameObject);
            yield return StartCoroutine(Fade("CarScene", 255f));

            yield break;
            
        }

        // Coroutine to fade the quad in or out
        public IEnumerator Fade(string nextSceneName, float targetAlpha)
        {
            float startAlpha = fadeMaterial.color.a;
            float elapsedTime = 0f;
            float newAlpha = 0;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

                SetQuadAlpha(newAlpha);


                yield return null;
            }

            // Ensure the final alpha is set
            SetQuadAlpha(targetAlpha);

            if(newAlpha == targetAlpha) 
            {
                SceneManager.LoadScene(nextSceneName);
            }

        }

        // Helper method to set the quad's alpha value
        private void SetQuadAlpha(float alpha)
        {
            Color color = fadeMaterial.color;
            color.a = alpha;
            fadeMaterial.color = color;
        }
    }
}


#region OldCode
    // private void FixedCamPosition(GameObject painting) 
    // {
    //     Debug.Log("FixedCameraPosition Function Executed...");
        
    //     // Stop the movement of the rig
    //     Collision collisionScript = childObj.GetComponent<Collision>();
        
    //     collisionScript.xROrigin.enabled = false;
        
    //     // Disable the tracking component on the cam
    //     trackedPoseDriver.enabled = false;

    //     float rotationSpeed = 1f;
        
    //     // Fetch the position of the painting
    //     Vector3 directionToPainting = painting.transform.position - cam.transform.position;  

    //     // Get the desired rotation to face the painting
    //     Quaternion targetRotation = Quaternion.LookRotation(directionToPainting);
        
    //     // If the camera isn't facing the painting
    //     float angleDifference = Quaternion.Angle(cam.transform.rotation, targetRotation);

    //     if(angleDifference > .1f) 
    //     {
    //         // Smoothly rotate the camera to face the painting
    //         cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //     }
    // }

    // private IEnumerator AfterTransition(string nextSceneName) 
    // {
    //     Debug.Log("AfterTransition Coroutine Started...");
    //     // Fade out to black
    //     yield return StartCoroutine(Fade(nextSceneName, .25f));

    //     // Load the next scene
    //     // SceneManager.LoadScene(nextSceneName);

    //     // Wait for the scene to load
    //     // yield return null;

    //     // // Fade in from black
    //     // yield return StartCoroutine(Fade(0f));

    //     yield break;
    // }

    // private bool PlayerSitDown() 
    // {
    //     Debug.Log("PlayerSitDown Function Executed...");
    //     if(isCloseBy) 
    //     {
    //         // Store the colliders in the array
    //         Collider[] colliders = Physics.OverlapSphere(childObj.position, colliderRadius);
            
    //         if(colliders.Length > 0) // Check if there are any colliders
    //         {
    //             Debug.Log($"Colliders length: {colliders.Length}");

    //             foreach (var collider in colliders) 
    //             {
    //                 if(collider.CompareTag("Player")) 
    //                 {
    //                     Debug.Log($"Collider name: {collider.gameObject.name} + PlayerMovementDetection: {PlayerMovementDetection.gameObject.name}");
    //                     if(collider.gameObject.name == PlayerMovementDetection.gameObject.name) // Double check
    //                     {
    //                         Debug.Log("The same Player");

    //                         XROrigin = collider.GetComponent<XROrigin>(); // Fetch the XROrigin script

    //                         float timer = 0f;

    //                         if(/*PlayerMovementDetection.headsetSpeed <= .05f && PlayerMovementDetection.controllerSpeed <= .05f*/PlayerMovementDetection.xrRig.position.magnitude < 0.01f) 
    //                         {
    //                             timer += Time.deltaTime;

    //                             if(timer >= 2f && !isPlayerSitDown) 
    //                             {
    //                                 isPlayerSitDown = true;
    //                                 Debug.Log("There is no movement detected after the player collided with the chair...");
    //                                 return true;
    //                             } 
    //                         }
    //                     }
    //                 }
    //                 else{ Debug.Log("Player is not found"); return false; }  
    //             }
    //         } 
    //     }

    //     return false;
    // }


    // private IEnumerator CameraZoomIn(float duration)
    // {
    //     Debug.Log("CameraZoomIn Coroutine Started...");
    //     Vector3 startPosition = cam.transform.position;
    //     Vector3 targetPosition = cam.transform.position + Vector3.forward; // Move forward along the z-axis
        
    //     // Vector3 targetPosition_test = painting.transform.position;

    //     float elapsedTime = 0f;

    //     while (elapsedTime < duration)
    //     {
    //         // Calculate t using an easing function
    //         float t = elapsedTime / duration;
    //         t *= t; // Quadratic easing (ease-in)

    //         // Interpolate position with eased t
    //         cam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    //         Debug.Log("Camera is zooming in");

    //         elapsedTime += Time.deltaTime;

    //         // Fetch the distance between the cam and the painting
    //         float distance = Vector3.Distance(cam.transform.position, targetPosition);

    //         // If the distance is 1 unit away from the painting
    //         if (distance <= distanceFromPainting)
    //         {
    //             Debug.Log("Distance is 1 unit away from the painting");
    //             cam.transform.position = targetPosition;

    //             yield return new WaitForSeconds(1f);
                
    //             StartCoroutine(AfterTransition("Museum"));
                
    //             yield break;
    //         }

    //         yield return null;
    //     }

    //     // Ensure the camera reaches the exact target position at the end.
    //     cam.transform.position = targetPosition;
    // }
#endregion