using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAtkCol : MonoBehaviour
{
    [SerializeField] Battle battle;    

    void Awake()
    {
        if(!battle)
            battle = GetComponentInParent<Battle>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("Destruct"))
        {
            battle.Atk(other.gameObject);
        }
    }
}
