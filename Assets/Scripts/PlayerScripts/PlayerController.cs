using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;


public enum WeaponTypes { None, Sword , Wand , Shield , Bow };


public class PlayerController : MonoBehaviour
{


    //PlayerController에서 플레이어의 입력을 받음
    //Movement2D 캐릭터의 이동에 관한 스크립트
    //Battle 캐릭터의 전투에 관한 스크립트
    //[Header("Player UI")]
    //public Slider playerHpBar;
    //[SerializeField] Image HpFill;
    //public Gradient gradient;
    //public Slider BarrierBar;
    //[SerializeField] Image BarrierFill;
    [Space(20f)]

    //GameManager는 씬의 초기 세팅, 설정 등에 관한 스크립트
    private Movement2D movement2D;
    private PlayerStatus status;
    private Interact interact;
    private Rigidbody2D rigid2D;
    private Battle battle;
    private ActiveSkill skill;
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
    [SerializeField] private KeyCode JumpKey = KeyCode.C;
    [SerializeField] private KeyCode Jumpkey = KeyCode.Space;
    private bool pressedJumpkey;
    private bool PressedJumpkey;
    [SerializeField] private KeyCode DashKey = KeyCode.Z;
    private bool pressedDashKey;
    [SerializeField] private KeyCode atkKey = KeyCode.X;
    [SerializeField] float turnToChargeTime = 0.1f;
    // private bool isAtkKey;

    private bool pressedAtkKey;
    [SerializeField] private KeyCode SkillKey = KeyCode.F;
    private bool pressedSkillKey;
    private bool isRepeatAtk = false;
    [SerializeField] private KeyCode ioInventory = KeyCode.Tab;
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
    private bool isBegincharging;

    void Awake()
    {
        #region Component Access
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        movement2D = GetComponent<Movement2D>();
        interact = GetComponent<Interact>();
        rigid2D = GetComponent<Rigidbody2D>();
        battle = GetComponent<Battle>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        status = GetComponent<PlayerStatus>();


        skill = GetComponent<ActiveSkill>();
        #endregion Component Access

        chargingTime = 0;
    }

    void Start()
    {
        
        SaveManager.instance.Load(); // 로드 시점에 따라 적용되는 지점이 달라서 여기서 일단 실행했어요
        PlayerWeaponType = inventory.HavingElement[0].WeaponTypes;
        SetEquipment();
        //playerHpBar.maxValue = status.maxHp;
    }



    void Update()
    {
        InputSystem();
        Act();
        Move();
        Swap();
    }

