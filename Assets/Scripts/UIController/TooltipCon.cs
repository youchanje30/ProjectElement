using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipCon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
            if (inventoryUI.inventory.HavingElement[i].ElementalID != 0 && inventoryUI.Card[i].gameObject == eventData.pointerCurrentRaycast.gameObject || inventoryUI.EleCards[i].gameObject == eventData.pointerCurrentRaycast.gameObject)
            {
                inventoryUI.Info.SetActive(true);
                tooltip.SetupTooltip(inventoryUI.inventory.HavingElement[i].ElementalName, inventoryUI.inventory.HavingElement[i].ElementalInfo);
                break;
            }
        }
        for (int i = 0; i < inventoryUI.InvenItem.Length; i++)
        {
            if (inventoryUI.inventory.HavingItem[i] != null && inventoryUI.Itempos[i].gameObject == eventData.pointerCurrentRaycast.gameObject || inventoryUI.InvenItem[i].gameObject == eventData.pointerCurrentRaycast.gameObject)
            {
                inventoryUI.ItemInfo.SetActive(true);
                tooltip.SetupTooltip(inventoryUI.inventory.HavingItem[i].ItemName, inventoryUI.inventory.HavingItem[i].ItemInfo);
                break;
            }
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.Info.SetActive(false);
        inventoryUI.ItemInfo.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
 