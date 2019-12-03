using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{ 
    public float maxDistance = 1;
    private bool _isCounting = false;
    public float countdown = 3;
    public float time = 3;
    private RaycastHit _hit;
    private bool finished = false;
    public ElevatorBehaviour elevator;
    public float zCountdown = 4;
    private bool zCounting = false;

    public GameObject zombie;
    private Renderer zombieRend;
    private AudioSource zombieAudio;

    void Start()
    {
        //Zombie Behaviour
        zombieRend = zombie.GetComponent<Renderer>();
        zombieAudio = zombie.GetComponent<AudioSource>();
    }

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
            zombieRend.enabled = true;
            zCounting = true;
            zombieAudio.Play();

            //Elevator Behaviour
            elevator.SetPower(true);
            
        }

        if (zCounting)
        {
            countdown = 500;
            zCountdown -= Time.deltaTime;
            if (zCountdown <= 0)
            {
                zombieRend.enabled = false;
                finished = true;
            }
        }
        
        if (_isCounting)
        {
            countdown -= Time.deltaTime;
        }
    }
}

