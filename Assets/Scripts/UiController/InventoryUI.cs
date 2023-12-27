using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public RectTransform rectTransform;
    private GameManager gameManager;
    [Header("Inventory Setting")]
    public GameObject InvenUI;
    public GameObject[] ElementInfo;
    public GameObject[] ItemInfo;
    public bool isInven = false;
    public float Speed;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    void Start()
    {
        for (int i = 0; i < ElementInfo.Length; i++) { ElementInfo[i].SetActive(false); }
        for (int i = 0; i < ItemInfo.Length; i++) { ItemInfo[i].SetActive(false); }
        InvenUI.SetActive(isInven);
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ElementImg();
            SetItem();
            isInven = !isInven;
            InvenUI.SetActive(isInven);
        }
        MoveElementsCard();
    }
    public void MoveElementsCard()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (rectTransform.anchoredPosition.x == 768 && rectTransform.anchoredPosition.y == 13)
            {
                rectTransform.anchoredPosition = new Vector3(381, 13, 0);
            }
            else if (rectTransform.anchoredPosition.x == 381 && rectTransform.anchoredPosition.y == 13)
            {
                rectTransform.anchoredPosition = new Vector3(574, -31, 2);
                rectTransform.SetAsLastSibling();
            }
            else if (rectTransform.anchoredPosition.x == 574 && rectTransform.anchoredPosition.y == -31)
            {
                rectTransform.anchoredPosition = new Vector3(768, 13, 0);
            }
        }
    }
    public void ElementImg()
    {
        for (int i = 0; i < gameManager.inventory.HasWeapon.Length; i++)
        {
            if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Sword)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[1];
            }
            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Wand)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[2];
            }
            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Shield)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[3];
            }
            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Bow)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[4];
            }          
        }
    }
    public void SetItem()
    {
        for(int i = 0; i<gameManager.inventory.HavingItem.Length; i++)
        {
            for (int j = 0; j < gameManager.Item.itemDatas.Count; j++)
            {
                if (gameManager.inventory.HavingItem[i] == gameManager.Item.itemDatas[j]  )
                {
                    gameManager.InvenItem[i] = gameManager.inventory.HavingItem[i].itemImg;
                }
                else
                {
                    gameManager.InvenItem[i] = null; 
                }
            }
        }
        
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if( collision.tag == "ElementalCard" && EventSystem.current.IsPointerOverGameObject()) 
    //    {
    //        for( int i = 0; i < gameManager.inventory.HasWeapon.Length; i++)
    //        {   if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Sword)
    //            { 
    //                ElementInfo[0].SetActive(true);
    //                ElementInfo[0].transform.position = Input.mousePosition;
    //            } 
    //            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Wand)
    //            {
    //                ElementInfo[1].SetActive(true);
    //            }
    //            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Shield)
    //            {
    //                ElementInfo[2].SetActive(true);
    //            }
    //            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Bow)
    //            {
    //                ElementInfo[3].SetActive(true);
    //            }
    //            else
    //            {
    //                ElementInfo[i].SetActive(false);
    //            }
    //        }
           
    //    }
    //    else if( collision.tag == "ItemSlot")
    //    {
    //        ItemInfo[].SetActive(false);
    //    }
    //}
    

}
