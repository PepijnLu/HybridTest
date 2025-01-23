using System.Collections;
using UnityEngine;

public class TestStuff : MonoBehaviour
{
    public Material steamMaterial; // Material with the SteamWipeShader
    public Camera playerCamera;    // Camera for detecting player input
    public float wipeRadius = 0.1f; // Radius of the wipe effect

    private Texture2D maskTexture;

    public float clearThreshold = 80f; // Percentage to trigger the action

    // public float fadeDuration = 2.0f; // Time it takes to fully reveal the paintings
    // public GameObject[] paintings;   // Array of game objects with SpriteRenderer to reveal

    // public Transform controllerPosition;

    // public GameObject steam, clear;


    void Start()
    {
        // Initialize the mask texture (512x512 for performance)
        maskTexture = new Texture2D(4, 4, TextureFormat.RGBA32, false);
        ClearMask(); // Set initial mask to "steamed" (white)
        steamMaterial.SetTexture("_Mask", maskTexture);
    }

    void Update()
    {
        // if (_true) // Left-click or touch
        // {
        //     Debug.Log("Method being executed");
        //     Ray ray = playerCamera.ScreenPointToRay(controllerPosition.position);
        //     if (Physics.Raycast(ray, out RaycastHit hit))
        //     {
        //         if (hit.collider.gameObject == gameObject) // Check if it's the mirror
        //         {
        //             Vector2 pixelUV = hit.textureCoord;
        //             DrawOnMask(pixelUV); // Draw the wipe effect on the mask
        //         }
        //     }
        // }

        if (Input.GetMouseButton(0)) // Left-click or touch
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if it's the mirror
                {
                    Vector2 pixelUV = hit.textureCoord;
                    DrawOnMask(pixelUV); // Draw the wipe effect on the mask
                }
            }
        }
    }

    public bool _true;
    public void InputDraw(bool _bool) 
    {
        _true = _bool;
    }

    // Clear the mask and set it to "steamed" (white)
    void ClearMask()
    {
        Color32[] clearColor = maskTexture.GetPixels32();
        for (int i = 0; i < clearColor.Length; i++)
            clearColor[i] = Color.white; // Start with fully steamed (white)
        maskTexture.SetPixels32(clearColor);
        maskTexture.Apply();
    }

    // Draw the wipe effect on the mask (reveal the clear areas)
    void DrawOnMask(Vector2 uv)
    {
        // Invert the y-coordinate for UV
        uv.y = 1.0f - uv.y;
        uv.x = 1.0f - uv.x;


        // Convert UV to mask texture coordinates
        int x = (int)(uv.x * maskTexture.width);
        int y = (int)(uv.y * maskTexture.height);

        // Draw a circle at the wipe position
        int radius = (int)(wipeRadius * maskTexture.width);
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                if (i * i + j * j <= radius * radius) // Circle equation
                {
                    int px = Mathf.Clamp(x + i, 0, maskTexture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, maskTexture.height - 1);

                    // Set the pixel to black to reveal the clear texture
                    maskTexture.SetPixel(px, py, Color.black); // Clear area (black)
                }
            }
        }
        maskTexture.Apply();

        CheckClearedPercentage(); // Check if 90% is cleared
    }

    // Check the percentage of cleared area
    void CheckClearedPercentage()
    {
        Color32[] pixels = maskTexture.GetPixels32();
        int clearedPixels = 0;

        foreach (Color32 pixel in pixels)
        {
            if (pixel.r == 0 && pixel.g == 0 && pixel.b == 0) // Black pixel (cleared)
            {
                clearedPixels++;
            }
        }

        float clearedPercentage = (clearedPixels / (float)pixels.Length) * 100f;
        if (clearedPercentage >= clearThreshold)
        {
            OnSurfaceCleared(); // Trigger the action when 90% is cleared
        }
    }

    // Gradually reveal paintings
    void OnSurfaceCleared()
    {
        Debug.Log("Surface is cleared! Revealing paintings...");

        // steam.SetActive(false);
        // clear.SetActive(false);

        // foreach (GameObject painting in paintings)
        // {
        //     StartCoroutine(FadeInSprite(painting.GetComponent<SpriteRenderer>()));
        // }
    }

    // Coroutine to fade in a sprite
    // IEnumerator FadeInSprite(SpriteRenderer spriteRenderer)
    // {
    //     if (spriteRenderer == null) yield break;

    //     Color color = spriteRenderer.color;
    //     float elapsedTime = 0f;
    //     float startAlpha = color.a;
    //     float targetAlpha = 1f;

    //     while (elapsedTime < fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
    //         spriteRenderer.color = new Color(color.r, color.g, color.b, newAlpha);
    //         yield return null;
    //     }

    //     // Ensure final alpha is set
    //     spriteRenderer.color = new Color(color.r, color.g, color.b, targetAlpha);
    // }
}