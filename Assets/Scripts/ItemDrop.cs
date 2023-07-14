using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    Rigidbody2D rigid2D;
    
    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        Drop();
    }

    
    public void Drop()
    {
        Vector2 DropDir = new Vector2(Random.Range(-30, 30 + 1), Random.Range(1, 90 + 1));
        float Force = Random.Range(0.1f, 0.5f);
        rigid2D.AddForce(DropDir * Force, ForceMode2D.Impulse);   
    }
}
