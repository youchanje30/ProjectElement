using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public CameraController controller;
    List<Background> backgrounds = new List<Background>();
 
    void Start()
    {
        if (!controller)
            controller = Camera.main.GetComponent<CameraController>();
 
        controller.cameraTranslate += Move;
 
        SetLayers();
    }
 
    void SetLayers()
    {
        backgrounds.Clear();
 
        for (int i = 0; i < transform.childCount; i++)
        {
            Background bg = transform.GetChild(i).GetComponent<Background>();
 
            if (bg != null)
            {
                backgrounds.Add(bg);
            }
        }
    }
 
    void Move(float x)
    {
        foreach (Background bg in backgrounds)
        {
            bg.Move(x);
        }
    }
}
