using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Movement")]
    public Transform Player;

    public Transform otherPortal;

    public Transform portal;
    
    private void LateUpdate()
    {
        Vector3 playerOffsetFromPortal = Player.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortals = Quaternion.Angle(portal.rotation, otherPortal.rotation);
        Quaternion portalRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortals, Vector3.up);
        Vector3 newCameraDirection = portalRotationDifference * Player.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}