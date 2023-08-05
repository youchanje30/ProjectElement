using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Elements
{
    None, Fire, South, Water, Wind
}

public enum WeaponTypes { Shield, Sword , Bow };

public class PlayerController : MonoBehaviour
{
    

    
    //PlayerController에서 플레이어의 입력을 받음
    //Movement2D 캐릭터의 이동에 관한 스크립트
    //Battle 캐릭터의 전투에 관한 스크립트
    //
    [Header("Player UI")]
    public Slider playerHpBar;
    [SerializeField] Image HpFill;
    public Gradient gradient;
    [Space(20f)]

    //GameManager는 씬의 초기 세팅, 설정 등에 관한 스크립트
    private Movement2D movement2D;
    private Interact interact;
    private Rigidbody2D rigid2D;
    private Battle battle;
    [SerializeField] private GameManager manager;
    private Animator animator;
    public RuntimeAnimatorController[] AnimController;



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
    [Space(20f)]

    [Header("Interact Setting")]
    [SerializeField] private KeyCode InteractKey = KeyCode.E;
    private bool pressedInteractKey;
    
    [Header("Atk Setting")]
    [SerializeField] private KeyCode LeftAtkKey = KeyCode.Z;
    private bool pressedLeftAtkKey;
    [SerializeField] private KeyCode RightAtkKey = KeyCode.X;

    private bool pressedRightAtkKey;

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
        #endregion Component Access

        playerHpBar.maxValue = battle.maxHp;

        // animator.runtimeAnimatorController = AnimController[(int)battle.WeaponType];
        SetEquipment();
        battle.WeaponType = PlayerWeaponType;
        chargingTime = 0;
        
        // animator = 
    }
    



    void Update()
    {
        InputSystem();
        Act();
        Move();
        PlayerUISystem();
        // ChangeEquipment();
    }

    void PlayerUISystem()
    {
        // playerHpBar.value = battle.curHp;
        playerHpBar.value = Mathf.Lerp(playerHpBar.value, battle.curHp, Time.deltaTime * 5f);
        HpFill.color = gradient.Evaluate(playerHpBar.normalizedValue);
    }

    void InputSystem()
    {
        if(manager.isShop && Input.GetKeyDown(KeyCode.Escape))
        {
            manager.isShop = false;
            manager.ShopUI.SetActive(false);
        }


        if(battle.WeaponType != WeaponTypes.Sword && !ischarging && Input.GetKeyDown(RightAtkKey) && !battle.fallAtking && !manager.isAction && !manager.isShop && !movement2D.isDashing)
        {
            pressedRightAtkKey = true;
            animator.SetBool("isCharge", true);
            animator.SetTrigger("Charging");
            ischarging = true;
        }


        if(battle.WeaponType != WeaponTypes.Sword && ischarging)
        {
            if(Input.GetKey(RightAtkKey))
            {

                chargingTime += Time.deltaTime;
            }

            if(Input.GetKeyUp(RightAtkKey))
            {
                ischarging = false;
                pressedRightAtkKey = false;
                
                if(chargingTime < 1f)
                    chargingTime = 0f;

                animator.SetBool("isCharge", false);
            }
        }
        


        if(movement2D.isDashing || manager.isAction || manager.isShop || battle.fallAtking || ischarging || battle.Atking)// || pressedRightAtkKey)//|| battle.Atking)
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
        pressedLeftAtkKey = Input.GetKeyDown(LeftAtkKey);
        pressedRightAtkKey = Input.GetKey(RightAtkKey);

        
        
        

        if(battle.Atking)// || pressedRightAtkKey)
        {   
            pressedInteractKey = false;
            pressedLeftAtkKey = false;
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

    public void SetEquipment()
    {
        battle.ResetStat();
        // if(animator.runtimeAnimatorController == Anims[(int)PlayerElementType].ElementAnim[(int)PlayerWeaponType]) return;
        animator.runtimeAnimatorController = Anims[(int)PlayerElementType].ElementAnim[(int)PlayerWeaponType];
        battle.WeaponType = PlayerWeaponType;
        // SaveManager.instance.Save();

        for (int i = 0; i < inventory.HavingItem.Length; i++)
        {
            if(inventory.HavingItem[i] != null)
            {
                //체력 증가
                battle.maxHp += inventory.HavingItem[i].HpIncrease;
                battle.maxPerHp += inventory.HavingItem[i].HpPerIncrease;
                
                //방어력 증가
                battle.def += inventory.HavingItem[i].DefIncrease;
                battle.defPer += inventory.HavingItem[i].DefPerIncrease;

                //물리 데미지 증가
                battle.meleeDmg += inventory.HavingItem[i].MeleeDmgIncrease;
                battle.meleePerDmg += inventory.HavingItem[i].MeleeDmgPerIncrease;

                //스킬 데미지 증가
                battle.skillDmg += inventory.HavingItem[i].SkillDmgIncrease;
                battle.skillPerDmg += inventory.HavingItem[i].SkillDmgPerIncrease;

                //공격 속도, 크리 확률, 크리 데미지 증가
                battle.atkSpeed += inventory.HavingItem[i].AtkSpeedIncrease;
                battle.crtRate += inventory.HavingItem[i].CrtRateIncrease;
                battle.crtDmg += inventory.HavingItem[i].CrtDmgIncrease;
            }
        }

        animator.SetFloat("AtkSpeed", battle.atkSpeed * 0.01f);
        

    }

    void Act() //상호작용, 공격 스킬 등의 입력을 전달하는 함수
    {
        if(pressedInteractKey && !manager.isAction) interact.InteractObj();

        if(pressedLeftAtkKey && !manager.isAction ) battle.AtkAction(0);

        if(!pressedRightAtkKey && !manager.isAction && chargingTime >= 1f)
        {
            battle.AtkAction(1);
            chargingTime = 0f;
        } 
    }


    void Move()
    {
        if(battle.Atking)
        {
            // movement2D.MoveX(hAxis);
            return;
        } 

        if(pressedDashKey && movement2D.curDashCnt > 0) StartCoroutine(movement2D.Dash());

        if(!movement2D.isDashing)
        {
            if(pressedJumpkey) movement2D.Jump();
            movement2D.MoveX(hAxis); 
        }
    }
}
