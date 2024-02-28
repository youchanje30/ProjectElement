using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatTest : MonoBehaviour
{
    
    [SerializeField] Transform target;
    [SerializeField] float jumpHeight;
    [SerializeField] Rigidbody2D rigid;

    [SerializeField] float floorCheck;

    [SerializeField] Vector2 maxHeightDisplacement;
    [SerializeField] int pointCount;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // if(GroundCheck() && rb.velocity.y <= 0.1f)
        //     rb.velocity = Vector2.zero;
    }

    [ContextMenu("Test")]
    void StartJump()
    {
        animator.SetTrigger("JumpAtk");
        Vector2 vec = new Vector2((target.transform.position.x - transform.position.x) / 2f, jumpHeight);
        JumpForce(vec);
    }

    private void JumpForce(Vector2 maxHeightDisplacement)
    {
        float v_y = Mathf.Sqrt(2 * rigid.gravityScale * -Physics2D.gravity.y * maxHeightDisplacement.y);
        float v_x = maxHeightDisplacement.x * v_y / (2 * maxHeightDisplacement.y);

        Vector2 force = rigid.mass * (new Vector2(v_x, v_y) - rigid.velocity);
        rigid.AddForce(force, ForceMode2D.Impulse);
    }

    
}
