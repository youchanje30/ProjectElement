using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private List<ItemData> ItemDatas;

    

    public List<ItemData> CommonItemDatas = new List<ItemData>();
    public List<ItemData> RareItemDatas = new List<ItemData>();
    public List<ItemData> EpicItemDatas = new List<ItemData>();
    

    
    [SerializeField] private int[] ItemGetPercent;




    void Awake()
    {
        instance = this;
        SortingItems();
    }

    [Tooltip("아이템 정렬하는 시스템")]
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
    [Tooltip("ID 값에 해당하는 아이템을 반환해주는 함수")]
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


        // Debug.Log(AddingItem);
        return AddingItem;
    }



    
    [Tooltip("랜덤하게 아이템 데이터를 반환하는 함수")]
    public ItemData GetShopItem()
    {
        ItemData ReturnItemData = null;
        int RandomValue = Random.Range(1, 100 + 1);
        int getItemIndex = -1;
        for (int i = 0, j = 0; i < ItemGetPercent.Length; i++)
        {
            j += ItemGetPercent[i];
            
            if(RandomValue <= j)
            {
                // 위 아이템이 뜸
                getItemIndex = i;
                break;
            }
        }
        
        switch (getItemIndex)
        {
            case 0:
                ReturnItemData = CommonItemDatas[Random.Range(0, CommonItemDatas.Count)];
                break;
            
            case 1:
                ReturnItemData = RareItemDatas[Random.Range(0, RareItemDatas.Count)];
                break;
            case 2:
                ReturnItemData = EpicItemDatas[Random.Range(0, EpicItemDatas.Count)];
                break;
        }


        return ReturnItemData;
    }

}
