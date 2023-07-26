using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    
    public int objectID;
    public string objectTag;
    public GameObject interactView;

    

    void Awake()
    {
        objectTag = gameObject.tag;    
    }

    public void Interaction()
    {
        switch(objectTag)
        {
            case "NPC":
                GameManager.instance.Action(gameObject);
                // Debug.Log("Action");
                break;

            case "Portal":
                Portal(objectID);
                break;

        }
    }


    public void Portal(int ID)
    {
        //Chapter Stage 이동은 ID 10
        //
        switch (ID)
        {
            case 10:
                GameManager.instance.RandomStageRoad();
                break;


        }


    }


    public void InteractView(bool isOn)
    {
        interactView.SetActive(isOn);
    }


}