    void InputSystem()
    {
        if (manager.isShop && Input.GetKeyUp(KeyCode.Escape))
        {
           closeShop();
        }

        if (manager.isSpiritAwake && Input.GetKeyUp(KeyCode.Escape))
        {
            closeSpiritawake();
        }

        if (manager.isSlotSwap && Input.GetKeyUp(KeyCode.Escape))
        {
            closeslotSwap();
        }
        if(manager.isInven && Input.GetKeyUp(KeyCode.Escape))
        {
            closeinven();
            Time.timeScale = 1f;
        }
        if (manager.isSlotSwap && Input.GetKeyDown(KeyCode.E)) 
        {

            StartCoroutine(EleUISwap());
        }
        

        if (Input.GetKeyDown(ioInventory) && !manager.isShop && !manager.isSlotSwap && !manager.isAction)
        {
            if ( manager.isInven && Time.timeScale == 0)
            {
                Time.timeScale = 1f;
                Invoke("closeinven", 0.1f);               
            }
            else if( Time.timeScale != 0 && !manager.isInven)
            { 
             manager.inventoryUI.InventoryAnim();
            }
        }
       

        
        // if (movement2D.isDashing || manager.isAction || manager.isShop || manager.isSlotSwap || manager.isInven|| battle.fallAtking || ischarging || battle.atking || skill.isCharging)//|| battle.Atking)
        if (movement2D.isDashing || manager.isAction || manager.isShop || manager.isSlotSwap || manager.isInven
        || battle.fallAtking || ischarging || battle.atking || skill.isCharging )
        {
            pressedDashKey = false;
            pressedJumpkey = false;
            PressedJumpkey = false;
            hAxis = 0;
            return;
        }

  

        pressedDashKey = Input.GetKeyDown(DashKey);
        pressedJumpkey = Input.GetKeyDown(JumpKey);
        PressedJumpkey = Input.GetKeyDown(Jumpkey);
        hAxis = Input.GetAxisRaw("Horizontal");

        pressedInteractKey = Input.GetKeyDown(InteractKey);

        pressedAtkKey = Input.GetKey(atkKey);
        pressedSkillKey = Input.GetKeyDown(SkillKey);
        pressedFirstSlot = Input.GetKeyDown(FirstSlot);
        pressedSecondSlot = Input.GetKeyDown(SecondSlot);
        pressedThirdSlot = Input.GetKeyDown(ThirdSlot);


        if (battle.atking)
        {
            pressedInteractKey = false;
        }

        
    }
    public IEnumerator EleUISwap()
    {
        yield return new WaitForSeconds(0.1f);
        manager.isSlotSwap = false;
        manager.swapUI.SlotSwapUI.SetActive(false);
        manager.Elements[manager.swapUI.slot].SetActive(true);
        inventory.HavingElement[manager.swapUI.slot] = ElementalManager.instance.AddElement((int)manager.ObjData.WeaponType * 1000);
        PlayerWeaponType = inventory.HavingElement[manager.swapUI.slot].WeaponTypes;
        SetEquipment();
        manager.isAction = false;
        manager.isSelected = false;
    }
    public void ChangeAnim()
    {
        animator.runtimeAnimatorController = anim[(int)PlayerWeaponType];
    }

    public void SetEquipment()
    {
        battle.WeaponType = PlayerWeaponType;
        ChangeAnim();
        GameObject.FindGameObjectWithTag("Spirit").GetComponent<Spirit>().ChangeAnim();
        status.SetStatue();
        GetComponent<PassiveSystem>().Swaped();
        animator.SetFloat("AtkSpeed", status.atkSpeed * 0.01f);
        //playerHpBar.maxValue = status.maxHp;
    }

    void Act() //상호작용, 공격 스킬 등의 입력을 전달하는 함수
    {
        bool AtkFin = Input.GetKeyUp(atkKey);

        if (pressedInteractKey && !manager.isAction)
        { 
            interact.InteractObj();
            pressedInteractKey = false;
            return;
        }
        
        if (battle.fallAtking || manager.isAction || manager.isShop || manager.isSlotSwap || movement2D.isDashing || battle.atking || manager.isInven || skill.isSouth || skill.isWater) 
        {
            if(AtkFin)
                isRepeatAtk = false;
            return;
        }
            
        
        // 행동 불가능한 상황
        if (pressedAtkKey && !isRepeatAtk && battle.WeaponType != WeaponTypes.None)
        {  
            if(battle.WeaponType == WeaponTypes.Sword || battle.WeaponType == WeaponTypes.Wand || !movement2D.isGround)
            {
                isRepeatAtk = true;
                if(battle.WeaponType == WeaponTypes.Bow)
                {
                    if(chargingTime >= 0.05f)
                    {   
                        battle.AtkAction(0, true);
                        chargingTime = 0f;
                        isRepeatAtk = true;
                        isBegincharging = false;
                        ischarging = false;
                        animator.SetBool("isCharge", false);
                    }
                }
                else
                {
                    battle.AtkAction(0);
                    if(battle.WeaponType == WeaponTypes.Shield)
                    {
                        chargingTime = 0f;
                        isRepeatAtk = true;
                        isBegincharging = false;
                        ischarging = false;
                        animator.SetBool("isCharge", false);
                    }
                }
                return;
            }
            else
            {
                if(rigid2D.velocity.y >= 0.1f)
                {
                    pressedAtkKey = false;
                    return;
                }
                    

                chargingTime += Time.deltaTime;

                if(!ischarging)
                {
                    rigid2D.velocity = Vector2.zero;
                    ischarging = true;
                }

                if(!isBegincharging && (chargingTime >= turnToChargeTime || battle.WeaponType == WeaponTypes.Bow))
                {
                    isBegincharging = true;
                    animator.SetBool("isCharge", true);
                    animator.SetTrigger("Charging");
                    if(battle.WeaponType == WeaponTypes.Bow)
                    {
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.BowCharging);
                    }
                }
            }
        }
        

