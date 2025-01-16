using UnityEngine;
using UnityEngine.UI;

public class WacomDrawing : MonoBehaviour
{
    public RawImage drawingSurface;
    public Color drawColor = Color.black;
    public int brushSize = 10;

    private Texture2D texture;
    private RectTransform canvasRect;

    private Vector2? lastPosition = null;

    void Start()
    {
        Cursor.visible = false;

        // Get the canvas rect transform
        canvasRect = drawingSurface.GetComponent<RectTransform>();

        // Create a lower-resolution texture for better performance
        texture = new Texture2D(512, 512);
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

            Vector2 currentPosition = new Vector2(x, y);

            if (lastPosition.HasValue)
            {
                DrawLine(lastPosition.Value, currentPosition);
            }
            else
            {
                Draw(x, y);
            }

            lastPosition = currentPosition;
        }
        else
        {
            lastPosition = null; // Reset when the mouse button is not pressed
        }

        texture.Apply(); // Apply changes once per frame
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        Vector2 direction = (end - start).normalized;

        for (float i = 0; i <= distance; i += 1.0f) // Use a larger step for performance
        {
            Vector2 point = start + direction * i;
            Draw(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
        }
    }

    void Draw(int x, int y)
    {
        // Draw a circle (brush)
        int sqrBrushSize = brushSize * brushSize;
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (i * i + j * j <= sqrBrushSize)
                {
                    int px = Mathf.Clamp(x + i, 0, texture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, texture.height - 1);
                    texture.SetPixel(px, py, drawColor);
                }
            }
        }
    }

    void ClearTexture()
    {
        Color[] clearPixels = new Color[texture.width * texture.height];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.white;
        }
        texture.SetPixels(clearPixels);
        texture.Apply();
    }
}
