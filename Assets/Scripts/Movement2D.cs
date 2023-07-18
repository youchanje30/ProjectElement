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
        // Time.timeScale = 0.1f;
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
        // gameObject.transform.position.y -0.47f), new Vector2(0.55f, 0.01f), 0f, GroundLayer);
        gameObject.transform.position.y -0.95f), new Vector2(0.55f, 0.05f), 0f, GroundLayer);

        // Debug.Log(isGround);

        
        /* Vector2 upVec = new Vector2(rigid2D.position.x, boxCollider2D.bounds.min.y);
        Debug.DrawRay(upVec, Vector3.up, new Color(1,0,0));
        RaycastHit2D raycast = Physics2D.Raycast(upVec, Vector3.up, 1f ,LayerMask.GetMask("Platform"));

        if(raycast.collider != null)
        {
            transform.position = new Vector2(transform.position.x ,transform.position.y + (raycast.transform.position.y - boxCollider2D.bounds.min.y));
        } */

        if(isGround == true && rigid2D.velocity.y <= 0)
        {
            curJumpCnt = maxJumpCnt;
            curDashCnt = maxJumpCnt;
            animator.SetBool("isGround", true);
            // animator.SetBool("isJump", false);
            // animator.SetBool("isAct", false);
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
        
        animator.SetBool("isGround", isGround);

        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawCube(footPos, new Vector2(1f, 0.1f));
        // Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.47f), new Vector2(0.55f, 0.01f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.95f), new Vector2(0.55f, 0.05f));
    
    }


    public void MoveX(float hAxis)
    {
        rigid2D.velocity = new Vector2(hAxis * moveSpeed, rigid2D.velocity.y);

        if(hAxis != 0)
        {
            // transform.localScale = new Vector3( -(hAxis), 1, 1);
            transform.localScale = new Vector3( -(hAxis)*2, 2, 1);
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
            animator.SetBool("isAct", true);
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
            // Vector2 forceDir = new Vector2(0, 1);
            // rigid2D.AddForce(forceDir * jumpForce, ForceMode2D.Impulse);
            // 하강할 때 점프가 제대로 되지 않고, 힘을 주는 방식이라 조금 부정확한듯
            curJumpCnt --;
        }
    }


    public IEnumerator Dash()
    {
        animator.SetBool("isAct", true);
        animator.SetTrigger("Dash");
        isDashing = true; 
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(-(transform.localScale.x) * dashingPower, 0f);
        // rigid2D.velocity = new Vector2(-(transform.localScale.x) * 0f, 0f);
        
        yield return new WaitForSeconds(dashingTime);
        // yield return new WaitForSeconds(2f);
        
        animator.SetBool("isAct", false);
        // animator.SetTrigger("Das");
        rigid2D.gravityScale = originalGravity;
        isDashing = false;
        curDashCnt --;
    }

}
