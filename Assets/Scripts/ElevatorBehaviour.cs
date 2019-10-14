using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ElevatorBehaviour : Activatable
{
    public Transform TrackingArea;

    public Transform Player;
    
    public GameObject Door;

    public Transform Platform;
    
    public BoxCollider PlatformCollider;

    public float[] YAxisStoppingPoints;

    private int CurrentStopIndex = 0;

    private int NextStopIndex = 0;

    private float AnimationDriver = 0.0f;

    private bool ResetPlatform = false;

    private Animator Animation;

    // Start is called before the first frame update
    public void Start()
    {
        ApplyElevatorMovement(Platform, YAxisStoppingPoints[0] + 0.315f);
        Animation = Door.GetComponent<Animator>();
    }

    public override void SetPower(bool powerEnabled)
    {
        Animation.Play("Elevator_DoorOpen");
        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(5);
        Animation.Play("Elevator_DoorClose");
        Up();
    }

    private void Up()
    {
        if (CurrentStopIndex < YAxisStoppingPoints.Length - 1)
        {
            NextStopIndex = CurrentStopIndex + 1;
        }
    }

    void Update()
    {
        if (NextStopIndex != CurrentStopIndex && PlatformCollider.bounds.Contains(Player.position) && !Door.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Elevator_DoorClose"))
        {
            if (AnimationDriver < 1.0f)
            {
                var upChange = Mathf.Lerp(YAxisStoppingPoints[CurrentStopIndex], YAxisStoppingPoints[NextStopIndex], AnimationDriver);
                
                ApplyElevatorMovement(Platform, upChange + 0.315f);
                ApplyElevatorMovement(Player, upChange);
                ApplyElevatorMovement(TrackingArea, upChange);
                
                AnimationDriver += 0.5f * Time.deltaTime;
            }
            else
            {
                CurrentStopIndex = NextStopIndex;
                AnimationDriver = 0.0f;
                ResetPlatform = true;
                Animation.Play("Elevator_DoorOpen");
            }
        }
        else if (!PlatformCollider.bounds.Contains(Player.position) && ResetPlatform)
        {
            Animation.Play("Elevator_DoorClose");
            ResetPlatform = false;
        }
    }

    private void ApplyElevatorMovement(Transform target, float yAxis)
    {
        var pos = target.position;
        pos = new Vector3(pos.x, yAxis, pos.z);
        target.position = pos;
    }
}