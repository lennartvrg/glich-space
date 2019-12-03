using UnityEngine;
using UnityEngine.XR;

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
        
        RenderTextureDescriptor desc = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 24);
        if (XRSettings.enabled)
        {
            desc = XRSettings.eyeTextureDesc;
            Debug.Log(desc.vrUsage);
            desc.width /= 2;
        }
        
        secondCamera.targetTexture = new RenderTexture(desc);
        secondCameraMaterial.mainTexture = secondCamera.targetTexture;
    }

    private void OnDestroy()
    {
        secondCamera.targetTexture.Release();
        secondCamera.targetTexture = null;
        secondCameraMaterial.mainTexture = null;
    }
}
