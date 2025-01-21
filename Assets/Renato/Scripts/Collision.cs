using Unity.XR.CoreUtils;
using UnityEngine;

namespace Assets.Renato.Scripts 
{
    [RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(SphereCollider))]
    public class Collision : MonoBehaviour
    {
        public bool playerSitDown;
        float timer = 0f;

        
        private void OnTriggerStay(Collider collider) 
        {
            if(collider.CompareTag("Player")) 
            {
                Debug.Log("Player found!");

                timer += Time.deltaTime;

                if(timer >= 1.5f && !playerSitDown) 
                {
                    playerSitDown = true;
                    Debug.Log("There is no movement detected after the player collided with the chair...");
                } 
            }
        }

    }
}
