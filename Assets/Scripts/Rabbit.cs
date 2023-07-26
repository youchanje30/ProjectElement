using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    
    [Header("Image Information")]
    public float ImgScale;
    public float FloorRayX;
    public float FloorRayY;
    [Space(20f)]
    

    [Header("Rabbit Status")]
    public float jumpX;
    public float jumpY;
    public float jumpForce;
    public bool canAtk;
    public bool isGround;
    public int MinCoolTime;
    public int MaxCoolTime;
    public Vector2 GroundSize;
    [Space(20f)]

    private Animator animator;
    private Rigidbody2D rigid2D;
    private Monster monster;
    [SerializeField] private LayerMask GroundLayer;

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - FloorRayY), GroundSize);
    }

    void Awake()
    {
        monster = GetComponent<Monster>();
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        RabbitAtk();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 frontVec = new Vector2(rigid2D.position.x, rigid2D.position.y - FloorRayY);

        // Debug.DrawRay(frontVec, Vector3.down * 0.3f, new Color(0,0,1));
        // RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 0.3f , GroundLayer);

        Collider2D Ground = Physics2D.OverlapBox(frontVec, GroundSize, 0f, GroundLayer);

        if(Ground != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
            animator.SetBool("isGround", false);
        }

        if(rigid2D.velocity.y < 0 && Ground != null)
        {
            animator.SetBool("isGround", true);
            canAtk = false;
        }
        
    }

    public void RabbitAtk()
    {
        if(!isGround)
        {
            Invoke("RabbitAtk", Random.Range(MinCoolTime, MaxCoolTime + 1));
            return;
        }

        
        animator.SetBool("isGround", false);
        animator.SetTrigger("Atk");
        Vector2 JumpVec = new Vector2(jumpX , jumpY).normalized;
        
        int vec = Random.Range(1, 2 + 1);
        if(vec == 1)
        {
            JumpVec = new Vector2(-(jumpX) , jumpY).normalized;
            transform.localScale = new Vector3(ImgScale, ImgScale ,1);
        }
        else
        {
            transform.localScale = new Vector3(-ImgScale, ImgScale ,1);
        }

        rigid2D.AddForce(JumpVec * jumpForce, ForceMode2D.Impulse);

        Invoke("RabbitAtk", Random.Range(MinCoolTime, MaxCoolTime + 1));
        canAtk = true;
    }   

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && !isGround && canAtk)
        {
            canAtk = false;
            other.GetComponent<Battle>().GetDamaged(monster.Dmg);
        }
    }

    

}
