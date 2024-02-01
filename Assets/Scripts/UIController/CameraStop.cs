using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStop : MonoBehaviour
{
    CameraController controller;
    Camera Camera;
    Transform Player;
    public bool isStart;

    void Start()
    {
        Camera = Camera.main;
        controller = Camera.transform.parent.GetComponent<CameraController>();
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Player.position.y, transform.position.z);
        if (isStart)
        {
            Camerastop();
        }
        else
        {
            EndCamera();
        }
    }

    public void Camerastop()
    {
       if(transform.position.x >= Player.position.x)
        {
            controller.cinemachineCam.Follow = transform;
        }
    }
    public void EndCamera()
    {
        if ( Player.position.x >= transform.position.x)
        {
            controller.cinemachineCam.Follow = transform;
        }
        else
        {
            controller.cinemachineCam.Follow = Player;
        }
    }
}
