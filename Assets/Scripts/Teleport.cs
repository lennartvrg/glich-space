using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform TrackedAlias;

    public ElevatorBehaviour ConnectedElevator;


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        TrackedAlias.Translate(new Vector3(0, 0, 4f));

        if (ConnectedElevator != null)
        {
            ConnectedElevator.SetPower(true);
        }
    }
}