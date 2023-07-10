using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //PlayerController에서 플레이어의 입력을 받음
    //Movement2D 캐릭터의 이동에 관한 스크립트
    //Battle 캐릭터의 전투에 관한 스크립트
    //
    
    //GameManager는 씬의 초기 세팅, 설정 등에 관한 스크립트
    [SerializeField] private Movement2D movement2D;

    [Header("Input Setting")]
    private float hAxis;
    private KeyCode JumpKey = KeyCode.Space;
    private bool pressedJumpkey;
    private KeyCode DashKey = KeyCode.LeftShift;
    private bool pressedDashKey;
    [Space(20f)]

    Rigidbody2D rigid2D;



    void Start()
    {
        
    }

    void Update()
    {
        InputSystem();
        Move();

    }

    void InputSystem()
    {
        if(movement2D.isDashing)
        {
            pressedDashKey = false;
            pressedJumpkey = false;
            hAxis = 0;
            return;
        } 

        pressedDashKey = Input.GetKeyDown(DashKey);
        pressedJumpkey = Input.GetKeyDown(JumpKey);
        hAxis = Input.GetAxisRaw("Horizontal");
        

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
