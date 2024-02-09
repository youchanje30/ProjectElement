using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCheck : MonoBehaviour
{
    [SerializeField] bool hit;

    void Awake()
    {
        hit = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -30f && !hit)
        {

            if(gameObject.CompareTag("Player"))
            {
                hit = true;
                GetComponent<Battle>().GetDamaged(100000);
            }
            else if(gameObject.CompareTag("Monster"))
            {
                hit = true;
                GetComponent<MonsterBase>().GetDamaged(100000, false);
            }
        }
    }
}
