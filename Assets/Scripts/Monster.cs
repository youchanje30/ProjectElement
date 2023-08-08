using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// public enum 
public enum MonsterTypes
{
    Snake,
    Wolf,
    Rabbit

}

public enum MonsterGrades
{
    Common,
    Elite,
    Boss
}



public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterTypes MonsterType;
    [SerializeField] private StageManager stageManager;



    [Header("Image Information")]
    public float ImgScale;
    public float FloorRayX;
    public float FloorRayY;
    [SerializeField] private SpriteRenderer sprite;
    [Space(20f)]

    private Transform playerTrans;
    public Transform atkTrans;
    public Vector2 atkSize;
    private Animator animator;
    private Rigidbody2D rigid2D;



    [Header("Enemy Status")]
    public float moveSpeed;
    public float maxHp;
    public float Dmg;
    public float atkCoolTime = 3f;
    public float AtkRange;
    public float FollowRange;
    [Space(20f)]





    [Header("Current Enemy Status")]
    public float curHp;
    [SerializeField] private float curAtkCoolTime;
    [SerializeField] private bool isKnockback = false;
    [SerializeField] private bool isDead = false;
    [Space(20f)]

    
    [Header("Enemy UI")]
    [SerializeField] private Slider Hpbar;
    public Gradient gradient;
    [SerializeField] Image HpFill;

    private int nextDir;
    


    

    void Awake()
    {
        #region Enemy Setting

        curHp = maxHp;
        Hpbar.maxValue = maxHp;
        Hpbar.value = curHp;
        curAtkCoolTime = atkCoolTime;
        SetHealth();

        #endregion 


        


        #region Component Setting

        sprite = GetComponent<SpriteRenderer>();
        stageManager = StageManager.FindAnyObjectByType<StageManager>();
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        RandomAct();

        #endregion
    }

    
    void Update()
    {
        if(!isDead)
        {
            curAtkCoolTime -= Time.deltaTime;
            if(MonsterType != MonsterTypes.Rabbit)
            {
                SearchTarget();
            }
            else
            {
                // Rabbit();
            }
            // Debug.Log(Vector2.Distance(playerTrans.position, transform.position));
        }
    }

    public void Rabbit()
    {

    }


    

    void SetHealth()
    {
        Hpbar.value = curHp;
        HpFill.color = gradient.Evaluate(Hpbar.normalizedValue);
    }

    public void SearchTarget()
    {
        if(Vector2.Distance(playerTrans.position, transform.position) <= AtkRange)
        {
            animator.SetBool("isRange", true);
            animator.SetBool("isMove", false);
            rigid2D.velocity = Vector2.zero; // 정지
            // rigid2D.velocity = new Vector2(rigid2D.velocity.x, rigid2D.velocity.y);
            
            //공격
            if(curAtkCoolTime <= 0)
            {
                //공격 시도
                animator.SetTrigger("Atk");
                curAtkCoolTime = atkCoolTime;
            }
            //아니면 애니메이션은 Idle 상태로 존재한다.
        }
        else if(Vector2.Distance(playerTrans.position, transform.position) <= FollowRange)
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

        // nextDir = 1;
        /* if(Mathf.Abs(playerTrans.position.x - transform.position.x) < 1f)
        {

        }
        else
        {
            
        } */

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



        Vector2 frontVec = new Vector2(rigid2D.position.x + nextDir * FloorRayX, rigid2D.position.y - FloorRayY);
        
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1 ,LayerMask.GetMask("Platform"));
        
        if(Mathf.Abs(playerTrans.position.x - transform.position.x) < 0.15f)
        {
            transform.localScale = transform.localScale;
        }
        else
        {
            transform.localScale = new Vector3(nextDir * - (ImgScale), ImgScale ,1);
        }



        if(raycast.collider == null)
            nextDir = 0;
        
        rigid2D.velocity = new Vector2(nextDir * moveSpeed, rigid2D.velocity.y);
    }


    void RandomMove()
    {
        animator.SetBool("isRange", false);
        animator.SetBool("isMove", nextDir != 0);
        
        rigid2D.velocity = new Vector2(nextDir * moveSpeed, rigid2D.velocity.y);

        if(nextDir != 0)    
            transform.localScale = new Vector3(nextDir * - (ImgScale), ImgScale ,1);

        Vector2 frontVec = new Vector2(rigid2D.position.x + nextDir * FloorRayX, rigid2D.position.y - FloorRayY);
        
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
        if(isKnockback || isDead) return;
        
        curHp -= dmg;
        SetHealth();

        if(curHp <= 0)
        {
            Hpbar.gameObject.SetActive(false);
            isDead = true;
            animator.SetTrigger("Dead");
            return;
        }


        animator.ResetTrigger("Hurt");
        animator.SetTrigger("Hurt");
        float x = transform.position.x - playerTrans.position.x;

        if(x < 0)
            x = 1;
        else
            x = -1;

        StartCoroutine(Knockback(x));

        

        
        
    }

    public void SnakeAtk()
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


    IEnumerator Knockback(float dir)
    {
        sprite.color = new Color(0, 0, 0);
        isKnockback = true;
        float ctime = 0;
        while (ctime < 0.2f)
        {
            if(transform.rotation.y == 0)
            {
                transform.Translate(Vector3.left * 10f * Time.deltaTime * dir);
                // transform.localScale = new Vector3(-ImgScale, ImgScale ,1);
            }
            else
            {
                transform.Translate(Vector3.left * 10f * Time.deltaTime * -1f* dir);
                // transform.localScale = new Vector3(ImgScale, ImgScale ,1);
            }

            ctime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
        sprite.color = new Color(1, 1, 1);
    }

    public void Dead()
    {
        stageManager.DeadMonster();
        Destroy(gameObject);
    }
    

}