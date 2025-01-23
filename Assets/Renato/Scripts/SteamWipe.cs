// using System.Collections;
// using UnityEngine;
// using UnityEngine.Experimental.Rendering;

// public class SteamWipe : MonoBehaviour
// {
//     public Material shaderGraphMaterial; // Material with the SteamWipeShader
//     public Camera playerCamera;    // Camera for detecting player input
//     public float wipeRadius = 0.1f; // Radius of the wipe effect
//     public RenderTexture maskRenderTexture; // Render texture for the mask

//     public float clearThreshold = 80f; // Percentage to trigger the action
//     public float fadeDuration = 2.0f;  // Time it takes to fully reveal the paintings
//     // public GameObject[] paintings;    // Array of game objects with SpriteRenderer to reveal

//     // public Transform controllerPosition;
//     // public GameObject steam, clear;

//     public Material drawMaterial; // Material used to draw on the mask
//     private bool _isDrawing = false;
//     // public Material debugMaterial; // A material to view the RenderTexture

//     void Start()
//     {
//         if (maskRenderTexture == null)
//         {
//             // maskRenderTexture = new RenderTexture(512, 512, 0, GraphicsFormat.R32G32B32A32_SFloat);
//             maskRenderTexture = new RenderTexture(512, 512, 0, GraphicsFormat.R8G8B8A8_UNorm)
//             {
//                 useMipMap = false,
//                 autoGenerateMips = false
//             };
            
//             maskRenderTexture.Create();
//         }

//         // Assign the RenderTexture to the material
//         if (shaderGraphMaterial != null)
//         {
//             shaderGraphMaterial.SetTexture("_Mask", maskRenderTexture);
//         }
//         else
//         {
//             Debug.LogError("Steam Material is not assigned!");
//         }

//         // Clear the RenderTexture
//         ClearMask();

//         // Create a material for drawing
//         // drawMaterial = new Material(Shader.Find("Assets/Renato/SteamWipe Graph"));

//         // Clear the mask texture
//         // ClearMask();

        
//     }

//     void OnGUI()
//     {
//         if (Event.current.type.Equals(EventType.Repaint))
//         {
//             RenderTexture.active = maskRenderTexture; // Bind RenderTexture

//             GL.PushMatrix();
//             GL.LoadOrtho();
            
//             drawMaterial.SetPass(0); // Use the material to draw
//             Graphics.DrawTexture(new Rect(0, 0, 1, 1), maskRenderTexture, drawMaterial);

//             GL.PopMatrix();
//             RenderTexture.active = null; // Unbind RenderTexture
//         }

//         // Update the shader graph material with the RenderTexture mask
//         shaderGraphMaterial.SetTexture("_Mask", maskRenderTexture);
//     }


//     void Update()
//     {
//         // if (_isDrawing)
//         // {
//         //     DrawAtControllerPosition(controllerPosition.position);
//         // }

//         if (Input.GetMouseButton(0)) // Left-click or touch
//         {
//             Vector3 mousePos = Input.mousePosition;
//             DrawAtMousePosition(mousePos);
//         }
        
//     }

//     public void InputDraw(bool isDrawing)
//     {
//         _isDrawing = isDrawing;
//     }

//     void ClearMask()
//     {
//         RenderTexture.active = maskRenderTexture;
//         GL.Clear(false, true, Color.white);
//         RenderTexture.active = null;
//     }

//     void DrawAtControllerPosition(Vector3 position)
//     {
//         Ray ray = playerCamera.ScreenPointToRay(position);
//         if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
//         {
//             DrawCircle(hit.textureCoord);
//         }
//     }

//     void DrawAtMousePosition(Vector3 mousePosition)
//     {
//         Ray ray = playerCamera.ScreenPointToRay(mousePosition);
//         if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
//         {
//             DrawCircle(hit.textureCoord);
//         }
//     }

//     void DrawCircle(Vector2 uv)
//     {
//         uv.y = 1.0f - uv.y;
//         // uv.x = 1.0f - uv.x;

//         // Set the shader properties
//         drawMaterial.SetVector("_UV", uv);  
//         drawMaterial.SetFloat("_Radius", wipeRadius);

//         // Draw on the render texture
//         RenderTexture.active = maskRenderTexture;
//         GL.PushMatrix();
//         GL.LoadOrtho();
//         drawMaterial.SetPass(0);
//         Debug.Log("The shader pass is valid");
//         GL.Begin(GL.QUADS);
//         GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 0);
//         GL.TexCoord2(1, 0); GL.Vertex3(1, 0, 0);
//         GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 0);
//         GL.TexCoord2(0, 1); GL.Vertex3(0, 1, 0);
//         GL.End();
//         GL.PopMatrix();
//         RenderTexture.active = null;

//         Debug.Log("Drawing circle");

//         CheckClearedPercentage();
//     }

//     void CheckClearedPercentage()
//     {
//         // Read pixels from the render texture and calculate the cleared percentage
//         RenderTexture.active = maskRenderTexture;
//         Texture2D tempTexture = new(maskRenderTexture.width, maskRenderTexture.height, TextureFormat.RGBA32, false);
//         tempTexture.ReadPixels(new Rect(0, 0, maskRenderTexture.width, maskRenderTexture.height), 0, 0);
//         tempTexture.Apply();

