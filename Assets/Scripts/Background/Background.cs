using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float force;
 
    public void Move(float x)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= x * force;
 
        transform.localPosition = newPos;
    }
}
