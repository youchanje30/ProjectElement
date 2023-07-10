using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [Header("Base Setting")]
    public float moveSpeed = 10f;
    public float jumpForce = 15f;

    
    private Rigidbody2D rigid2D;
    private BoxCollider2D boxCollider2D;

    [SerializeField] private float lowGravity = 4f;
    [SerializeField] private float highGravity = 8f;

    [SerializeField] private LayerMask Layer;
    public bool isground;
    private Vector3 footPos;
    


    [Header("Jump Setting")]
    [SerializeField] private int maxJumpCnt = 2;
    private int curJumpCnt;
    [Space(20f)]



    [Header("Dash Setting")]
    public bool isDashing;
    [SerializeField] private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    [SerializeField] private int maxDashCnt = 2;
    private int curDashCnt;
    





    void Awake() 
    {
        curDashCnt = maxDashCnt;
        curJumpCnt = maxJumpCnt;

        rigid2D = GetComponent<Rigidbody2D>();
    }


    public void MoveX(float hAxis)
    {
        rigid2D.velocity = new Vector2(hAxis * moveSpeed, rigid2D.velocity.y);
    }

    public void Jump()
    {

        if(curJumpCnt > 0)
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
            curJumpCnt --;
        }
    }

    public IEnumerator Dash()
    {
        isDashing = true; 
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(-(transform.localScale.x) * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);
        rigid2D.gravityScale = 1f;
        isDashing = false;
        curDashCnt --;

    }

}
