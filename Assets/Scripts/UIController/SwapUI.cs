using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwapUI : MonoBehaviour
{
    public RectTransform rectTransform;
    private GameManager gameManager;
    private Inventory inventory;

    public Image[] Slot;
    void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void Update()
    {
        sellectSlot();
    }
    public void ElementImg()
    {
        for (int i = 0; i < inventory.HasWeapon.Length; i++)
        {
            if (inventory.HavingElement[i].ElementalID != 0)
            {
                if (inventory.HavingElement[i].WeaponTypes == WeaponTypes.Sword)
                {
                    Slot[i].sprite = gameManager.Elemental.elementalDatas[1].elementalImg;
                }
                else if (gameManager.inventory.HavingElement[i].WeaponTypes == WeaponTypes.Wand)
                {
                    Slot[i].sprite = gameManager.Elemental.elementalDatas[2].elementalImg;
                }
                else if (gameManager.inventory.HavingElement[i].WeaponTypes == WeaponTypes.Shield)
                {
                    Slot[i].sprite = gameManager.Elemental.elementalDatas[3].elementalImg;
                }
                else if (gameManager.inventory.HavingElement[i].WeaponTypes == WeaponTypes.Bow)
                {
                    Slot[i].sprite = gameManager.Elemental.elementalDatas[4].elementalImg;
                }
            }
            Slot[3].sprite = gameManager.Elemental.elementalDatas[(int)gameManager.ObjData.WeaponType].elementalImg;
        }
    }
    public void sellectSlot()
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
            gameManager.slot = 0;
        }
        else if (rectTransform.anchoredPosition.x == 0)
        {
            gameManager.slot = 1;
        }
        else if (rectTransform.anchoredPosition.x == 562)
        {
            gameManager.slot = 2;
        }
    }
}
