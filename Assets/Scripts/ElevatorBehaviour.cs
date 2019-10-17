using System.Collections;
using UnityEngine;

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
    
    private const string ELEVATOR_DOOR_OPEN = "Elevator_DoorOpen";
    
    private const string ELEVATOR_DOOR_CLOSE = "Elevator_DoorClose";


    // Start is called before the first frame update
    public void Start()
    {
        ApplyElevatorMovement(Platform, YAxisStoppingPoints[0] + 0.315f);
        Animation = Door.GetComponent<Animator>();
    }
    
    public override void SetPower(bool powerEnabled)
    {
        Animation.Play(ELEVATOR_DOOR_OPEN);
        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(5);
        Animation.Play(ELEVATOR_DOOR_CLOSE);
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
        if (ElevatorShouldMove())
        {
            if (AnimationDriver < 1.0f)
            {
                var upChange = Mathf.Lerp(YAxisStoppingPoints[CurrentStopIndex], YAxisStoppingPoints[NextStopIndex], AnimationDriver);
                
                ApplyElevatorMovement(Platform, upChange + 0.315f);
                ApplyElevatorMovement(TrackingArea, upChange);
                
                AnimationDriver += 0.5f * Time.deltaTime;
            }
            else
            {
                CurrentStopIndex = NextStopIndex;
                AnimationDriver = 0.0f;
                ResetPlatform = true;
                Animation.Play(ELEVATOR_DOOR_OPEN);
            }
        }
        else if (!PlatformCollider.bounds.Contains(Player.position) && ResetPlatform)
        {
            Animation.Play(ELEVATOR_DOOR_CLOSE);
            ResetPlatform = false;
        }
    }

    private bool ElevatorShouldMove()
    {
        return NextStopIndex != CurrentStopIndex && PlatformCollider.bounds.Contains(Player.position) &&
               !Door.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ELEVATOR_DOOR_CLOSE);
    }

    private void ApplyElevatorMovement(Transform target, float yAxis)
    {
        var pos = target.position;
        pos = new Vector3(pos.x, yAxis, pos.z);
        target.position = pos;
    }
}