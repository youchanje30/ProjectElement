using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] float xFactor;
    [SerializeField] float yFactor;
    // public float force;
 
    // public void Move(float x)
    // {
    //     Vector3 newPos = transform.localPosition;
    //     newPos.x -= x * force;
 
    //     transform.localPosition = newPos;
    // }

    
    public void Move(Vector2 vec)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= vec.x * xFactor;
        newPos.y -= vec.y * yFactor;
        transform.localPosition = newPos;
    }
}
