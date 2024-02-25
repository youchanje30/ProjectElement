using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject scanObj;
    private Inventory inven;
    public bool isActionning;

    void Awake()
    {
        inven = GetComponent<Inventory>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tag를 변경하는게 좋아보인다.
        if(other.CompareTag("NPC") || other.CompareTag("Portal") || other.CompareTag("Shop") || other.CompareTag("Weapon"))
        {
            if(scanObj != null)
            {
                scanObj.gameObject.GetComponent<ObjectController>().InteractView(false);
            }
                

            

            scanObj = other.gameObject;
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(true);
        }
        else if(other.CompareTag("Coin"))
        {
            //골드 획득 시스템
            inven.GetGold();
            Destroy(other.transform.parent.gameObject);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if(scanObj == null && (other.CompareTag("NPC") || other.CompareTag("Portal") || other.CompareTag("Shop") || other.CompareTag("Weapon")))
        {
            scanObj = other.gameObject;
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if(scanObj == other.gameObject)
        {
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(false);
            scanObj = null;
        }
    }


    public void InteractObj()
    {
        if(scanObj != null)
        {
            scanObj.gameObject.GetComponent<ObjectController>().Interaction();
        }
    }

}
