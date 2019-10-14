using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    public virtual void SetPower(bool powerEnabled)
    {
        Debug.LogWarning("Activatable not implemented");
    }
}