using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public ItemData Item;

    public Image itemImage;

    public TMP_Text itemName;
    public TMP_Text itemInfo;
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

    }

    public void Setting(ItemData item)
    {
        Item = item;

        itemImage = Item.itemImg;
        itemID = Item.ItemID;

        itemName.text = Item.ItemName;
        // itemCost.text = Item.item
        itemInfo.text = Item.ItemInfo;
        itemRare.text = Item.ItemRare.ToString();
    }
}
