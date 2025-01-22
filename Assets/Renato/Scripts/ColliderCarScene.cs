using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Renato.Scripts 
{
    public class ColliderCarScene : MonoBehaviour
    {
        public Vector3 lastWorldPosition;
        public Management management;        
        [SerializeField] private MeshRenderer fadeQuadRenderer;
        private Material fadeMaterial;	
        [SerializeField] private float fadeDuration = 1f;

        public Transform XR_rig;

        void Awake() 
        {
            // management = FindObjectOfType<Management>();
        }

        void Start() 
        {
            // Get the material from the quad
            fadeMaterial = fadeQuadRenderer.material;

            // Ensure the quad starts fully transparent
            SetQuadAlpha(0f);

            // lastWorldPosition = management.lastWorldPosition;
        }

        private void OnTriggerEnter(Collider collider) 
        {
            if(collider.CompareTag("Car")) 
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() 
        {
            yield return StartCoroutine(FadeAndLoadScene("Museum", 1f));

            // Scene is now fully loaded, assign the world position to XR_rig
            if (XR_rig != null)
            {
                XR_rig.position = lastWorldPosition;
            }
        }

        private IEnumerator FadeAndLoadScene(string nextSceneName, float targetAlpha)
        {
            float startAlpha = fadeMaterial.color.a;
            float elapsedTime = 0f;
            float newAlpha = 0;

            // Fade to target alpha
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                SetQuadAlpha(newAlpha);

                yield return null;
            }

            // Ensure the final alpha is set
            SetQuadAlpha(targetAlpha);

            // Wait until the fade has completed (to ensure the alpha reached target value)
            if (newAlpha == targetAlpha)
            {
                // Load the next scene asynchronously
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
                asyncLoad.allowSceneActivation = false;

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    if (asyncLoad.progress >= 0.9f)
                    {
                        // Scene is ready, activate it
                        asyncLoad.allowSceneActivation = true;
                    }

                    yield return null;
                }
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
