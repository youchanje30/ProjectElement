using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour,IDropHandler
{
    GameManager gameManager;
    RectTransform rect;
    public InventoryUI inventoryUI;
    void Start()
    {
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    void Awake()
    { 
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        
    }
   public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null) 
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            if (inventoryUI.screenPoint.x  < 283)
            {
               gameManager.swapUI.slot = 0;
            }
        else if (inventoryUI.screenPoint.x >= 543.5)
            {
                gameManager.swapUI.slot = 2;
            }
            else { gameManager.swapUI.slot = 1; }

            gameManager.Elements[gameManager.swapUI.slot].gameObject.SetActive(true);
            inventoryUI.inventory.HavingElement[gameManager.swapUI.slot] = inventoryUI.gameManager.Elemental.AddElement(int.Parse(transform.GetChild(0).GetComponent<Text>().text));
           
        }       
    }

  
}
