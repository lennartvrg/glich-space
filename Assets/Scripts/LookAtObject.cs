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

    void Update()
    {
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
            //Elevator Behaviour
            Debug.Log("Moritz gucke mal es geht!");
        }

        if (_isCounting)
        {
            countdown -= Time.deltaTime;
        }
    }
}

