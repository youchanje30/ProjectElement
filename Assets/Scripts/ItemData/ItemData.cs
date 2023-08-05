using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemRares
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemData : ScriptableObject
{

    


    [Header("Item Info")]
    [SerializeField] private string itemName;
    public string ItemName { get { return itemName; } }
    [SerializeField] private string itemInfo;
    public string ItemInfo { get { return itemInfo; } }
    [SerializeField] private ItemRares itemRare;
    public ItemRares ItemRare { get { return itemRare; } }
    [SerializeField] private int itemID;
    public int ItemID { get { return itemID; } }
    public Image itemImg;
    [Space(20f)]


    [Header("Battle Stat Increase Data")]
    [Tooltip("체력 증가량")]
    [SerializeField] private int hpIncrease;
    public int HpIncrease { get { return hpIncrease; } }
    [Tooltip("체력% 증가량")]
    [SerializeField] private int hpPerIncrease;
    public int HpPerIncrease { get { return hpPerIncrease; } }
    [Tooltip("방어력 증가량")]
    [SerializeField] private int defIncrease;
    public int DefIncrease {get { return defIncrease; } }
    [Tooltip("방어력% 증가량")]
    [SerializeField] private int defPerIncrease;
    public int DefPerIncrease {get { return defPerIncrease; } }
    [Tooltip("물리 데미지 증가량")]
    [SerializeField] private int meleeDmgIncrease;
    public int MeleeDmgIncrease {get { return meleeDmgIncrease; } }
    [Tooltip("물리 데미지% 증가량")]
    [SerializeField] private int meleeDmgPerIncrease;
    public int MeleeDmgPerIncrease {get { return meleeDmgPerIncrease; } }
    [Tooltip("스킬 데미지 증가량")]
    [SerializeField] private int skillDmgIncrease;
    public int SkillDmgIncrease{get { return skillDmgIncrease; } }
    [Tooltip("스킬 데미지% 증가량")]
    [SerializeField] private int skillDmgPerIncrease;
    public int SkillDmgPerIncrease{get { return skillDmgPerIncrease; } }
    [Tooltip("공격 속도% 증가량")]
    [SerializeField] private int atkSpeedIncrease;
    public int AtkSpeedIncrease { get { return atkSpeedIncrease; } }
    [Tooltip("크리티컬 확률% 증가량")]
    [SerializeField] private int crtRateIncrease;
    public int CrtRateIncrease { get { return crtRateIncrease; } }
    [Tooltip("크리티컬 데미지% 증가량")]
    [SerializeField] private int crtDmgIncrease;
    public int CrtDmgIncrease { get { return crtDmgIncrease; } }



}
