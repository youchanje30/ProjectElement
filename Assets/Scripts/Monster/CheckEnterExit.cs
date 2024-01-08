using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnterExit : MonoBehaviour
{
    [SerializeField] DetectPlayer detect;
    public bool isEnterTrigger;
    

    void Awake()
    {
        if(!detect)
            detect = GetComponentInParent<DetectPlayer>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isEnterTrigger = true;
            detect.AddCheck();
        }
    }    

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isEnterTrigger = false;
            detect.SubtractCheck();
        }
            
    }
}
