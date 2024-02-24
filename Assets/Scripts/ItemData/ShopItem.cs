using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopItem : MonoBehaviour
{
    public Inventory inventory;
    public ObjectController objectController;


    public enum ShopType
    {
        Item,
        Consumable
    }


    [SerializeField] private ShopType Type;
    [SerializeField] private int ConsumableItemID;
    private PlayerController player;
    public ItemData Item;

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemInfo;
    private int buyCost;
    private int sellCost;
    [SerializeField] private TMP_Text itemCostTxt;


    [Header("Reverse Setting")]
    [SerializeField] bool isChanging = false;
    [SerializeField] float reverseTime;
    [SerializeField] bool isReverse = false;
    [SerializeField] GameObject afterObj;
    [SerializeField] GameObject beforeObj;
    [SerializeField] Transform reverseObj;


    private int itemID;
    public bool buyBool;


    
    void Awake()
    {
        if(Type == ShopType.Consumable)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
    }

    
    void Update()
    {

    }

    public void Btn()
    {
        if (buyBool) Buy();
        else Sell();
    }


    void Buy()
    {
        if(inventory.Gold < buyCost) return;

        


        if(Type == ShopType.Item)
        {
            if(inventory.isItemFull()) return;
            inventory.GetItem(ItemManager.instance.AddItem(itemID));
            objectController.AddShop(itemID);
        }

        if(Type == ShopType.Consumable)
        {
            UseConsumable(ConsumableItemID);
        }

        inventory.Gold -= buyCost;

        objectController.buyShopObjects.Remove(gameObject.GetComponent<RectTransform>());
        objectController.SetPosShop();
        Destroy(gameObject);
        SaveManager.instance.Save();
        inventory.GetComponent<PlayerStatus>().SetStatue();
    }

    void Sell()
    {
        inventory.Gold += sellCost;
        inventory.RemoveItem(ItemManager.instance.AddItem(itemID));
        objectController.sellShopObjects.Remove(gameObject.GetComponent<RectTransform>());
        objectController.SetPosShop();
        Destroy(gameObject);
        SaveManager.instance.Save();
        inventory.GetComponent<PlayerStatus>().SetStatue();
    }

    public void UseConsumable(int ConsumableID)
    {
        switch (ConsumableID)
        {
            // 체력 회복
            case 10:
                player.GetComponent<Battle>().HealHp(10);
                break;
            // 정령 영혼
            case 20:
                player.GetComponent<Inventory>().SpiritSoul++;
                break;
        }
    }



    public void Setting(ItemData item)
    {
        // inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        Item = item;

        if(Item.itemImg)
            itemImage.sprite = Item.itemImg;
            
        itemID = Item.ItemID;

        buyCost = Item.ItemCost;
        sellCost = Item.SellCost;

        if(buyBool)
            itemCostTxt.text = buyCost.ToString();
        else
            itemCostTxt.text = sellCost.ToString();

        itemName.text = Item.ItemName;
        itemInfo.text = Item.ItemInfo;

        if(afterObj)
        {
            afterObj.SetActive(isReverse);
            afterObj.GetComponent<Image>().sprite = Item.ItemRareAfterImg;
        }
            
        if(beforeObj)
        {
            beforeObj.SetActive(!isReverse);
            beforeObj.GetComponent<Image>().sprite = Item.ItemRareBeforeImg;
        }
    }


    public void ReverseObj()
    {
        if(isChanging) return;
        isChanging = true;
        // DOTween.Clear();
        reverseObj.DORotate(new Vector3(0, 90, 0), reverseTime).SetUpdate(true).OnComplete(() =>
        {
            isReverse = !isReverse;
            afterObj.SetActive(isReverse);
            beforeObj.SetActive(!isReverse);
            reverseObj.DORotate(new Vector3(0, 0, 0), reverseTime).SetUpdate(true).OnComplete(()=>
            {
                isChanging = false;
            });
        });

        // if(isReverse)
        // {
            
        // }
        // else
        // {
        //     transform.DORotate(new Vector3(0, 0, 0), reverseTime);
        // }

    }
}
