using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform TrackedAlias;

    public ElevatorBehaviour ConnectedElevator;

    private bool IgnoreNextTrigger = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (IgnoreNextTrigger)
        {
            IgnoreNextTrigger = false;
            return;
        }

        TrackedAlias.Translate(new Vector3(0, 0, 4));
        if (ConnectedElevator != null)
        {
            ConnectedElevator.SetPower(true);
        }
    }

    public void SetIgnoreNextTrigger(bool ignore)
    {
        IgnoreNextTrigger = !IgnoreNextTrigger && ignore;
    }
}