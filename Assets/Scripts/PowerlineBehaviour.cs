using UnityEngine;

public class PowerlineBehaviour : MonoBehaviour
{
    private const string SHADER_EMISSION_KEYWORD = "_EMISSION";

    public PowerlineBehaviour Next;
    
    private Material Material;
    
    // Start is called before the first frame update
    public void Start()
    {
        Material = GetComponent<Renderer>().material;
    }

    public void SetPower(bool enabled)
    {
        if (enabled)
        {
            Material.EnableKeyword(SHADER_EMISSION_KEYWORD);
        }
        else
        {
            Material.DisableKeyword(SHADER_EMISSION_KEYWORD);
        }
        
        if (Next != null) Next.SetPower(enabled);
    }
}