        if(ischarging && (AtkFin || chargingTime >= 1f))
        {
            isBegincharging = false;
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

        if(ischarging)
            pressedSkillKey = false;
        
        if (pressedSkillKey && skill.skillData[(int)battle.WeaponType].isSkillReady)
        {
            if(battle.WeaponType == WeaponTypes.Sword)
            {
                animator.SetTrigger("Skill");
                battle.isSwap = false;
                battle.atking = true;
                rigid2D.velocity = Vector3.zero;
            }
            else
            {
                skill.TriggerSkill(battle.WeaponType);
            }
        }
    }

    void Move()
    {
        if (battle.atking || ischarging) return;
        
        if (pressedDashKey && movement2D.curDashCnt > 0) StartCoroutine(movement2D.Dash());

        if (!movement2D.isDashing)
        {
            if (pressedJumpkey || PressedJumpkey)
            {
                if(!movement2D.DownJump())
                    movement2D.Jump();
            }
            
            movement2D.MoveX(hAxis);
        }
    }
    
    void Swap()
    {
        if(!battle.isSwap) return;
        if (ischarging || battle.fallAtking || manager.isAction || manager.isShop || manager.isSlotSwap || battle.atking || manager.isInven || skill.isSouth || skill.isWater) return;


        if(pressedFirstSlot || pressedSecondSlot || pressedThirdSlot)
        {
            battle.isSwap = false;
            battle.StartCoroutine(battle.ReturnSwap());
        }
        else
            return;

        if (pressedFirstSlot)
        {
            AudioManager.instance.StopSfx(AudioManager.Sfx.Walk);
            PlayerWeaponType = inventory.HavingElement[0].WeaponTypes;
            SetEquipment();
            return;
        }

        if (pressedSecondSlot)
        {
            AudioManager.instance.StopSfx(AudioManager.Sfx.Walk);
            PlayerWeaponType = inventory.HavingElement[1].WeaponTypes;
            SetEquipment();
            return;
        }
        
        if (pressedThirdSlot)
        {
            AudioManager.instance.StopSfx(AudioManager.Sfx.Walk);
            PlayerWeaponType = inventory.HavingElement[2].WeaponTypes;
            SetEquipment();
            return;
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
                inventory.GetEle(ElementalManager.instance.AddElement(W * 1000));
                SetEquipment(); 
                manager.isSelected = false;
                break;
            }
        }
        
    }
    public void closeShop()
    {
        manager.isAction = false;
        manager.isShop = false;
        manager.ShopUI.SetActive(false);
    }
    public void closeSpiritawake()
    {
        manager.isAction = false;
        manager.isSpiritAwake = false;
        manager.SpiritAwakeUI.SetActive(false);
    }
    public void closeslotSwap()
    {
        manager.isAction = false;
        manager.isSelected = false;
        manager.isSlotSwap = false;
        manager.swapUI.SlotSwapUI.SetActive(false);
    }
    public void closeinven()
    {
        manager.isAction = false;
        manager.isInven = false;
        manager.inventoryUI.InvenUI.SetActive(false);
        manager.inventoryUI.inven.SetActive(false);
    }
}