//         Color[] pixels = tempTexture.GetPixels();
//         int clearedPixels = 0;

//         foreach (Color pixel in pixels)
//         {
//             if (pixel.r < 0.1f) // Check if the pixel is "cleared" (black)
//             {
//                 clearedPixels++;
//             }
//         }

//         Destroy(tempTexture);

//         float clearedPercentage = clearedPixels / (float)pixels.Length * 100f;
//         if (clearedPercentage >= clearThreshold)
//         {
//             OnSurfaceCleared();
//         }
//     }

//     void OnSurfaceCleared()
//     {
//         Debug.Log("Surface is cleared! Revealing paintings...");
        
//         // foreach (GameObject painting in paintings)
//         // {
//         //     StartCoroutine(FadeInSprite(painting.GetComponent<SpriteRenderer>()));
//         // }
//     }

//     IEnumerator FadeInSprite(SpriteRenderer spriteRenderer)
//     {
//         if (spriteRenderer == null) yield break;

//         Color color = spriteRenderer.color;
//         float elapsedTime = 0f;

//         while (elapsedTime < fadeDuration)
//         {
//             elapsedTime += Time.deltaTime;
//             float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
//             spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
//             yield return null;
//         }

//         spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
//     }
// }

using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SteamWipe : MonoBehaviour
{
    public Material steamWipeMaterial; // Material with the SteamWipeShader
    public Material drawMaterial; // Material with the SteamWipeShader
    public Texture2D steamTexture, clearTexture; // Steam texture to show

    public Camera playerCamera;    // Camera for detecting player input
    public float wipeRadius = 0.1f; // Radius of the wipe effect
    public RenderTexture maskRenderTexture; // Render texture for the mask

    public float clearThreshold = 80f; // Percentage to trigger the action
    public float fadeDuration = 2.0f;  // Time it takes to fully reveal the paintings

    private bool _isDrawing = false;

    void Start()
    {
        if (maskRenderTexture == null)
        {
            maskRenderTexture = new RenderTexture(512, 512, 0, GraphicsFormat.R32G32B32A32_SFloat);
            maskRenderTexture.Create();
        }

        // Assign textures to the material
        if (steamWipeMaterial != null)
        {
            // Assign textures to the shader graph properties
            steamWipeMaterial.SetTexture("_SteamTexture", steamTexture);
            steamWipeMaterial.SetTexture("_ClearTexture", clearTexture);
            steamWipeMaterial.SetTexture("_Mask", maskRenderTexture);

        }
        else
        {
            Debug.LogError("Steam Material is not assigned!");
        }

        drawMaterial = new Material(Shader.Find("Custom/DrawMaskShader"));

        if (maskRenderTexture == null)
        {
            maskRenderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
            maskRenderTexture.Create();
            Debug.Log("RenderTexture Created!");
        }

        // Clear the RenderTexture to initialize
        ClearMask();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left-click or touch
        {
            Vector3 mousePos = Input.mousePosition;
            DrawAtMousePosition(mousePos);
        }
    }

    public void InputDraw(bool isDrawing)
    {
        _isDrawing = isDrawing;
    }

    void ClearMask()
    {
        RenderTexture.active = maskRenderTexture;
        GL.Clear(false, true, Color.white);  // Clear the mask with white color
        RenderTexture.active = null;
    }

    void DrawAtMousePosition(Vector3 mousePosition)
    {
        Ray ray = playerCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
        {
            Debug.Log(hit.collider.gameObject.name);
            DrawCircle(hit.textureCoord);
        }
    }

    void DrawCircle(Vector2 uv)
    {
        uv.y = 1.0f - uv.y;
        uv.x = 1.0f - uv.x;

        // Set the shader properties
        drawMaterial.SetVector("_UV", uv);
        drawMaterial.SetFloat("_Radius", wipeRadius);

        // Draw on the render texture
        RenderTexture.active = maskRenderTexture;
        GL.PushMatrix();
        GL.LoadOrtho();
        drawMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 0);
        GL.TexCoord2(1, 0); GL.Vertex3(1, 0, 0);
        GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 0);
        GL.TexCoord2(0, 1); GL.Vertex3(0, 1, 0);
        GL.End();
        GL.PopMatrix();
        RenderTexture.active = null;

        Debug.Log("Drawing circle");

        CheckClearedPercentage();
    }

    void CheckClearedPercentage()
    {
        RenderTexture.active = maskRenderTexture;
        Texture2D tempTexture = new(maskRenderTexture.width, maskRenderTexture.height, TextureFormat.RGBA32, false);
        tempTexture.ReadPixels(new Rect(0, 0, maskRenderTexture.width, maskRenderTexture.height), 0, 0);
        tempTexture.Apply();

        Color[] pixels = tempTexture.GetPixels();
        int clearedPixels = 0;

        foreach (Color pixel in pixels)
        {
            if (pixel.r < 0.1f) // Check if the pixel is "cleared" (black)
            {
                clearedPixels++;
            }
        }

        Destroy(tempTexture);

        float clearedPercentage = clearedPixels / (float)pixels.Length * 100f;
        if (clearedPercentage >= clearThreshold)
        {
            OnSurfaceCleared();
        }
    }

    void OnSurfaceCleared()
    {
        Debug.Log("Surface is cleared! Revealing paintings...");
    }
}
