using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public Transform playerTrans;
    public Transform atkTrans;
    public Vector2 atkSize;
    private Animator animator;
    private Rigidbody2D rigid2D;

    [Header("Snake Stats")]
    public float moveSpeed;
    public float maxHp;
    public float curHp;
    public float Dmg;
    public float atkCoolTime = 3f;
    public float curAtkCoolTime;
    public bool isKnockback = false;
    
    private int nextDir;
    

    

    void Awake()
    {
        curHp = maxHp;
        curAtkCoolTime = atkCoolTime;
        animator = GetComponent<Animator>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        rigid2D = GetComponent<Rigidbody2D>();
        RandomAct();
    }

    
    void Update()
    {
        curAtkCoolTime -= Time.deltaTime;
        SearchTarget();
        // Debug.Log(Vector2.Distance(playerTrans.position, transform.position));
    }

    public void SearchTarget()
    {
        if(Vector2.Distance(playerTrans.position, transform.position) <= 2.5f)
        {
            animator.SetBool("isRange", true);
            animator.SetBool("isMove", false);
            // rigid2D.velocity = new Vector2(rigid2D.velocity.x, rigid2D.velocity.y);
            rigid2D.velocity = Vector2.zero;
            //공격
            if(curAtkCoolTime <= 0)
            {
                //공격 시도
                animator.SetTrigger("Atk");
                curAtkCoolTime = atkCoolTime;
            }
        }
        else if(Vector2.Distance(playerTrans.position, transform.position) <= 8f)
        {
            //추적
            animator.SetBool("isRange", false);
            animator.SetBool("isMove", true);
            Follow();
        }
        else
        {
            //랜덤 이동
            RandomMove();
        }
    }

    
    void Follow()
    {
        Vector3 moveDir = Vector3.zero;
        


        if(playerTrans.position.x > transform.position.x)
        {
            nextDir = 1;
            // moveDir = Vector3.right;
            // rigid2D.velocity = new Vector2(1 * moveSpeed, rigid2D.velocity.y);
            // transform.localScale = new Vector3(-2, 2 ,1);

        }
        else
        {
            
            nextDir = -1;
            // moveDir = Vector3.left;
            // rigid2D.velocity = new Vector2(-1 * moveSpeed, rigid2D.velocity.y);
            // transform.localScale = new Vector3(2, 2 ,1);
        }

        Vector2 frontVec = new Vector2(rigid2D.position.x + nextDir * 2, rigid2D.position.y - 2);
        
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1 ,LayerMask.GetMask("Platform"));
        


        transform.localScale = new Vector3(nextDir * -4, 4 ,1);
        if(raycast.collider == null)
        {
            nextDir = 0;
        }
        rigid2D.velocity = new Vector2(nextDir * moveSpeed, rigid2D.velocity.y);
    }

    void RandomMove()
    {
        animator.SetBool("isRange", false);
        animator.SetBool("isMove", nextDir != 0);
        
        rigid2D.velocity = new Vector2(nextDir * moveSpeed, rigid2D.velocity.y);


        if(nextDir != 0)    
            transform.localScale = new Vector3(nextDir * -4, 4 ,1);

        Vector2 frontVec = new Vector2(rigid2D.position.x + nextDir * 2, rigid2D.position.y - 2);
        
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1 ,LayerMask.GetMask("Platform"));
        
        if(raycast.collider == null)
        {
            nextDir = nextDir * -1;
            CancelInvoke();
            Invoke("RandomAct", 2f);
        }
    }

    public void RandomAct()
    {
        nextDir = Random.Range(-1, 1 + 1);
        Invoke("RandomAct", Random.Range(1f , 3f));
    }

    
    public void WolfAtk()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkTrans.position, atkSize,  0);
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Player")
                {
                    collider.GetComponent<Battle>().GetDamaged(Dmg);
                }
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(atkTrans.position, atkSize);    
        
    }


    public void GetDamaged(float dmg)
    {
        if(isKnockback) return;

        animator.ResetTrigger("Hurt");
        animator.SetTrigger("Hurt");
        float x = transform.position.x - playerTrans.position.x;
        if(x < 0)
            x = 1;
        else
            x = -1;

        StartCoroutine(Knockback(x));

        curHp -= dmg;
    }

    IEnumerator Knockback(float dir)
    {
        isKnockback = true;
        float ctime = 0;
        while (ctime < 0.2f)
        {
            if(transform.rotation.y == 0)
                transform.Translate(Vector3.left * 10f * Time.deltaTime * dir);
            else
                transform.Translate(Vector3.left * 10f * Time.deltaTime * -1f * dir);

            ctime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
    }
}
