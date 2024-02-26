using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    bool canHit;
    [SerializeField] float damage;

    [SerializeField] Battle battle;

    void EnableAtk()
    {
        canHit = true;
    }

    void OnDisable()
    {
        canHit = false;
    }

    void Update()
    {
        if(battle != null && canHit)
        {
            battle.GetDamaged(damage);
            Debug.Log("Hit " + gameObject.name);
            
            canHit = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            battle = other.GetComponent<Battle>();   
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            battle = null;
        }
    }
}
