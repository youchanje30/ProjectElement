using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Movement2D : MonoBehaviour
{
    [Header("Base Setting")]
    public float moveSpeed = 4f;
    public float jumpForce = 15f;
    [Space(20f)]
    
    private Rigidbody2D rigid2D;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    [SerializeField] private LayerMask GroundLayer;
    public bool isGround;
    private Vector2 footPos;
    


    [Header("Jump Setting")]
    [SerializeField] private int maxJumpCnt = 2;
    [SerializeField] private int curJumpCnt;
    [Space(20f)]



    [Header("Dash Setting")]
    public bool isDashing;
    [SerializeField] private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    [SerializeField] private int maxDashCnt = 2;
    public int curDashCnt;
    





    void Awake() 
    {
        curDashCnt = maxDashCnt;
        curJumpCnt = maxJumpCnt;

        rigid2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    { 
        // Bounds bounds = boxCollider2D.bounds;
        // footPos = new Vector2(bounds.center.x, bounds.min.y);

        //footPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);
        //isground = Physics2D.OverlapBox(footPos, new Vector2(1f, 0.1f), Layer);
        isGround = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, 
        gameObject.transform.position.y -0.47f), new Vector2(0.55f, 0.01f), 0f, GroundLayer);

        if(isGround == true && rigid2D.velocity.y <= 0)
        {
            curJumpCnt = maxJumpCnt;
            curDashCnt = maxJumpCnt;
            animator.SetBool("isGround", true);
            animator.SetBool("isJump", false);
            // Debug.Log("Reset Cnt");
        }

        if( !isDashing && rigid2D.velocity.y > 0)
        {
            // rigid2D.gravityScale = 
        }
        else if(!isDashing)
        {
            // rigid2D.gravityScale = 
        }

        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawCube(footPos, new Vector2(1f, 0.1f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.47f), new Vector2(0.55f, 0.01f));
    
    }


    public void MoveX(float hAxis)
    {
        rigid2D.velocity = new Vector2(hAxis * moveSpeed, rigid2D.velocity.y);

        if(hAxis != 0)
        {
            transform.localScale = new Vector3( -(hAxis), 1, 1);
            animator.SetBool("isWalk", true);
        }  
        else
        {
            animator.SetBool("isWalk", false);
        }
            

    }

    public void Jump()
    {
        if(curJumpCnt > 0)
        {
            animator.SetBool("isGround", false);
            animator.SetBool("isJump", true);
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
            curJumpCnt --;
        }
    }


    public IEnumerator Dash()
    {
        animator.SetTrigger("DashStart");
        isDashing = true; 
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(-(transform.localScale.x) * dashingPower, 0f);
        
        yield return new WaitForSeconds(dashingTime);
        
        animator.SetTrigger("DashEnd");
        rigid2D.gravityScale = originalGravity;
        isDashing = false;
        curDashCnt --;
    }

}
