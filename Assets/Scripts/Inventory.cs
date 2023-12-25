using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Money Setting")]
    public int Gold = 0;
    public int SpiritSoul = 0;
    public ItemData[] HavingItem;
    public int[] HavingElemental = { 5 };
    public int[] HavingWeapon = { 5 };
    public bool[] HasWeapon;


    public void GetGold()
    {
        Gold++;
        SaveManager.instance.Save();
    }

    public void GetSpiritSoul()
    {
        SpiritSoul++;
        SaveManager.instance.Save();
    }


    public void GetItem(ItemData item)
    {
        for (int i = 0; i < HavingItem.Length; i++)
        {
            if(HavingItem[i] == null)
            {
                HavingItem[i] = item;
                break;
            }
        }
    }

    public bool isItemFull()
    {
        for (int i = 0; i < HavingItem.Length; i++)
        {
            if(HavingItem[i] == null)
            {
                return false;
            }
        }

        return true;
    }

    public int PlayerGold()
    {
        return Gold;
    }
}