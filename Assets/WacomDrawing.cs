// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.InputSystem;


// public class WacomDrawing : MonoBehaviour
// {
//     // Input Action
//     [SerializeField] private InputActionProperty rightHandPrimaryButton;


//     public RawImage drawingSurface;
//     public Color drawColor = Color.black;
//     public int brushSize = 10;

//     private Texture2D texture;
//     private RectTransform canvasRect;

//     private Vector2? lastPosition = null;

//     [SerializeField] private InputActionProperty rightControllerPositionAction; // VR controller position
//     [SerializeField] private InputActionProperty rightControllerBButtonAction;  // VR B button action

//     private bool isDrawing = false;

//     bool inputting;

//     void OnEnable()
//     {
//         // Subscribe to B button press
//         rightControllerBButtonAction.action.performed += OnBButtonPressed;
//         rightControllerBButtonAction.action.canceled += OnBButtonReleased;
//     }

//     void OnDisable()
//     {
//         // Unsubscribe from B button press
//         rightControllerBButtonAction.action.performed -= OnBButtonPressed;
//         rightControllerBButtonAction.action.canceled -= OnBButtonReleased;
//     }


//     void Start()
//     {        
//         Cursor.visible = false;

//         // Get the canvas rect transform
//         canvasRect = drawingSurface.GetComponent<RectTransform>();

//         // Create a lower-resolution texture for better performance
//         texture = new Texture2D(512, 512);
//         drawingSurface.texture = texture;

//         // Initialize texture with white color
//         ClearTexture();
//     }

//     private void Update() 
//     {
//         if (isDrawing)
//         {
//             // Get the position of the VR controller
//             Vector3 controllerPosition = rightControllerPositionAction.action.ReadValue<Vector3>();

//             // Convert the controller position to a 2D screen point
//             Vector2 screenPoint = Camera.main.WorldToScreenPoint(controllerPosition);

//             // Convert the screen point to local canvas coordinates
//             RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 localPos);

//             // Map the local position to texture coordinates
//             int x = Mathf.FloorToInt((localPos.x + canvasRect.rect.width / 2) / canvasRect.rect.width * texture.width);
//             int y = Mathf.FloorToInt((localPos.y + canvasRect.rect.height / 2) / canvasRect.rect.height * texture.height);

//             Vector2 currentPosition = new Vector2(x, y);

//             // Draw a line if there was a previous position
//             if (lastPosition.HasValue)
//             {
//                 DrawLine(lastPosition.Value, currentPosition);
//             }
//             else
//             {
//                 Draw(x, y);
//             }

//             lastPosition = currentPosition;

//             // Apply the changes to the texture
//             texture.Apply();
//         }
//         else
//         {
//             lastPosition = null; // Reset when not drawing
//         }
//     }
    

//     private void OnBButtonPressed(InputAction.CallbackContext context)
//     {
//         isDrawing = true;
//         Debug.Log("B button pressed, drawing started.");
//     }

//     private void OnBButtonReleased(InputAction.CallbackContext context)
//     {
//         isDrawing = false;
//         Debug.Log("B button released, drawing stopped.");
//     }


//     void DrawLine(Vector2 start, Vector2 end)
//     {
//         float distance = Vector2.Distance(start, end);
//         Vector2 direction = (end - start).normalized;

//         for (float i = 0; i <= distance; i += 1.0f) // Use a larger step for performance
//         {
//             Vector2 point = start + direction * i;
//             Draw(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
//         }
//     }

//     void Draw(int x, int y)
//     {
//         // Draw a circle (brush)
//         int sqrBrushSize = brushSize * brushSize;
//         for (int i = -brushSize; i <= brushSize; i++)
//         {
//             for (int j = -brushSize; j <= brushSize; j++)
//             {
//                 if (i * i + j * j <= sqrBrushSize)
//                 {
//                     int px = Mathf.Clamp(x + i, 0, texture.width - 1);
//                     int py = Mathf.Clamp(y + j, 0, texture.height - 1);
//                     texture.SetPixel(px, py, drawColor);
//                 }
//             }
//         }
//     }

//     void ClearTexture()
//     {
//         Color32[] clearPixels = new Color32[texture.width * texture.height];
//         for (int i = 0; i < clearPixels.Length; i++)
//         {
//             clearPixels[i] = Color.white;
//         }
//         texture.SetPixels32(clearPixels);
//         texture.Apply();
//     }
// }

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WacomDrawing : MonoBehaviour
{
    public RawImage drawingSurface;
    public Color drawColor = Color.black;
    public int brushSize = 10;

    private Texture2D texture;
    private RectTransform canvasRect;

    private Vector2? lastPosition = null;

    // Input Action
      // Input Action
    [SerializeField] private InputActionProperty rightControllerPositionAction;
    // [SerializeField] private InputActionProperty rightControllerPositionAction;

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

    // void Update()
    // {
    //     if (secondaryButton.action.WasPressedThisFrame()) 
    //     {
    //         Debug.Log("Secondary button from the right controller is pressed");
    //         // Get mouse/stylus position
    //         Vector2 localPos;
    //         RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, transform.up, null, out localPos);

    //         // Map to texture coordinates
    //         int x = Mathf.FloorToInt((localPos.x + canvasRect.rect.width / 2) / canvasRect.rect.width * texture.width);
    //         int y = Mathf.FloorToInt((localPos.y + canvasRect.rect.height / 2) / canvasRect.rect.height * texture.height);

    //         Vector2 currentPosition = new Vector2(x, y);

    //         if (lastPosition.HasValue)
    //         {
    //             DrawLine(lastPosition.Value, currentPosition);
    //         }
    //         else
    //         {
    //             Draw(x, y);
    //         }

    //         lastPosition = currentPosition;
    //     }
    //     else if(secondaryButton.action.WasReleasedThisFrame()) 
    //     {
    //         lastPosition = null; // Reset when the mouse button is not pressed
    //     }

        

    //     texture.Apply(); // Apply changes once per frame
    // }


    void Update()
    {
        if (rightControllerPositionAction.action.WasPressedThisFrame())
        {
            Debug.Log("Trigger button from the right controller is pressed");

            // Get the right controller's position
            Vector3 controllerWorldPosition = rightControllerPositionAction.action.ReadValue<Vector3>();

            // Convert the controller position to screen space
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(controllerWorldPosition);

            // Convert the screen point to local canvas coordinates
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, Camera.main, out Vector2 localPos))
            {
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
                Debug.LogWarning("Failed to map controller position to canvas coordinates.");
            }
        }
        else
        {
            lastPosition = null; // Reset when the button is not pressed
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


// if (Input.GetMouseButton(0)) // Stylus or mouse press
        // {
        //     // Get mouse/stylus position
        //     Vector2 localPos;
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPos);

        //     // Map to texture coordinates
        //     int x = Mathf.FloorToInt((localPos.x + canvasRect.rect.width / 2) / canvasRect.rect.width * texture.width);
        //     int y = Mathf.FloorToInt((localPos.y + canvasRect.rect.height / 2) / canvasRect.rect.height * texture.height);

        //     Vector2 currentPosition = new Vector2(x, y);

        //     if (lastPosition.HasValue)
        //     {
        //         DrawLine(lastPosition.Value, currentPosition);
        //     }
        //     else
        //     {
        //         Draw(x, y);
        //     }

        //     lastPosition = currentPosition;
        // }
        // else
        // {
        //     lastPosition = null; // Reset when the mouse button is not pressed
        // }