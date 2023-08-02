using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Battle : MonoBehaviour
{
    [Header("Player Status")]
    [Tooltip("현재 체력")]
    public float curHp;
    [Tooltip("최대 체력")]
    public float maxHp = 10f;
    [Tooltip("최대% 체력")]
    public float maxPerHp; // 100%
    [Tooltip("방어력")]
    public float def;
    [Tooltip("방어력%")]
    public float defPer; // 100%
    [Tooltip("물리 데미지")]
    public float meleeDmg; 
    [Tooltip("물리 데미지%")]
    public float meleePerDmg; 
    [Tooltip("스킬 데미지")]
    public float skillDmg; 
    [Tooltip("스킬 데미지%")]
    public float skillPerDmg; 
    [Tooltip("공격 속도")]
    public float atkSpeed; // 100%
    [Tooltip("크리티컬 확률")]
    public float crtRate; // 기본 크리 10%
    [Tooltip("크리티컬 데미지")]
    public float crtDmg; // 기본 값 150%
    [Space(20f)]

    
    public Animator animator;
    
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    public bool fallAtking;

    [Header("Weapon Setting")]
    [Tooltip("무기 타입")]
    public WeaponTypes WeaponType;
    public bool Atking;
    public bool isGuard;
    public float[] Left_AtkCoolTime;
    public float[] Right_AtkCoolTime;
    public bool[] isAtkReady;
    public Transform[] atkPos;
    public Vector2[] atkSize;
    // public Transform[] atkPos;
    public GameObject arrow;

    public float originalScale;


    [Header("Sword Setting")]
    [SerializeField] private bool CanComboAtk = false;
    [SerializeField] private float ComboTime;
    [SerializeField] private Vector2 ComboAtkSize;
    [SerializeField] private Transform ComboAtkPos;



    [Header("Shield Setting")]
    [SerializeField] private float ShieldAtkDashTime;
    [SerializeField] private float AtkDashingPower;
    [Space(5f)]
    [SerializeField] private float ShieldDashingTime;
    [SerializeField] private float DashingPower;
    [SerializeField] private Vector2 ShieldDashAtkSize;
    [SerializeField] private Transform ShieldDashPos;
    // public bool 
    [Space(20f)]

    public Transform fallDownAtkPos;
    public Vector2 fallDownAtkSize;



    void Awake()
    {
        curHp = maxHp;
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
        // Debug.Log(CanComboAtk);

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
        CameraController.instance.StartCoroutine(CameraController.instance.Shake());
        
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

    public void ResetStat()
    {
        maxHp = 10;
        maxPerHp = 100;
        def = 0;
        defPer = 100;
        meleeDmg = 3;
        meleePerDmg = 0;
        skillDmg = 0;
        skillPerDmg = 0;
        atkSpeed = 100;
        crtRate = 10;
        crtDmg = 150;
    }

    public void AtkAction(int id)
    {
        // if(!atk) return;

        if(id == 0) //좌클릭
        {
            if(!movement2D.isGround)
            {
                if(WeaponType == WeaponTypes.Bow) return; //활은 점프 공격이 없어요
                
                    
                
                
                StartCoroutine(FallDownAtk());
                return;
            }

            if(WeaponType == WeaponTypes.Bow)
            {
                StartCoroutine(BowAtk());
                // Debug.Log("Start Atk Bow"); 
                return; //활은 좌클릭이 없어요
            }
            StartCoroutine(LeftAtk());
            if(WeaponType == WeaponTypes.Shield) StartCoroutine(DashGuard());
        }
        else if (id == 1)
        {
            if(WeaponType == WeaponTypes.Sword) return; //칼은 우클릭이 없어요
            // StartCoroutine(RightAtk());
            if(WeaponType == WeaponTypes.Bow) ChargingBowAtk(); //StartCoroutine(BowAtk());
            if(WeaponType == WeaponTypes.Shield) StartCoroutine(ShieldGuard());
        }
        // Atking = true;
    }

    public IEnumerator DashGuard()
    {
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(-(transform.localScale.x) * AtkDashingPower, 0f);
        yield return new WaitForSeconds(ShieldAtkDashTime);
        // rigid2D.gravityScale = originalGravity;
        rigid2D.gravityScale = 4f;
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
                rigid2D.gravityScale = 10f;
        }
    }

    public IEnumerator LeftAtk()
    {
        if(isAtkReady[(int)WeaponType])
        {
            Atking = true;
            isAtkReady[(int)WeaponType] = false;
            animator.SetBool("isAct", true);

            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Left_BeforAtkDelay[weaponType]);

            //칼의 경우 콤보 여부에 따른 어택 변경
            if(WeaponType == WeaponTypes.Sword && CanComboAtk)
            {
                animator.SetTrigger("ComboAtk");
                //콤보 공격
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(ComboAtkPos.position, ComboAtkSize, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Monster" || collider.tag == "Destruct")
                    {
                        Atk(collider.gameObject);
                    }
                }
                CanComboAtk = false;
            }
            else
            {
                animator.SetTrigger("Atk");
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType], 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Monster" || collider.tag == "Destruct")
                    {
                        Atk(collider.gameObject);
                        // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                    }
                }

                if(WeaponType == WeaponTypes.Sword)
                {
                    StartCoroutine(ComboAtk());
                    CanComboAtk = true;
                }
            }
            // 공격 중인거 종료

            yield return new WaitForSeconds(Left_AtkCoolTime[(int)WeaponType]/(atkSpeed * 0.01f));

            //애니메이션 종료 및 공격 종료

            isAtkReady[(int)WeaponType] = true;
        }
    }

    public void AtkDetection()
    {

        if(WeaponType == WeaponTypes.Sword && CanComboAtk)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(ComboAtkPos.position, ComboAtkSize, 0);
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    Atk(collider.gameObject);
                }
            }
            CanComboAtk = false;
        }
        else
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType], 0);
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    Atk(collider.gameObject);
                    // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                }
            }

            if(WeaponType == WeaponTypes.Sword)
            {
                StartCoroutine(ComboAtk());
                CanComboAtk = true;
            }
        }

    }


    public void AtkEnd()
    {
        Atking = false;
        animator.SetBool("isAct", false);

        if(WeaponType == WeaponTypes.Bow)
        {
            GameObject ThrowArrow = Instantiate(arrow);
            ThrowArrow.GetComponent<ProjectileType>().Damage = meleeDmg;
            ThrowArrow.transform.position = atkPos[(int)WeaponType].position;
            ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
        }
    }


    public IEnumerator ComboAtk()
    {
        yield return new WaitForSeconds(ComboTime/(atkSpeed * 0.01f));
        CanComboAtk = false;
    }


    public IEnumerator BowAtk()
    {
        if(isAtkReady[(int)WeaponType])
        {
            animator.SetTrigger("Atk");
            Atking = true;
            isAtkReady[(int)WeaponType] = false;
            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

            //화살 소환
            
            // Atking = false;

            yield return new WaitForSeconds(Left_AtkCoolTime[(int)WeaponType]/(atkSpeed * 0.01f));

            isAtkReady[(int)WeaponType] = true;
        } 
    }

    /* public IEnumerator ChargingBowAtk()
    {
        Atking = true;
        // 공격 애니메이션 시작
        // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

        //화살 소환
        GameObject ThrowArrow = Instantiate(arrow);
        ThrowArrow.GetComponent<ProjectileType>().Damage = meleeDmg;
        ThrowArrow.transform.position = atkPos[(int)WeaponType].position;
        ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
        Atking = false;
    }
 */
    public void ChargingBowAtk()
    {
        Atking = true;
        // 공격 애니메이션 시작
        // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

        //화살 소환
        GameObject ThrowArrow = Instantiate(arrow);
        ThrowArrow.GetComponent<ProjectileType>().Damage = meleeDmg;
        ThrowArrow.transform.position = atkPos[(int)WeaponType].position;
        ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
        Atking = false;
    }

    public IEnumerator ShieldGuard()
    {
        Atking = true;
        isGuard = true;
        movement2D.isDashing = true;
        float originalGravity = rigid2D.gravityScale;

        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(-(transform.localScale.x) * DashingPower, 0f);

        yield return new WaitForSeconds(ShieldDashingTime * 0.5f);

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(ShieldDashPos.position, ShieldDashAtkSize, 0);
        foreach(Collider2D collider in collider2Ds)
        {
            if(collider.tag == "Monster" || collider.tag == "Destruct")
            {
                Atk(collider.gameObject);
                // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
            }
        }

        yield return new WaitForSeconds(ShieldDashingTime * 0.5f);

        rigid2D.gravityScale = originalGravity;
        movement2D.isDashing = false;
        Atking = false;
        isGuard = false;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType]);    
        Gizmos.DrawWireCube(fallDownAtkPos.position, fallDownAtkSize);    
        Gizmos.DrawWireCube(ShieldDashPos.position, ShieldDashAtkSize);    
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(ComboAtkPos.position, ComboAtkSize);    
    }


   



    public void GetDamaged(float Damage)
    {
        if(isGuard) return;

        // Debug.Log("Get Damaged");

        curHp -= Damage;

        
        // StopCoroutine(ShieldGuard());
    }



    
}
