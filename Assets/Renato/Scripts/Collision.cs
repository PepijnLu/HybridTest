using Unity.XR.CoreUtils;
using UnityEngine;

namespace Assets.Renato.Scripts 
{
    [RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(SphereCollider))]
    public class Collision : MonoBehaviour
    {
        public bool playerSitDown;
        // public float colliderRadius = 1f;
        public XROrigin xROrigin;
        // public BodyMovementDetection playerMovementDetection;
        float timer = 0f;
        // private SphereCollider sphereCollider;

        // void Start() 
        // {
        //     sphereCollider = GetComponent<SphereCollider>();
        // }
        
        private void OnTriggerStay(Collider collider) 
        {
            if(collider.CompareTag("Player")) 
            {
                // xROrigin = collider.GetComponent<XROrigin>();

                // Transform locoSystem = collider.transform.GetChild(1);
                // BodyMovementDetection bodyMovementDetection = locoSystem.GetComponentInChildren<BodyMovementDetection>();

                Debug.Log("Player found!");
                // if(bodyMovementDetection.headsetSpeed <= .05f && bodyMovementDetection.controllerSpeed <= .05f && bodyMovementDetection.leftControllerSpeed <= .05f/*playerMovementDetection.xrRig.position.magnitude < 0.01f*/) 
                // {

                    timer += Time.deltaTime;

                    if(timer >= 2f && !playerSitDown) 
                    {
                        playerSitDown = true;
                        Debug.Log("There is no movement detected after the player collided with the chair...");
                    } 
                // }
            }
        }

        
        // [SerializeField] private Color sphereColor = Color.green; // The color of the sphere in the Scene view

        // private void OnDrawGizmos()
        // {
        //     // Set the Gizmo color
        //     Gizmos.color = sphereColor;

        //     // Draw the sphere at the given position and radius
        //     Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            
        // }
    }
}
