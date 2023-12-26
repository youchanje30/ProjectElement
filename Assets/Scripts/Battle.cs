using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Battle : MonoBehaviour
{
    public Animator animator;
    private PlayerStatus status;
    
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
    public float atkDamage { get { return status.AtkDamage();} }

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

    [Header("Swap Setting")]
    [SerializeField] public bool isSwap;
    [SerializeField] private float swapTime;



    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        movement2D = GetComponent<Movement2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        
        fallAtking = false;
        originalScale = rigid2D.gravityScale;
        // weaponType = 0;    
        for (int i = 0; i < isAtkReady.Length; i++)
        {
            isAtkReady[i] = true;
        }
        isSwap = true;
        animator = GetComponent<Animator>();
    }



    void Update()
    {
        // Debug.Log(CanComboAtk);

        Vector2 underVec = new Vector2(rigid2D.transform.position.x - 0.5f, rigid2D.transform.position.y - 0.9f);
        Debug.DrawRay(underVec, new Vector3(0, -0.2f, 0), new Color(0,1,0));
        RaycastHit2D MiddleRaycast = Physics2D.Raycast(underVec, Vector3.down, 0.2f ,LayerMask.GetMask("Platform"));

        Vector2 leftUnderVec = new Vector2(rigid2D.transform.position.x, rigid2D.transform.position.y - 0.9f);
        Debug.DrawRay(leftUnderVec, new Vector3(0, -0.2f, 0), new Color(0,1,0));
        RaycastHit2D LeftRaycast = Physics2D.Raycast(leftUnderVec, Vector3.down, 0.2f ,LayerMask.GetMask("Platform"));

        Vector2 rightUnderVec = new Vector2(rigid2D.transform.position.x + 0.5f, rigid2D.transform.position.y - 0.9f);
        Debug.DrawRay(rightUnderVec, new Vector3(0, -0.2f, 0), new Color(0,1,0));
        RaycastHit2D RightRaycast = Physics2D.Raycast(rightUnderVec, Vector3.down, 0.2f ,LayerMask.GetMask("Platform"));

        // if(fallAtking && movement2D.isGround) //착지 공격 참 + 땅에 도착 참
        if(fallAtking && (MiddleRaycast.collider != null || LeftRaycast.collider != null || RightRaycast.collider != null))
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
            // bool isCrt = Random.Range(1, 100 + 1) <= crtRate;
            // float NormalDmg = damage * damagePer * 0.01f;
            // float FinalDmg = isCrt ? NormalDmg * crtDmg * 0.01f : NormalDmg;

            AtkObj.GetComponent<Monster>().GetDamaged(atkDamage);
            // AtkObj.GetComponent<Monster>().GetDamaged(meleeDmg);
        }

        if(AtkObj.tag == "Destruct")
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
        }

    }

    public IEnumerator ReturnSwap()
    {
        if(isSwap == false)
        {
            yield return new WaitForSeconds(swapTime);
            isSwap = true;
        }
       
    }
    public IEnumerator ReturnAttack()
    {
        for(int i = 0;i<isAtkReady.Length;i++)
        {
            if (!isAtkReady[i])
            {

                yield return new WaitForSeconds(Left_AtkCoolTime[i] / (status.atkSpeed * 0.01f));
                isAtkReady[i] = true;
            }
        }     
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
        rigid2D.velocity = Vector2.zero;
        rigid2D.velocity = new Vector2(-(transform.localScale.x) * AtkDashingPower, 0f);
        yield return new WaitForSeconds(ShieldAtkDashTime);
        // rigid2D.gravityScale = originalGravity;
        rigid2D.gravityScale = 4f;
    }

    // 체력 회복
    public void HealHp(float increaseHp)
    {
        if(status.curHp + increaseHp <= status.maxHp)
        {
            status.curHp += increaseHp;
        }
        else
        {
            status.curHp = status.maxHp;
        }
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
            rigid2D.velocity = Vector2.zero;
            Atking = true;
            //isSwap = false; 스왑 1안 추가 나중에 주석 풀면 됨
            isAtkReady[(int)WeaponType] = false;               
            
          
            animator.SetBool("isAct", true);

            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Left_BeforAtkDelay[weaponType]);
            if(WeaponType == WeaponTypes.Wand) 
            {
                animator.SetTrigger("Atk");
            }
            else if(WeaponType == WeaponTypes.Sword && CanComboAtk) //칼의 경우 콤보 여부에 따른 어택 변경
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
            
            /*yield return new WaitForSeconds((Left_AtkCoolTime[(int)WeaponType] / (status.atkSpeed * 0.01f)));
            isSwap = true; 나중에 주석 풀면 됨*/
            StartCoroutine(ReturnAttack());

            yield return null;




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
        if(WeaponType == WeaponTypes.Bow)
        {
            GameObject ThrowArrow = Instantiate(arrow);
            ThrowArrow.GetComponent<ProjectileType>().Damage = atkDamage;
            ThrowArrow.transform.position = atkPos[(int)WeaponType].position;
            ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
        }

        if(WeaponType == WeaponTypes.Wand)
        {
            GameObject Magic = Instantiate(arrow);
            Magic.GetComponent<ProjectileType>().Projectile = Type.Magic;
            Magic.GetComponent<ProjectileType>().Damage =  atkDamage;
            // Magic.transform.position = atkPos[(int)WeaponType].position;
            Magic.transform.position = transform.position;

            Transform target = null;
            float targetDistance = 100;
            // Debug.Log("targetDistance = " + targetDistance);
            // Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType].x);
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType], 0);
            bool isMonster = false;
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag != "Monster" && collider.tag != "Destruct")
                    continue;

                if(collider.CompareTag("Destruct") && isMonster)
                    continue;
                

                if(collider.CompareTag("Monster") && !isMonster)
                {
                        isMonster = true;
                        target = collider.transform;
                }
                
                if(target != null)
                    targetDistance = Vector2.Distance(transform.position , target.transform.position);

                float colliderDistance = Vector2.Distance(transform.position , collider.transform.position);
                Debug.Log("colliderDistance = " + colliderDistance);
                if(colliderDistance < targetDistance)
                    target = collider.transform;
            }
            Magic.GetComponent<ProjectileType>().target = target;
        }

        
        Atking = false;
        animator.SetBool("isAct", false);
    }


    public IEnumerator ComboAtk()
    {
        yield return new WaitForSeconds(ComboTime/(status.atkSpeed * 0.01f));
        CanComboAtk = false;
    }


    public IEnumerator BowAtk()
    {
        if(isAtkReady[(int)WeaponType])
        {
            rigid2D.velocity = Vector2.zero;
            animator.SetTrigger("Atk");
            Atking = true;
            isAtkReady[(int)WeaponType] = false;
            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

            //화살 소환
            
            // Atking = false;

            yield return new WaitForSeconds(Left_AtkCoolTime[(int)WeaponType]/(status.atkSpeed * 0.01f));

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
    }*/
 
    public void ChargingBowAtk()
    {
        Atking = true;
        // 공격 애니메이션 시작
        // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

        //화살 소환
        GameObject ThrowArrow = Instantiate(arrow);
        ThrowArrow.GetComponent<ProjectileType>().Damage = atkDamage;
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
        if(WeaponType == WeaponTypes.Wand)
        {
            // Gizmos.DrawWireSphere(transform.position, atkSize[(int)WeaponType].x);
            Gizmos.DrawWireCube(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType]);
        }  
        Gizmos.DrawWireCube(ComboAtkPos.position, ComboAtkSize);    
    }


   


    // 데미지 받을 때 처리하는 함수
    public void GetDamaged(float Damage)
    {
        if(isGuard) return;

        bool isMiss = Random.Range(1, 100 + 1) <= status.missRate;

        if(!isMiss)
            status.curHp -= Damage * (100 - status.defPer) * 0.01f;

        if(status.curHp <= 0)
        {
            SaveManager.instance.ResetData();
            SceneManager.LoadScene("Maintown");
        }    
        // StopCoroutine(ShieldGuard());
    }
}
