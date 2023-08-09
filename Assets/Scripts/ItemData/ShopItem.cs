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

    public Image itemImage;

    public TMP_Text itemName;
    public TMP_Text itemInfo;
    public int itemCost;
    public TMP_Text itemCostTxt;
    
    public TMP_Text itemRare;

    public int itemID;
    


    
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

    public void Buy()
    {
        if(inventory.Gold < itemCost) return;

        


        if(Type == ShopType.Item)
        {
            if(inventory.isItemFull()) return;
            inventory.GetItem(ItemManager.instance.AddItem(itemID));
        }

        if(Type == ShopType.Consumable)
        {
            UseConsumable(ConsumableItemID);
        }

        inventory.Gold -= itemCost;

        // ItemManager.instance.AddItem(itemID);
        // this.gameObject.SetActive(false);
        objectController.shopObjects.Remove(gameObject.GetComponent<RectTransform>());
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

        itemName.text = Item.ItemName;
        // itemCost.text = Item.item
        itemInfo.text = Item.ItemInfo;
        itemRare.text = Item.ItemRare.ToString();
    }
}
