using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CameraFollow : MonoBehaviour
{
    
    public Transform playerCamera;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCamera.position + new Vector3(0, 0, 4);
        transform.rotation = playerCamera.rotation;
    }

    
}