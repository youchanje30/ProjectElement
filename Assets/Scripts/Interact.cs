using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject scanObj;

    public bool isActionning;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Untagged" && other.gameObject.tag != "Monster")
        {
            if(scanObj != null)
                scanObj.gameObject.GetComponent<ObjectController>().InteractView(false);

            scanObj = other.gameObject;
            scanObj.gameObject.GetComponent<ObjectController>().InteractView(true);

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(scanObj == null && other.gameObject.tag != "Untagged" && other.gameObject.tag != "Monster")
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
            Debug.Log("interaction");
        }
    }

}
