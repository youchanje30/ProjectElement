using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tooltip tooltip;
    public InventoryUI inventoryUI;
    void Start()
    {
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
       for (int i = 0; i < inventoryUI.Card.Length; i++)
        {
            if (inventoryUI.elementalData != null && inventoryUI.Card[i].gameObject == eventData.pointerCurrentRaycast.gameObject) 
            {
                inventoryUI.Info.SetActive(true);
                tooltip.SetupTooltip(inventoryUI.inventory.HavingElement[i].ElementalName, inventoryUI.inventory.HavingElement[i].ElementalInfo);
                break;
            }
        }
      
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.Info.SetActive(false);
    }
}
