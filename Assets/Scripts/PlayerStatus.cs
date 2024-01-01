using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 플레이어 스탯에 관한 스크립트
    // 총 4가지 스탯으로 나눌 예정이다.


    // 기본 스탯
    // 증가한 스탯
    // 최종 스탯

    // 사용은 무조건 최종 스탯으로 작동한다.

    #region 스테이터스 묶음표
    [Header("스테이터스 묶음표")]
    [Tooltip("힘")]
    public float strength;
    [Tooltip("민첩")]
    public float dexterity;
    [Tooltip("행운")]
    public float luck;
    [Space(20f)]
    #endregion

    [Header("플레이어 초기 스텟")]
    #region 플레이어 기본 스탯 정의 - 초기 스탯
    public float basicMaxHp = 10f; 
    public float basicHpPer = 100f;
    public float basicDamage;
    public float basicDamagePer = 100f;
    public float basicAtkSpeed = 100f;
    public float basicCrtRate = 10f;
    public float basicCrtDamage = 150f;
    public float basicJumpForce = 15f;
    public float basicJumpForcePer = 100f;
    public float basicMissRate = 5f;
    public float basicDefPer = 0f;
    public float basicPlayerSpeed = 8f;
    public float basicCoolDownReductionPer = 0f;
    [Space(20f)]
    #endregion

    #region 증가한 스탯
    // 스테이터스 묶음표 값에 따른 증가량
    [Header("플레이어 스탯")]
    private float increaseMaxHp;          
    private float increaseHpPer;                
    private float increaseDamage;               
    private float increaseDamagePer;
    private float increaseAtkSpeed;             
    private float increaseCrtRate;              
    private float increaseCrtDamage;               
    private float increaseJumpForce;               
    private float increaseJumpForcePer;              
    private float increaseMissRate; 
    private float increaseDefPer;
    private float increasePlayerSpeed;
    private float increaseCoolDownReductionPer; 
    [Space(40f)]
    #endregion
    
    #region 최종 스탯(사용하는 스탯)
    [Header("최종 플레이어 스탯")]
    public float curHp;                 // 현재 체력
    public float maxHp;                 // 최대 체력
    public float damage;                // 데미지
    public float atkSpeed;              // 공격 속도
    public float crtRate;               // 크리티컬 확률
    public float crtDamage;             // 크리티컬 데미지
    public float missRate;              // 회피 확률
    public float coolDownReductionPer;  // 쿨타임 감소
    public float defPer;                // 데미지 감소%

    public float playerSpeed;           // 이동 속도
    public float jumpForce;              // 점프력
    [Space(20f)]
    #endregion


    #region 참조 스크립트
    private Inventory inventory;
    #endregion


    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    void Start()
    {
        SetStatue();
        curHp = maxHp;
    }

    // 데미지 반환하는 함수
    public float AtkDamage()
    {
        bool isCrt = Random.Range(1, 100 + 1) <= crtRate;
        float finalDmg = isCrt ? damage * crtDamage * 0.01f : damage;

        return finalDmg;
    }

    // 스탯 재적용 관련 스크립트
    public void SetStatue()
    {
        SetEquipment();

        maxHp = (basicMaxHp + increaseMaxHp) * (basicHpPer + increaseHpPer) * (0.01f);
        damage = (basicDamage + increaseDamage) * (basicDamagePer + increaseDamagePer) * (0.01f);
        crtRate = basicCrtRate + increaseCrtRate;
        crtDamage = basicCrtDamage + increaseCrtDamage;
        atkSpeed = basicAtkSpeed + increaseAtkSpeed;
        missRate = basicMissRate + increaseMissRate;
        coolDownReductionPer = basicCoolDownReductionPer + increaseCoolDownReductionPer;
        defPer = basicDefPer + increaseDefPer;

        playerSpeed = basicPlayerSpeed + increasePlayerSpeed;
        jumpForce = (basicJumpForce + increaseJumpForce) * (basicJumpForcePer + increaseJumpForcePer) * 0.01f;
    }
    

    // 장비 착용 증가 값 초기화
    public void SetEquipment()
    {
        // Increase 값 모두 초기화
        increaseMaxHp = 0;          
        increaseHpPer = 0;                
        increaseDamage = 0;               
        increaseDamagePer = 0;
        increaseAtkSpeed = 0;             
        increaseCrtRate = 0;              
        increaseCrtDamage = 0;               
        increaseJumpForce = 0;               
        increaseJumpForcePer = 0;              
        increaseMissRate = 0; 
        increaseDefPer = 0;
        increasePlayerSpeed = 0;
        increaseCoolDownReductionPer = 0; 

        // 장비 증가 값 Increase에 추가
        for (int i = 0; i < inventory.HavingItem.Length; i++)
        {
            if(inventory.HavingItem[i] == null) continue;
            
            ItemData item = inventory.HavingItem[i];

            increaseHpPer += item.HpPer;
            increaseMaxHp += item.Hp;
            
            increaseDamage += item.Damage;
            increaseDamagePer += item.DamagePer;

            increaseAtkSpeed += item.AtkSpeed;

            increaseCrtRate += item.CrtRate;
            increaseCrtDamage += item.CrtDamage;

            increaseMissRate += item.MissRate;

            increaseDefPer += item.DefPer;

            increasePlayerSpeed += item.PlayerSpeed;

            increaseCoolDownReductionPer += item.CoolDownReductionPer;

            increaseJumpForce += item.JumpForce;
            increaseJumpForcePer += item.JumpForcePer;
        }

        // 스테이터스(힘 민첩 행운) 값 Increase에 추가
        
        increaseDamage += 2 * strength + 2 * dexterity;
        increaseMaxHp += strength;
        increaseAtkSpeed += dexterity;
        
        increaseCrtRate += luck;
        increaseCrtDamage += luck;
        increaseMissRate += luck;

        // 체력 연동

        
    }


}