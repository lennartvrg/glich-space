using UnityEngine;

public class TextureSetup : MonoBehaviour
{
    public Camera secondCamera;
    
    public Material secondCameraMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        secondCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        secondCameraMaterial.mainTexture = secondCamera.targetTexture;
    }

    private void OnDestroy()
    {
        secondCamera.targetTexture.Release();
        secondCamera.targetTexture = null;
        secondCameraMaterial.mainTexture = null;
    }
}
