using UnityEngine;
using UnityEngine.UI;

public class WacomDrawing : MonoBehaviour
{
    public RawImage drawingSurface;
    public Color drawColor = Color.black;
    public int brushSize = 10;

    private Texture2D texture;
    private RectTransform canvasRect;

    void Start()
    {
        // Get the canvas rect transform
        canvasRect = drawingSurface.GetComponent<RectTransform>();

        // Create a texture
        texture = new Texture2D(1024, 1024);
        drawingSurface.texture = texture;

        // Initialize texture with white color
        ClearTexture();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Stylus or mouse press
        {
            // Get mouse/stylus position
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPos);

            // Map to texture coordinates
            int x = Mathf.FloorToInt((localPos.x + canvasRect.rect.width / 2) / canvasRect.rect.width * texture.width);
            int y = Mathf.FloorToInt((localPos.y + canvasRect.rect.height / 2) / canvasRect.rect.height * texture.height);

            Draw(x, y);
        }
    }

    void Draw(int x, int y)
    {
        // Draw a circle (brush)
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize)
                {
                    int px = Mathf.Clamp(x + i, 0, texture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, texture.height - 1);
                    texture.SetPixel(px, py, drawColor);
                }
            }
        }

        texture.Apply(); // Apply changes
    }

    void ClearTexture()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }
}
