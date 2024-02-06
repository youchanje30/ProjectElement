using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwapUI : MonoBehaviour
{
    public RectTransform rectTransform;
    private GameManager gameManager;
    private Inventory inventory;
    public GameObject SlotSwapUI;
    public int slot;
    public Image[] Slot;
    public TMP_Text[] eleInfo;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void Update()
    {
        if (gameManager.isSlotSwap) { sellectSlot(); }

    }
    public void ElementImg()
    {   
        for (int i = 0; i < inventory.HasWeapon.Length; i++)
        {
            Slot[i].sprite = inventory.HavingElement[i].elementalImg;
        }
        Slot[3].sprite = gameManager.Elemental.elementalDatas[(int)gameManager.ObjData.WeaponType].elementalImg;
        
    }
    public void sellectSlot()
    {     if (SlotSwapUI.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && rectTransform.anchoredPosition.x < 562)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + 562, -277);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && rectTransform.anchoredPosition.x > -562)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - 562, -277);
            }

            if (rectTransform.anchoredPosition.x == -562)
            {
                slot = 0;
            }
            else if (rectTransform.anchoredPosition.x == 0)
            {
                slot = 1;
            }
            else if (rectTransform.anchoredPosition.x == 562)
            {
                slot = 2;
            }
        }
    }
    public void SetInformation()
    {
        for(int i = 0;i<3;i++)
        {
            eleInfo[i].text = inventory.HavingElement[i].ElementalName + "\n" + inventory.HavingElement[i].ElementalInfo;
        }
        eleInfo[3].text = gameManager.Elemental.elementalDatas[(int)gameManager.ObjData.WeaponType].ElementalName + "\n" + gameManager.Elemental.elementalDatas[(int)gameManager.ObjData.WeaponType].ElementalInfo;
    }
}
