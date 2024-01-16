using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Arrow, Magic
}

public class ProjectileType : MonoBehaviour
{
    public Type Projectile;
    private CapsuleCollider2D Collider2D;
    public float Damage;
    public float moveSpeed;
    public Transform target;


    public float duration;
    public float tick;
    public float per;


    void Update()
    {
        Move();
        
    }

    void Move()
    {
        switch (Projectile)
        {
            case Type.Arrow:
                transform.position += new Vector3(-(transform.localScale.x),0,0) * Time.deltaTime * moveSpeed;
                break;
            
            case Type.Magic:
                if(target != null)
                    transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
                else
                    transform.position += new Vector3(-(transform.localScale.x),0,0) * Time.deltaTime * moveSpeed;

                
                break;
        }
    }

    private void Awake()
    {
        Collider2D = GetComponent<CapsuleCollider2D>();
        Invoke("Remove", 5f);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log(other.gameObject.tag);
        if(other.tag == "Floor")
        {
            Destroy(gameObject);
        }
        else if(other.tag == "Monster")
        {
            // other.GetComponent<Monster>().GetDamaged(Damage);
            other.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
            if(Projectile == Type.Magic)
                other.GetComponentInParent<MonsterDebuffBase>().ContinueBuff(0f, duration, tick, BuffTypes.Slow, per);

            Destroy(gameObject);
        }
        else if(other.tag == "Destruct")
        {
            other.GetComponent<DestructObject>().DestroyObj();
            Destroy(gameObject);
        }
        
    }
    
    void Remove()
    {
        Destroy(gameObject);
    }
}
