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
        if(other.tag == "NPC" || other.tag == "Portal") // || other.gameObject.tag == "NPC" )
        {
            if(scanObj != null)
                scanObj.gameObject.GetComponent<ObjectController>().InteractView(false);

            

            scanObj = other.gameObject;
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(true);
        }
        else if(other.tag == "Coin")
        {
            //골드 획득 시스템
            inven.GetGold();
            Destroy(other.transform.parent.gameObject);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if(scanObj == null && (other.gameObject.tag == "NPC" || other.gameObject.tag == "Portal"))
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
