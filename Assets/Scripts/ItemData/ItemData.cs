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

    [SerializeField] private int itemCost;
    public int ItemCost { get { return itemCost; } }
    [SerializeField] private int sellCost;
    public int SellCost { get { return sellCost; } }
    [SerializeField] private int itemID;
    public int ItemID { get { return itemID; } }
    public Image itemImg;
    [Space(40f)]







    [Header("Battle Stat Increase Data")]
    [Tooltip("체력 증가량")]
    [SerializeField] private float hp;
    public float Hp { get { return hp; } }
    [Tooltip("체력% 증가량")]
    [SerializeField] private float hpPer;
    public float HpPer { get { return hpPer; } }
    
    [Tooltip("데미지 증가량")]
    [SerializeField] private float damage;
    public float Damage { get { return damage; } }
    [Tooltip("데미지% 증가량")]
    [SerializeField] private float damagePer;
    public float DamagePer { get { return damagePer; } }
    
    [Tooltip("공격 속도% 증가량")]
    [SerializeField] private float atkSpeed;
    public float AtkSpeed { get { return atkSpeed; } }

    [Tooltip("크리티컬 확률% 증가량")]
    [SerializeField] private float crtRate;
    public float CrtRate { get { return crtRate; } }
    [Tooltip("크리티컬 데미지% 증가량")]
    [SerializeField] private float crtDamage;
    public float CrtDamage { get { return crtDamage; } }

    [Tooltip("회피확률 증가량")]
    [SerializeField] private float missRate;
    public float MissRate { get { return missRate; } }

    [Tooltip("피해 감소 증가량")]
    [SerializeField] private float defPer;
    public float DefPer { get { return defPer; } }


    [Tooltip("이동 속도 증가량")]
    [SerializeField] private float playerSpeed;
    public float PlayerSpeed { get { return playerSpeed; } }

    [Tooltip("쿨타임 감소 증가량")]
    [SerializeField] private float coolDownReductionPer;
    public float CoolDownReductionPer { get { return coolDownReductionPer; } }
    
    [Tooltip("점프력 증가량")]
    [SerializeField] private float jumpForce;
    public float JumpForce { get { return jumpForce; } }
    [Tooltip("점프력% 증가량")]
    [SerializeField] private float jumpForcePer;
    public float JumpForcePer { get { return jumpForcePer; } }
}
