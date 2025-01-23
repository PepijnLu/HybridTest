using UnityEngine;
using UnityEngine.UI;

public class BlaBlaStuff : MonoBehaviour
{
    public RawImage rawImage;
    public RenderTexture maskRenderTexture;

    void Start()
    {
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = maskRenderTexture;
    }
}
