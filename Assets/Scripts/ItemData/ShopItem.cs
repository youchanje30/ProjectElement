using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] TMP_Text btnText;
    
    [SerializeField] TMP_Text itemRare;

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


    public void Buy()
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

        // ItemManager.instance.AddItem(itemID);
        // this.gameObject.SetActive(false);
        objectController.buyShopObjects.Remove(gameObject.GetComponent<RectTransform>());
        objectController.SetPosShop();
        Destroy(gameObject);
        SaveManager.instance.Save();

    }

    public void Sell()
    {
        inventory.Gold += sellCost;
        inventory.RemoveItem(ItemManager.instance.AddItem(itemID));
        objectController.sellShopObjects.Remove(gameObject.GetComponent<RectTransform>());
        objectController.SetPosShop();
        Destroy(gameObject);
        SaveManager.instance.Save();
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

        itemImage = Item.itemImg;
        itemID = Item.ItemID;

        buyCost = Item.ItemCost;
        sellCost = Item.SellCost;

        if(buyBool)
            itemCostTxt.text = buyCost.ToString();
        else
            itemCostTxt.text = sellCost.ToString();


        itemName.text = Item.ItemName;
        // itemCost.text = Item.ItemCost;
        itemInfo.text = Item.ItemInfo;
        itemRare.text = Item.ItemRare.ToString();
    }
}
