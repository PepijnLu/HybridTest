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
        ClearMask();
        steamMaterial.SetTexture("_Mask", maskTexture);
    }

    // public void 

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
                    DrawOnMask(pixelUV);
                }
            }
        }
    }

    void ClearMask()
    {
        // Initialize the mask with a fully steamed texture (black)
        Color32[] clearColor = maskTexture.GetPixels32();
        for (int i = 0; i < clearColor.Length; i++)
            clearColor[i] = Color.black;
        maskTexture.SetPixels32(clearColor);
        maskTexture.Apply();
    }

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
                    maskTexture.SetPixel(px, py, Color.white);
                }
            }
        }
        maskTexture.Apply();
    }
}
