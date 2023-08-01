using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    
    public enum InteractObjects
    {
        NPC,
        Portal,
        Shop
    }

    [SerializeField] private InteractObjects ObjType;
    public int objectID;
    public string objectTag;
    public GameObject interactView;

    

    void Awake()
    {
        objectTag = gameObject.tag;







        switch(ObjType)
        {
            case InteractObjects.NPC:
                // GameManager.instance.Action(gameObject);
                break;

            case InteractObjects.Portal:
                // Portal(objectID);
                break;

            case InteractObjects.Shop:
                
                break;
        }


    }



    public void Interaction()
    {
        switch(objectTag)
        {
            case "NPC":
                GameManager.instance.Action(gameObject);
                break;

            case "Portal":
                Portal(objectID);
                break;

            case "Shop":
                
                break;
        }


        switch(ObjType)
        {
            case InteractObjects.NPC:
                GameManager.instance.Action(gameObject);
                break;

            case InteractObjects.Portal:
                Portal(objectID);
                break;

            case InteractObjects.Shop:
                
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

            case 20:
                GameManager.instance.MainstageRoad();
                break;

        }
    }




    public void InteractView(bool isOn)
    {
        interactView.SetActive(isOn);
    }


}