using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{ 
    public float maxDistance = 100;
    private bool _isCounting = false;
    public float countdown = 3;
    public float time = 3;
    private RaycastHit _hit;
    private bool finished = false;
    public ElevatorBehaviour elevator;

    public GameObject zombie;
    private Renderer zombieRend;

    void Update()
    {
        if (finished)
        {
            return;
        }
        
        if (Physics.Raycast(transform.position, transform.forward, out _hit, maxDistance) && _hit.transform.CompareTag("Mirror"))
        {
            _isCounting = true;
        }
        else
        {
            _isCounting = false;
            countdown = time;
        }

        if (countdown <= 0)
        {
            //Zombie Behaviour
            zombieRend = zombie.GetComponent<Renderer>();
            zombieRend.enabled = true;
            
            //Elevator Behaviour
            Debug.Log("BumBum");
            elevator.SetPower(true);
            finished = true;

        }

        if (_isCounting)
        {
            countdown -= Time.deltaTime;
        }
    }
}

