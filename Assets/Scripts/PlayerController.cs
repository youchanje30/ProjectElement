using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Elements
{
    None, Fire, South, Water, Wind
}

public enum WeaponTypes { Shield, Sword , Bow , Wand };


public class PlayerController : MonoBehaviour
{


    //PlayerController에서 플레이어의 입력을 받음
    //Movement2D 캐릭터의 이동에 관한 스크립트
    //Battle 캐릭터의 전투에 관한 스크립트
    [Header("Player UI")]
    public Slider playerHpBar;
    [SerializeField] Image HpFill;
    public Gradient gradient;
    [Space(20f)]

    //GameManager는 씬의 초기 세팅, 설정 등에 관한 스크립트
    private Movement2D movement2D;
    private PlayerStatus status;
    private Interact interact;
    private Rigidbody2D rigid2D;
    private Battle battle;
    [SerializeField] private GameManager manager;
    private Animator animator;



    [Header("Player Info")]
    public Elements PlayerElementType;
    public WeaponTypes PlayerWeaponType;
    public Inventory inventory;


    [System.Serializable]
    public class ElementAnims
    {
        public RuntimeAnimatorController[] ElementAnim;
    }
    public ElementAnims[] Anims;
    // None, Fire, South, Water, Wind 
    // Shield, Sword, Bow

    [Header("Input Setting")]
    private float hAxis;
    private KeyCode JumpKey = KeyCode.Space;
    private bool pressedJumpkey;
    private KeyCode DashKey = KeyCode.LeftShift;
    private bool pressedDashKey;
    private KeyCode atkKey = KeyCode.Z;
    private bool pressedAtkKey;
    [Space(20f)]

    [Header("Interact Setting")]
    [SerializeField] private KeyCode InteractKey = KeyCode.E;
    private bool pressedInteractKey;

    // [Header("Atk Setting")]
    // [SerializeField] private KeyCode LeftAtkKey = KeyCode.Z;
    // private bool pressedLeftAtkKey;
    // [SerializeField] private KeyCode RightAtkKey = KeyCode.X;
    // private bool pressedRightAtkKey;

    [Header("Swap Setting")]
    [SerializeField] private KeyCode FirstSlot = KeyCode.A;
    private bool pressedFirstSlot;
    [SerializeField] private KeyCode SecondSlot = KeyCode.S;
    private bool pressedSecondSlot;
    [SerializeField] private KeyCode ThirdSlot = KeyCode.D;
    private bool pressedThirdSlot;


    public float chargingTime;

    private bool ischarging;


    void Awake()
    {
        #region Component Access
        movement2D = GetComponent<Movement2D>();
        interact = GetComponent<Interact>();
        rigid2D = GetComponent<Rigidbody2D>();
        battle = GetComponent<Battle>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        status = GetComponent<PlayerStatus>();
        #endregion Component Access


        // animator.runtimeAnimatorController = AnimController[(int)battle.WeaponType];
        chargingTime = 0;

        // animator = 
    }

    void Start()
    {
        SetEquipment();
        battle.WeaponType = PlayerWeaponType;

        playerHpBar.maxValue = status.maxHp;
    }



    void Update()
    {
        InputSystem();
        Act();
        Move();
        PlayerUISystem();
        Swap();
        // ChangeEquipment();
    }

    void PlayerUISystem()
    {
        // playerHpBar.value = battle.curHp;
        playerHpBar.value = Mathf.Lerp(playerHpBar.value, status.curHp, Time.deltaTime * 5f);
        HpFill.color = gradient.Evaluate(playerHpBar.normalizedValue);
    }

    void InputSystem()
    {
        if (manager.isShop && Input.GetKeyDown(KeyCode.Escape))
        {
            manager.isShop = false;
            manager.ShopUI.SetActive(false);
        }

        if (manager.isSpiritAwake && Input.GetKeyDown(KeyCode.Escape))
        {
            manager.isSpiritAwake = false;
            manager.SpiritAwakeUI.SetActive(false);
        }


        // if (battle.WeaponType != WeaponTypes.Sword && !ischarging && Input.GetKeyDown(RightAtkKey) && !battle.fallAtking && !manager.isAction && !manager.isShop && !movement2D.isDashing && !battle.Atking)
        // {
        //     // pressedRightAtkKey = true;
        //     animator.SetBool("isCharge", true);
        //     animator.SetTrigger("Charging");
        //     ischarging = true;
        // }


        // if (battle.WeaponType != WeaponTypes.Sword && ischarging)
        // {
        //     if (Input.GetKey(RightAtkKey))
        //     {

        //         chargingTime += Time.deltaTime;
        //     }

        //     if (Input.GetKeyUp(RightAtkKey))
        //     {
        //         ischarging = false;
        //         pressedRightAtkKey = false;

        //         if (chargingTime < 1f)
        //             chargingTime = 0f;

        //         animator.SetBool("isCharge", false);
        //     }
        // }



        if (movement2D.isDashing || manager.isAction || manager.isShop || manager.isSlotSwap || battle.fallAtking || ischarging || battle.Atking)// || pressedRightAtkKey)//|| battle.Atking)
        {
            pressedDashKey = false;
            pressedJumpkey = false;
            hAxis = 0;
            return;
        }

        pressedDashKey = Input.GetKeyDown(DashKey);
        pressedJumpkey = Input.GetKeyDown(JumpKey);
        hAxis = Input.GetAxisRaw("Horizontal");

        pressedInteractKey = Input.GetKeyDown(InteractKey);

        // pressedLeftAtkKey = Input.GetKeyDown(LeftAtkKey);
        // pressedRightAtkKey = Input.GetKey(RightAtkKey);

        pressedAtkKey = Input.GetKey(atkKey);

        pressedFirstSlot = Input.GetKeyDown(FirstSlot);
        pressedSecondSlot = Input.GetKeyDown(SecondSlot);
        pressedThirdSlot = Input.GetKeyDown(ThirdSlot);


        if (battle.Atking)// || pressedRightAtkKey)
        {
            pressedInteractKey = false;
            // pressedLeftAtkKey = false;
            // pressedRightAtkKey = false;
        }

        /* if(Input.GetKeyDown(InteractKey))
        {

        }
        else if(Input.GetKey(InteractKey))
        {

        }
        else if(Input.GetKeyUp(InteractKey))
        {
            
        } */

        
    }

