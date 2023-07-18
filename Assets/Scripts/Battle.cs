using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum weaponTypes { shield, sword ,bow };
public class Battle : MonoBehaviour
{
    [Header("Player Status")]
    public float maxHp = 10f;
    public float curHp;
    public float def;
    public float meleeDmg;
    public float skillDmg;
    public float atkSpeed;
    public float crtRate;
    public float crtDmg;
    [Space(20f)]

    public RuntimeAnimatorController[] AnimController;
    public Animator animator;
    
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    public bool fallAtking;

    [Header("Weapon Setting")]
    public int weaponType;
    public bool Atking;
    public bool isGuard;
    public float[] Left_BeforAtkDelay;
    public float[] Left_AfterAtkDelay;
    public float[] Right_BeforAtkDelay;
    public float[] Right_AfterAtkDelay;
    public float[] Left_AtkCoolTime;
    public float[] Right_AtkCoolTime;
    public bool[] isAtkReady;
    public Transform[] atkPos;
    public Vector2[] atkSize;
    // public Transform[] atkPos;
    public GameObject arrow;

    public float originalScale;


    public Transform fallDownAtkPos;
    public Vector2 fallDownAtkSize;



    void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        fallAtking = false;
        originalScale = rigid2D.gravityScale;
        // weaponType = 0;    
        for (int i = 0; i < isAtkReady.Length; i++)
        {
            isAtkReady[i] = true;
        }
        
        animator = GetComponent<Animator>();
    }



    void Update()
    {
        Vector2 underVec = new Vector2(rigid2D.transform.position.x, rigid2D.transform.position.y - 0.9f);

        Debug.DrawRay(underVec, new Vector3(0, -0.2f, 0), new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(underVec, Vector3.down, 0.2f ,LayerMask.GetMask("Platform"));

        // if(fallAtking && movement2D.isGround) //착지 공격 참 + 땅에 도착 참
        if(fallAtking && raycast.collider != null)
        {
            animator.SetBool("isGround", true);
            animator.SetBool("isAct", false);
            fallAtking = false;
            Atking = false;
            rigid2D.gravityScale = originalScale;
            // rigid2D.velocity = Vector2.zero;
            
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(fallDownAtkPos.position, fallDownAtkSize, 0);
            foreach(Collider2D collider in collider2Ds)
            {
                
                if(collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    Atk(collider.gameObject);
                    
                    // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                }
            }
            
        }
    }


    public void Atk(GameObject AtkObj)
    {
        if(AtkObj.tag == "Monster")
        {
            AtkObj.GetComponent<Monster>().GetDamaged(meleeDmg);
            // 데미지 계산 식
        }

        if(AtkObj.tag == "Destruct")
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
        }

    }

    public void AtkAction(int id)
    {
        // if(!atk) return;

        if(id == 0) //좌클릭
        {
            if(weaponType == 2) return; //활은 좌클릭이 없어요
           
            if(!movement2D.isGround)
            {
                StartCoroutine(FallDownAtk());
                return;
            }
            StartCoroutine(LeftAtk());
        }
        else if (id == 1)
        {
            if(weaponType == 1) return; //칼은 우클릭이 없어요
            // StartCoroutine(RightAtk());
            if(weaponType == 2) StartCoroutine(BowAtk());
            if(weaponType == 0) StartCoroutine(ShieldGuard());
        }
        // Atking = true;
    }

    public IEnumerator FallDownAtk()
    {
        if(!Atking)
        {
            // 체공 시간
            // originalScale = rigid2D.gravityScale;
            Atking = true;
            fallAtking = true;
            rigid2D.gravityScale = 0f;
            rigid2D.velocity = Vector2.zero;

            animator.ResetTrigger("JumpAtk");
            animator.SetTrigger("JumpAtk");
            
            yield return new WaitForSeconds(0.2f);
            if(!movement2D.isGround)
                rigid2D.gravityScale = 20f;
        }
    }

    public IEnumerator LeftAtk()
    {
        if(isAtkReady[weaponType])
        {
            Atking = true;
            isAtkReady[weaponType] = false;
            animator.SetTrigger("Atk");
            animator.SetBool("isAct", true);

            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Left_BeforAtkDelay[weaponType]);

            // 공격 피격
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos[weaponType].position, atkSize[weaponType], 0);
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    Atk(collider.gameObject);
                    // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                }
            }
            // 공격 중인거 종료
            Atking = false;
            animator.SetBool("isAct", false);

            yield return new WaitForSeconds(Left_AtkCoolTime[weaponType]);
            //애니메이션 종료 및 공격 종료
            isAtkReady[weaponType] = true;
        }
    }
    public IEnumerator BowAtk()
    {
        if(isAtkReady[weaponType])
        {
            Atking = true;
            isAtkReady[weaponType] = false;
            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

            //화살 소환
            GameObject ThrowArrow = Instantiate(arrow);
            ThrowArrow.transform.position = atkPos[weaponType].position; 
            ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
            Atking = false;

            yield return new WaitForSeconds(Right_AtkCoolTime[weaponType]);

            isAtkReady[weaponType] = true;
        } 
    }

    public IEnumerator ShieldGuard()
    {
        Atking = true;
        // 차징 애니메이션 시작

        yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);
        
        isGuard = true;
        Atking = false;
    }
    

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(atkPos[weaponType].position, atkSize[weaponType]);    
        Gizmos.DrawWireCube(fallDownAtkPos.position, fallDownAtkSize);    
    }


   



    public void GetDamaged(float Damage)
    {
        Debug.Log("Get Damaged");

        if(isGuard) return;
        StopCoroutine(ShieldGuard());
    }
}
