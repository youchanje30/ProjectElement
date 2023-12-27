using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject scanObj;
    private Inventory inven;
    public GameObject ScanObj;
    public bool isActionning;


    void Awake()
    {
        inven = GetComponent<Inventory>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tag를 변경하는게 좋아보인다.
        if(other.tag == "NPC" || other.tag == "Portal" || other.tag == "Shop" || other.tag == "Weapon" || other.tag == "SpiritAwake") // || other.gameObject.tag == "NPC" )
        {
            if(scanObj != null)
            {
                scanObj.gameObject.GetComponent<ObjectController>().InteractView(false);
                ScanObj = scanObj;
            }
                

            

            scanObj = other.gameObject;
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(true);
            ScanObj = scanObj;
        }
        else if(other.tag == "Coin")
        {
            //골드 획득 시스템
            inven.GetGold();
            Destroy(other.transform.parent.gameObject);
        }
        else if(other.tag == "SpiritSoul")
        {
            //정수 획득 시스템
            inven.GetSpiritSoul();
            Destroy(other.transform.parent.gameObject);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if(scanObj == null && (other.gameObject.tag == "NPC" || other.gameObject.tag == "Portal" || other.tag == "Shop" || other.tag == "Weapon" || other.tag == "SpiritAwake"))
        {
            scanObj = other.gameObject;
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(true);
            ScanObj = scanObj;
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
