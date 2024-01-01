using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum WeaponTypes { None, Sword , Wand , Shield , Bow };


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
    public WeaponTypes PlayerWeaponType;
    public Inventory inventory;


    public RuntimeAnimatorController[] anim;
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
    private bool isRepeatAtk = false;
    [SerializeField] private KeyCode ioInventory = KeyCode.Tab;
    private bool pressedInvenKey;
    [Space(20f)]

    [Header("Interact Setting")]
    [SerializeField] private KeyCode InteractKey = KeyCode.E;
    private bool pressedInteractKey;

    [Header("Swap Setting")]
    [SerializeField] private KeyCode FirstSlot = KeyCode.A;
    private bool pressedFirstSlot;
    [SerializeField] private KeyCode SecondSlot = KeyCode.S;
    private bool pressedSecondSlot;
    [SerializeField] private KeyCode ThirdSlot = KeyCode.D;
    private bool pressedThirdSlot;    
    [Tooltip("무기 중복체크")]
    public bool checkSlot = false;


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

        chargingTime = 0;
<<<<<<< HEAD
        // animator = 
=======
>>>>>>> main
    }

    void Start()
    {

        SetEquipment();
        battle.WeaponType = PlayerWeaponType;
        //manager.InvenUI.SetActive(manager.isInven);
        playerHpBar.maxValue = status.maxHp;
    }



    void Update()
    {

        InputSystem();
        Act();
        Move();
        PlayerUISystem();
        Swap();
<<<<<<< HEAD

        // ChangeEquipment();
=======
>>>>>>> main
    }

    void PlayerUISystem()
    {
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

        if (manager.isSlotSwap && Input.GetKeyDown(KeyCode.Escape))
        {
            manager.SlotSwapUI.SetActive(false);
            manager.isAction = false;
            manager.isSelected = false;
            manager.isSlotSwap = false;
        }
        if (manager.isSlotSwap && Input.GetKeyDown(KeyCode.E)) // 수정 해야함
        {
            manager.SlotSwapUI.SetActive(false);
            manager.isAction = false;
            manager.isSelected = false;

            EleUISwap();
        }
      





<<<<<<< HEAD
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            manager.OpenInventory();
        }


        if (movement2D.isDashing || manager.isAction || manager.isShop || manager.isSlotSwap || manager.isInven|| battle.fallAtking || ischarging || battle.Atking)// || pressedRightAtkKey)//|| battle.Atking)
=======

        if (movement2D.isDashing || manager.isAction || manager.isShop || manager.isSlotSwap || battle.fallAtking || ischarging || battle.Atking)// || pressedRightAtkKey)//|| battle.Atking)
>>>>>>> main
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

        pressedAtkKey = Input.GetKey(atkKey);

        pressedFirstSlot = Input.GetKeyDown(FirstSlot);
        pressedSecondSlot = Input.GetKeyDown(SecondSlot);
        pressedThirdSlot = Input.GetKeyDown(ThirdSlot);
        pressedInvenKey = Input.GetKeyDown(ioInventory);

        if (battle.Atking)
        {
            pressedInteractKey = false;
        }

        
    }
    public void EleUISwap()
    {
        for (int i = 0; i < inventory.HasWeapon.Length; i++)
        {
            if (inventory.HavingElement[i] == inventory.HavingElement[manager.slot])
            {
                manager.Elements[i].SetActive(true);
            }
        }
        interact.ScanObj.gameObject.gameObject.SetActive(false);
        inventory.HavingElement[manager.slot] = ElementalManager.instance.AddElement((int)manager.ObjData.WeaponType);
        manager.Elements[manager.slot] = interact.ScanObj.gameObject;

        manager.isSlotSwap = false;
        manager.TalkPanel.SetActive(false);
    }
    public void ChangeAnim()
    {
        animator.runtimeAnimatorController = anim[(int)PlayerWeaponType];
    }
  
        

    public void SetEquipment()
    {
        ChangeAnim();
        status.SetStatue();
        battle.WeaponType = PlayerWeaponType;

        animator.SetFloat("AtkSpeed", status.atkSpeed * 0.01f);
    }

    void Act() //상호작용, 공격 스킬 등의 입력을 전달하는 함수
    {
        if (pressedInteractKey && !manager.isAction) interact.InteractObj();

        
<<<<<<< HEAD
        if (battle.fallAtking || manager.isAction || manager.isShop || manager.isSlotSwap || movement2D.isDashing || battle.Atking || manager.isInven)
=======
        if (battle.fallAtking || manager.isAction || manager.isShop || manager.isSlotSwap || movement2D.isDashing || battle.Atking)
        {
            if(isRepeatAtk)
                isRepeatAtk = false;
>>>>>>> main
            return;
        }
            
        

        // 행동 불가능한 상황
        if (pressedAtkKey && !isRepeatAtk)
        {  
            if(battle.WeaponType == WeaponTypes.Sword || battle.WeaponType == WeaponTypes.Wand || !movement2D.isGround)
            {
                isRepeatAtk = true;
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
            
            isRepeatAtk = true;
        }
        
        if(AtkFin)
        {
            isRepeatAtk = false;
        }

    }

    void Move()
    {
        if (battle.Atking)
        {
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
        if (pressedFirstSlot && battle.isSwap == true)
        {
            PlayerWeaponType = inventory.HavingElement[0].WeaponTypes;
            SetEquipment();
            battle.isSwap = false;
            battle.StartCoroutine(battle.ReturnSwap());
        }
        if (pressedSecondSlot && battle.isSwap == true)
        {
            PlayerWeaponType = inventory.HavingElement[1].WeaponTypes;
            SetEquipment();
            battle.isSwap = false;
            battle.StartCoroutine(battle.ReturnSwap());
        }
        
        if (pressedThirdSlot && battle.isSwap == true)
        { 
            PlayerWeaponType = inventory.HavingElement[2].WeaponTypes;
            SetEquipment();
            battle.isSwap = false;
            battle.StartCoroutine(battle.ReturnSwap());
        }
        
    }


    public void GetElement(int W)
    {
        
        for (int j = 0; j < inventory.HasWeapon.Length; j++)
        {
            if (inventory.HavingElement[j].ElementalID == W * 1000)
            {
                checkSlot = true;
                break;
            }
            else
            {
                checkSlot = false;
            }
        }
        for (int i = 0; i < inventory.HasWeapon.Length; i++)
        {
            if (checkSlot == true)
            {
                manager.TalkPanel.SetActive(false);
                Debug.Log("존재하는 정령입니다.");
                break;
            }
            else if (inventory.HavingElement[2].ElementalID != 0)
            {
                manager.OpenSwap();
                break;
            }
            else if (inventory.HavingElement[i].ElementalID == 0)
            {
                PlayerWeaponType = (WeaponTypes)W;
                //inventory.HavingWeapon[i] = (int)PlayerWeaponType;
                inventory.GetEle(ElementalManager.instance.AddElement(W * 1000));
                manager.Elements[i] = manager.ObjData.gameObject;
                // inventory.HavingElemental[i] = (int)PlayerElementType;
                manager.ObjData.gameObject.SetActive(false);
                SetEquipment();
                break;
            }
        }

    }
    
}
