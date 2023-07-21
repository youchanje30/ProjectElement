using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        movement2D = GetComponent<Movement2D>();
        interact = GetComponent<Interact>();
        rigid2D = GetComponent<Rigidbody2D>();
        battle = GetComponent<Battle>();
        animator = GetComponent<Animator>();

        playerHpBar.maxValue = battle.maxHp;
        animator.runtimeAnimatorController = AnimController[(int)battle.WeaponType];
        
        chargingTime = 0;
        
        // animator = 
    }
    

    void Start()
    {
        // Time.timeScale = 0.2f;
    }

    void Update()
    {
        InputSystem();
        Move();
        Act();
        PlayerUISystem();
    }

    void PlayerUISystem()
    {
        // playerHpBar.value = battle.curHp;
        playerHpBar.value = Mathf.Lerp(playerHpBar.value, battle.curHp, Time.deltaTime * 5f);
        HpFill.color = gradient.Evaluate(playerHpBar.normalizedValue);
    }

    void InputSystem()
    {
        /* if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W Down");
        }
        else if(Input.GetKey(KeyCode.W))
        {
            Debug.Log("W Pressing");
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("W Up");
        } */


        if(battle.WeaponType != WeaponTypes.Sword || !ischarging)
        {
            if(Input.GetKeyDown(RightAtkKey))
            {
                pressedRightAtkKey = true;
                animator.SetBool("isCharge", true);
                animator.SetTrigger("Charging");
            }

            if(Input.GetKey(RightAtkKey))
                chargingTime += Time.deltaTime;

            if(Input.GetKeyUp(RightAtkKey))
            {
                
                pressedRightAtkKey = false;
                
                if(chargingTime < 1f)
                    chargingTime = 0f;

                animator.SetBool("isCharge", false);
            }
        }



        if(movement2D.isDashing || manager.isAction || battle.fallAtking)// || pressedRightAtkKey)//|| battle.Atking)
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

        
        
        

        if(battle.Atking || manager.isAction || movement2D.isDashing || battle.fallAtking)// || pressedRightAtkKey)
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
        if(pressedDashKey && movement2D.curDashCnt > 0) StartCoroutine(movement2D.Dash());

        if(!movement2D.isDashing)
        {
            if(pressedJumpkey) movement2D.Jump();
            movement2D.MoveX(hAxis); 
        }
    }
}
