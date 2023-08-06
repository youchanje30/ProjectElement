using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public Inventory inventory;
    public ObjectController objectController;


    public ItemData Item;

    public Image itemImage;

    public TMP_Text itemName;
    public TMP_Text itemInfo;
    public int itemCost;
    public TMP_Text itemCostTxt;
    
    public TMP_Text itemRare;

    public int itemID;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy()
    {
        if(inventory.Gold < itemCost) return;
        if(inventory.isItemFull()) return;

        inventory.Gold -= itemCost;

        
        inventory.GetItem(ItemManager.instance.AddItem(itemID));

        // ItemManager.instance.AddItem(itemID);
        // this.gameObject.SetActive(false);
        objectController.shopObjects.Remove(gameObject.GetComponent<RectTransform>());
        objectController.SetPosShop();
        Destroy(gameObject);
        SaveManager.instance.Save();
    }

    public void Setting(ItemData item)
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        Item = item;

        itemImage = Item.itemImg;
        itemID = Item.ItemID;

        itemName.text = Item.ItemName;
        // itemCost.text = Item.item
        itemInfo.text = Item.ItemInfo;
        itemRare.text = Item.ItemRare.ToString();
    }
}
