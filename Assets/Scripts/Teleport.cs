using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRTK;

public class Teleport : MonoBehaviour
{
    public Transform player;

    private Vector3[] cube =
    {
        new Vector3(0.0122f, 2f, -0.0902f), 
        new Vector3(-0.5f, 2f, -0.0902f), 
        new Vector3(-0.5f, 2f, -1.5f), 
        new Vector3(0.0122f, 2f, -1.5f), 
        
        new Vector3(0.0122f, 0, -0.0902f), 
        new Vector3(-0.5f, 0, -0.0902f), 
        new Vector3(-0.5f, 0, -1.5f), 
        new Vector3(0.0122f, 0, -1.5f), 

    };
    
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        
        var playerPos = player.position - transform.position;
        var dotProduct = Vector3.Dot(transform.up, playerPos);
        if (dotProduct < 0) {
            player.position += new Vector3(0, 0, 4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}