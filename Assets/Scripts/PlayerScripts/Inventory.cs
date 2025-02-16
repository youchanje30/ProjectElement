using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Money Setting")]
    public int Gold = 0;
    public int SpiritSoul = 0;
    public ItemData[] HavingItem;
    public ElementalData[] HavingElement;
    public bool[] HasWeapon;
    

    [Header("코인 1개 당 얻는 골드 양")]
    public int getCoin = 1;

    public void Update()
    {
        CheckWeapon();
    }
    public void GetGold()
    {
        Gold += getCoin;
        SaveManager.instance.Save();
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < HavingItem.Length; i++)
        {
            if (!HavingItem[i]) continue;

            if (HavingItem[i].ItemID != item.ItemID) continue;

            HavingItem[i] = null;
        }
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
    public void GetEle(ElementalData ele)
    {

        for (int i = 0; i < HavingElement.Length; i++)
        {
            if (HavingElement[i].ElementalID == 0)
            {
                HavingElement[i] = ele;
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
    public void CheckWeapon()
    {
        for (int i = 0; i < HasWeapon.Length; i++)
        {
            if (HavingElement[i].WeaponTypes == 0)
            {
                HasWeapon[i] = false;
            }
            else
            {
                HasWeapon[i] = true;
            }
        }
    }
}