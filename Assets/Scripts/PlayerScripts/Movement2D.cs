using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Movement2D : MonoBehaviour
{
    public float moveSpeed
    {
        get
        {
            return playerStatus.playerSpeed;
        }
    }
    public float jumpForce
    {
        get 
        {
            return playerStatus.jumpForce;
        }
    }
    
    private Rigidbody2D rigid2D;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private GameObject currentOneWayPlatform;
    private PlayerStatus playerStatus;
    private Battle battle;
    // [SerializeField] private BoxCollider2D

    [SerializeField] private LayerMask GroundLayer;
    public bool isGround;
    
    [Header("Jump Setting")]
    [Tooltip("최대 점프 횟수")]
    [SerializeField] private int maxJumpCnt = 2;
    [SerializeField] private int curJumpCnt;
    [Space(20f)]



    [Header("Dash Setting")]
    [SerializeField] private float DashCoolTime;
    public bool isDashing;
    [Tooltip("대쉬 힘(높을 수록 멀리 감)")]
    [SerializeField] private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    [Tooltip("최대 대쉬 횟수")]
    [SerializeField] private int maxDashCnt;
    public int curDashCnt;
    

    [Header("Ground Check")]
    [SerializeField] float floorY;
    [SerializeField] Vector2 floorSize;
    public bool isNearFloor;
    [SerializeField] float nearFloorY;
    [SerializeField] float nearFloorX;
    [SerializeField] float nearFloorRayLength;
    


    void Awake() 
    {
        // Time.timeScale = 0.1f;
        curDashCnt = maxDashCnt;
        curJumpCnt = maxJumpCnt;

        battle = GetComponent<Battle>();
        rigid2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
    }


    void Update()
    {
        if(isDashing)
            rigid2D.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        if(isGround && rigid2D.velocity.y <= 0.1f)
        {
            curJumpCnt = maxJumpCnt;
            animator.SetBool("isGround", true);
            // animator.SetBool("isAct", false);
        }

        if(isGround && rigid2D.velocity.y <= 0 && Input.GetKey(KeyCode.DownArrow) && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.C)))
        {
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
        
        if(isGround && rigid2D.velocity.y <= 0.1f)
        {
            curJumpCnt = maxJumpCnt;
            animator.SetBool("isGround", true);
        }
        else
            animator.SetBool("isGround", false);
        
    }

    void FixedUpdate()
    {
        CheckGround();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - floorY), floorSize);
    }


    public void MoveX(float hAxis)
    {
        rigid2D.velocity = new Vector2(hAxis * moveSpeed, rigid2D.velocity.y);

        if(hAxis != 0)
        {
            transform.localScale = new Vector3(hAxis * 1.8f, 1.8f, 1);
            animator.SetBool("isMove", true);
        }  
        else
        {
            animator.SetBool("isMove", false);
        }
            

    }

    public void Jump()
    {
        if(curJumpCnt > 0)
        {
            animator.SetBool("isGround", false);
            animator.SetTrigger("Jump");
            // animator.SetBool("isAct", true);
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
            // Vector2 forceDir = new Vector2(0, 1);
            // rigid2D.AddForce(forceDir * jumpForce, ForceMode2D.Impulse);
            // 하강할 때 점프가 제대로 되지 않고, 힘을 주는 방식이라 조금 부정확한듯
            curJumpCnt --;

        }
    }

    public bool DownJump()
    {
        if(isGround && rigid2D.velocity.y <= 0 && Input.GetKey(KeyCode.DownArrow) && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.C)))
        {
            Debug.Log("DownJump!");
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
                return true;
            }
        }
        return false;
    }

    public IEnumerator Dash()
    {
        // animator.SetBool("isAct", true);
        animator.SetTrigger("Dash");
        isDashing = true; 
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        battle.isGuard = true;
    
        yield return new WaitForSeconds(dashingTime);
    
        animator.SetTrigger("DashEnd");
        rigid2D.gravityScale = originalGravity;
        isDashing = false;
        curDashCnt --;
        battle.isGuard = false;
        StartCoroutine(DashCoolDown(DashCoolTime));
    }

    public IEnumerator DashCoolDown(float dashCoolTime)
    {
        yield return new WaitForSeconds(dashCoolTime);
        curDashCnt = maxDashCnt;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("OneWayPlatForm"))
        {
            currentOneWayPlatform = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("OneWayPlatForm"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        CompositeCollider2D platformCollider = currentOneWayPlatform.GetComponent<CompositeCollider2D>();

        Physics2D.IgnoreCollision(boxCollider2D, platformCollider);
        yield return new WaitForSeconds(0.45f);
        Physics2D.IgnoreCollision(boxCollider2D, platformCollider, false);
        
    }

    private void CheckGround()
    {
        isGround = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, 
        gameObject.transform.position.y - floorY), floorSize, 0f, GroundLayer);

        isNearFloor = false;
        for (int i = 0; i < 3; i++)
        {
            Vector2 nearVec = new Vector2(transform.position.x + (1 - i) * nearFloorX, transform.position.y - nearFloorY);
            Debug.DrawRay(nearVec, new Vector3(0, -nearFloorRayLength, 0), new Color(0,1,0));
            // RaycastHit2D raycast = Physics2D.Raycast(nearVec, Vector3.down, nearFloorRayLength, LayerMask.GetMask("Platform"));
            RaycastHit2D raycast = Physics2D.Raycast(nearVec, Vector3.down, nearFloorRayLength, GroundLayer);

            if(raycast.collider != null) 
            {
                isNearFloor = true;
                break;   
            }
        }
    }
}
