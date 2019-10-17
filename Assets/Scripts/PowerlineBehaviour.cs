using UnityEngine;

public class PowerlineBehaviour : Activatable
{
    private const string SHADER_EMISSION_KEYWORD = "_EMISSION";

    public Activatable Next;
    
    private Material Material;
    
    // Start is called before the first frame update
    public void Start()
    {
        Material = GetComponent<Renderer>().material;
    }

    public override void SetPower(bool powerEnabled)
    {
        if (powerEnabled) Material.EnableKeyword(SHADER_EMISSION_KEYWORD);
        else Material.DisableKeyword(SHADER_EMISSION_KEYWORD);
        
        if (Next != null) Next.SetPower(powerEnabled);
    }
}
