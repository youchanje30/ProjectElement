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
    [Space(20f)]

    private Animator animator;
    private Rigidbody2D rigid2D;
    private Monster monster;
    [SerializeField] private LayerMask GroundLayer;

    
    void Awake()
    {
        monster = GetComponent<Monster>();
        rigid2D = GetComponent<Rigidbody2D>();
        RabbitAtk();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 frontVec = new Vector2(rigid2D.position.x, rigid2D.position.y - FloorRayY);

        Debug.DrawRay(frontVec, Vector3.down * 0.3f, new Color(0,0,1));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 0.3f , GroundLayer);

        if(raycast.collider != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        if(rigid2D.velocity.y < 0 && raycast.collider != null)
            canAtk = false;
        
    }

    public void RabbitAtk()
    {
        if(!isGround)
        {
            Invoke("RabbitAtk", Random.Range(MinCoolTime, MaxCoolTime + 1));
            return;
        }


        Vector2 JumpVec = new Vector2(jumpX , jumpY).normalized;
        
        int vec = Random.Range(1, 2 + 1);
        if(vec == 1)
        {
            JumpVec = new Vector2(-(jumpX) , jumpY).normalized;
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
