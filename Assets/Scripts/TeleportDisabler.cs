using UnityEngine;

public class TeleportDisabler : MonoBehaviour
{
    public Teleport Teleporter;

    private void OnTriggerEnter(Collider other)
    {
        Teleporter.SetIgnoreNextTrigger(true);
    }
}
