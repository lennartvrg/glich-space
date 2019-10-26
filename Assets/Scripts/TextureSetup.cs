using UnityEngine;

public class TextureSetup : MonoBehaviour
{

    public Camera secondCamera;
    public Material secondCameraMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        if (secondCamera.targetTexture != null)
        {
            secondCamera.targetTexture.Release();
        }
        secondCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB64);
        secondCameraMaterial.mainTexture = secondCamera.targetTexture;
    }
}