    public void ChangeAnim()
    {
        animator.runtimeAnimatorController = Anims[(int)PlayerElementType].ElementAnim[(int)PlayerWeaponType];
    }

    public void SetEquipment()
    {
        ChangeAnim();
        status.SetEquipment();
        // battle.ResetStat();
        // if(animator.runtimeAnimatorController == Anims[(int)PlayerElementType].ElementAnim[(int)PlayerWeaponType]) return;
        battle.WeaponType = PlayerWeaponType;
        // SaveManager.instance.Save();

        /*
        for (int i = 0; i < inventory.HavingItem.Length; i++)
        {
            if(inventory.HavingItem[i] != null)
            {
                //체력 증가
                playerStatus.maxHp += inventory.HavingItem[i].HpIncrease;
                // playerStatus.hpPer += inventory.HavingItem[i].HpPerIncrease;
                
                //방어력 증가
                // battle.def += inventory.HavingItem[i].DefIncrease;
                // battle.defPer += inventory.HavingItem[i].DefPerIncrease;

                //물리 데미지 증가
                // battle.meleeDmg += inventory.HavingItem[i].MeleeDmgIncrease;
                // battle.meleePerDmg += inventory.HavingItem[i].MeleeDmgPerIncrease;

                //공격 속도, 크리 확률, 크리 데미지 증가
                playerStatus.atkSpeed += inventory.HavingItem[i].AtkSpeedIncrease;
                playerStatus.crtRate += inventory.HavingItem[i].CrtRateIncrease;
                playerStatus.crtDamage += inventory.HavingItem[i].CrtDmgIncrease;
            }
        }
        */

        animator.SetFloat("AtkSpeed", status.atkSpeed * 0.01f);
    }

    void Act() //상호작용, 공격 스킬 등의 입력을 전달하는 함수
    {
        if (pressedInteractKey && !manager.isAction) interact.InteractObj();

        /*
        if (pressedLeftAtkKey && !manager.isAction) battle.AtkAction(0);

        if (!pressedRightAtkKey && !manager.isAction && chargingTime >= 1f)
        {
            battle.AtkAction(1);
            chargingTime = 0f;
        }
        */


        // if (pressedAtkKey && !manager.isAction)
        // {
        // 
        // }

        /*
        if (battle.WeaponType != WeaponTypes.Sword && ischarging)
        {
                
            if (Input.GetKey(RightAtkKey))
            {

                chargingTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(RightAtkKey))
            {
                ischarging = false;
                pressedRightAtkKey = false;

                if (chargingTime < 1f)
                    chargingTime = 0f;

                animator.SetBool("isCharge", false);
            }
        }
        */
        
        if (battle.fallAtking || manager.isAction || manager.isShop || movement2D.isDashing || battle.Atking)
            return;
        


        
        
        // 행동 불가능한 상황
        if (pressedAtkKey)
        {  
            
            if(battle.WeaponType == WeaponTypes.Sword || !movement2D.isGround)
            {
                battle.AtkAction(0);
                return;
            }
            else
            {
                chargingTime += Time.deltaTime;

                if(!ischarging)
                {
                    animator.SetBool("isCharge", true);
                    animator.SetTrigger("Charging");
                    ischarging = true;
                }
            }
                
        }
        
        bool AtkFin = Input.GetKeyUp(atkKey);

        if(ischarging && (AtkFin || chargingTime >= 1f))
        {
            ischarging = false;
            animator.SetBool("isCharge", false);

            if(chargingTime < 1f)
                battle.AtkAction(0);
            else
                battle.AtkAction(1);

            chargingTime = 0f;
        }
    }

    void Move()
    {
        if (battle.Atking)
        {
            // movement2D.MoveX(hAxis);
            return;
        }

        if (pressedDashKey && movement2D.curDashCnt > 0) StartCoroutine(movement2D.Dash());

        if (!movement2D.isDashing)
        {
            if (pressedJumpkey) movement2D.Jump();
            movement2D.MoveX(hAxis);
        }
    }
    
    void Swap()
    {
        if (pressedFirstSlot)
        {
            PlayerWeaponType = (WeaponTypes)inventory.HavingWeapon[0];
            PlayerElementType = (Elements)inventory.HavingElemental[0];
            battle.WeaponType = (WeaponTypes)inventory.HavingWeapon[1];
            SetEquipment();
        }
        if (pressedSecondSlot)
        {
            PlayerWeaponType = (WeaponTypes)inventory.HavingWeapon[1];
            PlayerElementType = (Elements)inventory.HavingElemental[1];
            battle.WeaponType = (WeaponTypes)inventory.HavingWeapon[1];
            SetEquipment();
        }
        
        if (pressedThirdSlot)
        { 
            PlayerWeaponType = (WeaponTypes)inventory.HavingWeapon[2];
            PlayerElementType = (Elements)inventory.HavingElemental[2];
            battle.WeaponType = (WeaponTypes)inventory.HavingWeapon[1];
            SetEquipment();
        }
        
    }
}
