using UnityEngine;

public class SteamWipe : MonoBehaviour
{
    public Material steamMaterial; // Material with the SteamWipeShader
    public Camera playerCamera;    // Camera for detecting player input
    public float wipeRadius = 0.1f; // Radius of the wipe effect

    private Texture2D maskTexture;
    private Vector2 lastMousePos;

    void Start()
    {
        // Initialize the mask texture (512x512 for performance)
        maskTexture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        ClearMask(); // Set initial mask to "steamed" (white)
        steamMaterial.SetTexture("_Mask", maskTexture);
    }

    void Update()
    {
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
    }
}
