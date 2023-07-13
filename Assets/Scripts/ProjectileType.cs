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
        if(other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Monster")
        {
            other.gameObject.GetComponent<Monster>().GetDamaged(Damage);
            Destroy(gameObject);
        }
    }
    
}
