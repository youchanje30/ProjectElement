using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    Rigidbody2D rigid2D;
    
    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Drop();
    }
    
    public void Drop()
    {
        Vector2 DropDir = new Vector2(Random.Range(-2, 2 + 1), Random.Range(0f, 1f));
        float Force = Random.Range(1f, 3f);
        rigid2D.AddForce(DropDir * Force, ForceMode2D.Impulse);   
        rigid2D.velocity = DropDir * Force;
    }

}
