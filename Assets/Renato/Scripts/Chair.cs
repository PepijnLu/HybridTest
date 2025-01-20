    	using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

namespace Assets.Renato.Scripts 
{
    public class Chair : MonoBehaviour
    {
        public GameObject player;
        public GameObject painting;
        // public float colliderRadius = 1.5f;
        public float distanceFromPainting = 2f;
        
        [SerializeField] private Camera cam;
        [SerializeField] private Transform childObj;
        

        [SerializeField] private MeshRenderer fadeQuadRenderer;
        [SerializeField] private float fadeDuration = 1f;
        private Material fadeMaterial;	
        
        public bool isCloseBy;

        private Collision collisionScript;
        
        void Awake()
        {
            if(childObj == null) 
            {
                childObj = transform.GetChild(0);
            }

            collisionScript = GetComponentInChildren<Collision>();
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
                StartCoroutine(Transition(2f, 4f, painting));
            }
        }

        void OnTriggerEnter(Collider collider) 
        {
            if(collider.CompareTag("Player")) 
            {
                if(!isCloseBy) 
                {
                    isCloseBy = true;
                    player = collider.gameObject;
                    Debug.Log("IsCloseBy");   
                    // Transform parent = collider.transform.parent; // Cam Offset
                    // Transform parentsParent = parent.transform.parent; // XR Rig

                    // Transform locoSystem = collider.transform.GetChild(1);

                    // BodyMovementDetection playerMovementDetection = locoSystem.GetComponentInChildren<BodyMovementDetection>();
                    
                    // if(playerMovementDetection != null)
                    // {
                        // PlayerMovementDetection = playerMovementDetection;
                
                        // Visual effect...
                        SomeVisualEffect();
                    // }
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
                }
            }
        }

        private void SomeVisualEffect() 
        {
            Debug.Log("Some visual effect...");
            // yield break;
        } 

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
        
        private void FixedCamPosition(GameObject painting) 
        {
            Debug.Log("FixedCameraPosition Function Executed...");
            
            // Stop the movement of the rig
            Collision collisionScript = childObj.GetComponent<Collision>();
            
            collisionScript.xROrigin.enabled = false;
            
            // Disable the tracking component on the cam
            Transform cam = player.transform.GetChild(0).transform.GetChild(0);
            TrackedPoseDriver trackedPoseDriver = cam.GetComponent<TrackedPoseDriver>();
            trackedPoseDriver.enabled = false;

            float rotationSpeed = 2f;
            
            // Fetch the position of the painting
            Vector3 directionToPainting = painting.transform.position - cam.transform.position;  

            // Get the desired rotation to face the painting
            Quaternion targetRotation = Quaternion.LookRotation(directionToPainting);
            
            // If the camera isn't facing the painting
            float angleDifference = Quaternion.Angle(cam.transform.rotation, targetRotation);

            if(angleDifference > .1f) 
            {
                // Smoothly rotate the camera to face the painting
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // private IEnumerator CameraZoomIn(float duration) 
        // {
        //     Debug.Log("CameraZoomIn Coroutine Started...");
        //     Vector3 startPosition = cam.transform.position;
        //     Vector3 targetPosition = cam.transform.position + Vector3.forward; // Move forward along the z-axis

        //     float elapsedTime = 0f;

        //     while(elapsedTime < duration) 
        //     {
        //         float t = elapsedTime / duration;
        //         cam.transform.position = Vector3.Lerp(startPosition, Vector3.forward, t);
        //         Debug.Log("Camera is zooming in");
        //         elapsedTime += Time.deltaTime;

        //         // Fetch the distanc between the cam and the painting
        //         float distance = Vector3.Distance(cam.transform.position, targetPosition);

        //         // If the distance is 1 unit away from the painting
        //         if(distance <= 1f) 
        //         {
        //             Debug.Log("Distance is 1 unit away from the painting");
        //             cam.transform.position = targetPosition;
        //             yield break;
        //         }
    
        //         yield return null;
        //     }

        //     // Ensure the camera reaches the exact target position at the end.
        //     cam.transform.position = targetPosition;
        // }

        private IEnumerator CameraZoomIn(float duration)
        {
            Debug.Log("CameraZoomIn Coroutine Started...");
            Vector3 startPosition = cam.transform.position;
            Vector3 targetPosition = cam.transform.position + Vector3.forward; // Move forward along the z-axis

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // Calculate t using an easing function
                float t = elapsedTime / duration;
                t *= t; // Quadratic easing (ease-in)

                // Interpolate position with eased t
                cam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                Debug.Log("Camera is zooming in");

                elapsedTime += Time.deltaTime;

                // Fetch the distance between the cam and the painting
                float distance = Vector3.Distance(cam.transform.position, targetPosition);

                // If the distance is 1 unit away from the painting
                if (distance <= distanceFromPainting)
                {
                    Debug.Log("Distance is 1 unit away from the painting");
                    cam.transform.position = targetPosition;
                    yield break;
                }

                yield return null;
            }

            // Ensure the camera reaches the exact target position at the end.
            cam.transform.position = targetPosition;
        }

        private IEnumerator Transition(float waitDuration, float transitionDuration, GameObject painting) 
        {
            Debug.Log("Transition Coroutine Started...");
        
            FixedCamPosition(painting);

            yield return new WaitForSeconds(waitDuration);

            StartCoroutine(CameraZoomIn(transitionDuration));

            yield return new WaitForFixedUpdate();

            StartCoroutine(AfterTransition("Museum"));

            yield break;
            
        }

        private IEnumerator AfterTransition(string nextSceneName) 
        {
            Debug.Log("AfterTransition Coroutine Started...");
            // Fade out to black
            yield return StartCoroutine(Fade(1f));

            // Load the next scene
            SceneManager.LoadScene(nextSceneName);

            // Wait for the scene to load
            yield return null;

            // Fade in from black
            yield return StartCoroutine(Fade(0f));

            yield break;
        }

         // Coroutine to fade the quad in or out
        private IEnumerator Fade(float targetAlpha)
        {
            float startAlpha = fadeMaterial.color.a;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

                SetQuadAlpha(newAlpha);
                yield return null;
            }

            // Ensure the final alpha is set
            SetQuadAlpha(targetAlpha);
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
