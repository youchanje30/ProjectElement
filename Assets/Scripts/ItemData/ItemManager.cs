using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private List<ItemData> ItemDatas;
    [SerializeField] private GameObject ItemPrefab;

    

    public List<ItemData> CommonItemDatas = new List<ItemData>();
    public List<ItemData> RareItemDatas = new List<ItemData>();
    public List<ItemData> EpicItemDatas = new List<ItemData>();
    





    void Awake()
    {
        instance = this;
        SortingItems();
    }

    
    public void SortingItems()
    {
        foreach(ItemData item in ItemDatas)
        {
            switch (item.ItemRare)
            {
                case ItemRares.Common:
                    CommonItemDatas.Add(item);
                    break;

                case ItemRares.Rare:
                    RareItemDatas.Add(item);
                    break;
                    
                case ItemRares.Epic:
                    EpicItemDatas.Add(item);
                    break;                
            }
        }
    }


    // 일반 등급은 1000부터, 고급은 2000, 영웅은 3000

    public ItemData AddItem(int ID)
    {
        if(ID == 0) return null;

        ItemData AddingItem = null;

        switch ((int)(ID/1000))
        {
            case 1:
                foreach (ItemData item in CommonItemDatas)
                {
                    if(item.ItemID == ID)
                    {
                        AddingItem = item;
                        break;
                    }
                }
                break;
            
            case 2:
                foreach (ItemData item in RareItemDatas)
                {
                    if(item.ItemID == ID)
                    {
                        AddingItem = item;
                        break;
                    }
                }
                break;


            case 3:
                foreach (ItemData item in EpicItemDatas)
                {
                    if(item.ItemID == ID)
                    {
                        AddingItem = item;
                        break;
                    }
                }
                break;
        }


        Debug.Log(AddingItem);
        return AddingItem;
    }


    // public 

}
