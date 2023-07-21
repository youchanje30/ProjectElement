using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Arrow
}

public class ProjectileType : MonoBehaviour
{
    public Type Projectile;
    private CapsuleCollider2D Collider2D;
    public float Damage;
    public float Throwspeed;

    
    void Update()
    {
        transform.position += new Vector3(-(transform.localScale.x),0,0) * Time.deltaTime * Throwspeed;
    }

    private void Awake()
    {
        Collider2D = GetComponent<CapsuleCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log(other.gameObject.tag);
        if(other.tag == "Floor")
        {
            Destroy(gameObject);
        }
        else if(other.tag == "Monster")
        {
            other.GetComponent<Monster>().GetDamaged(Damage);
            Destroy(gameObject);
        }
        else if(other.tag == "Destruct")
        {
            other.GetComponent<DestructObject>().DestroyObj();
            Destroy(gameObject);
        }
        
    }
    
}
