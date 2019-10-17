using System.Collections;
using System.Collections.Generic;
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
        secondCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        secondCameraMaterial.mainTexture = secondCamera.targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
