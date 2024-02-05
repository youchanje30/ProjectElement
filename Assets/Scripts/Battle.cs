using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Battle : MonoBehaviour
{
    public Animator animator;
    private PlayerStatus status;
    
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    private PassiveSystem passive;
    private Synergy synergy;
    public bool fallAtking;
    [Header("진동 설정")]
    [SerializeField] float[] shakeDuration;
    [SerializeField] float[] shakeForce;
    [SerializeField] float DamagedDuration;
    [SerializeField] float DamagedForce;
    



    [Header("Weapon Setting")]
    [Tooltip("무기 타입")]
    public WeaponTypes WeaponType;
    public bool atking;
    public bool isGuard;
    public float[] atkCoolTime;
    public bool[] isAtkReady;
    public Transform[] atkPos;
    public Vector2[] atkSize;
    // public Transform[] atkPos;
    public GameObject arrow;

    public float originalScale;
    public float atkDamage { get { return status.AtkDamage();} }
    public float WeatheringDamage { get { return atkDamage * ( 1 + (synergy.SouthSpiritDamageIncreaseRate / 100)); } }

    [Header("Sword Setting")]
    [SerializeField] private bool canComboAtk = false;
    [SerializeField] private float comboAtkTime;
    private float curComboTime;


    [Header("Shield Setting")]
    [SerializeField] private float ShieldAtkDashTime;
    [SerializeField] private float AtkDashingPower;
    [Space(5f)]
    [SerializeField] private float ShieldDashingTime;
    [SerializeField] private float DashingPower;
    [SerializeField] private Vector2 ShieldDashAtkSize;
    [SerializeField] private Transform ShieldDashPos;
    public GameObject barrier;
    // public bool 
    [Space(20f)]

    [SerializeField] Transform fallDownAtkPos;
    [SerializeField] Vector2 fallDownAtkSize;

    [Header("Swap Setting")]
    [SerializeField] public bool isSwap;
    [SerializeField] private float swapTime;



    void Awake()
    {
        synergy = GetComponent<Synergy>();
        passive = GetComponent<PassiveSystem>();
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
            atking = false;
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
        barrier.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, (status.barrier / (status.maxHp * passive.shieldPer * 0.01f)));
        UIController.instance.HpFill.fillAmount = Mathf.Lerp(UIController.instance.HpFill.fillAmount, status.curHp / status.maxHp, Time.deltaTime * 5f);

        if(WeaponType == WeaponTypes.Sword)
        {
            if(canComboAtk)
            {
                curComboTime += Time.deltaTime;
                if(curComboTime >= comboAtkTime)
                    canComboAtk = false;
            }
            else
            {
                curComboTime = 0;
            }
        }
    }

 
    #region 기본 함수
   
    public void GetDamaged(float Damage)
    {
        if(isGuard) return;

        bool isMiss = Random.Range(1, 100 + 1) <= status.missRate;

        if(!isMiss)
        {
            float getDmg = Damage * (100 - status.defPer) * 0.01f;
            // CameraController.instance.StartCoroutine(CameraController.instance.Shake(DamagedDuration, DamagedForce));
            
            CameraController.instance.ShakeCamera(DamagedDuration, DamagedForce);
            if (status.barrier > getDmg)
            {
                status.barrier -= getDmg;
                getDmg = 0f;
            }
            else
            {
                getDmg -= status.barrier;
                status.barrier = 0f;
            }

            status.curHp -= getDmg;
        }
            

        passive.Atked();

        if(status.curHp <= 0)
        {
            
            SaveManager.instance.ResetData();
            SceneManager.LoadScene("Main Scene");
            
        }    
        // StopCoroutine(ShieldGuard());
    }

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

#endregion

    #region 공격 관련 함수들
    public void Atk(GameObject AtkObj)
    {
        // CameraController.instance.StartCoroutine(CameraController.instance.Shake(shakeDuration[(int)WeaponType], shakeForce[(int)WeaponType]));
        CameraController.instance.ShakeCamera(shakeDuration[(int)WeaponType], shakeForce[(int)WeaponType]);
        // CameraController.instance.StartCoroutine(CameraController.instance.ShakeR(shakeDuration[(int)WeaponType], shakeForce[(int)WeaponType]));
        
        if(AtkObj.CompareTag("Monster"))
        {       
            if(AtkObj.GetComponentInParent<MonsterSynergy>().isWeathering && WeaponType == WeaponTypes.Shield)
            {
                Debug.Log(WeatheringDamage);
                AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(WeatheringDamage);
                status.barrier *= 1 - (synergy.BarrierDecreaseRate / 100);
                Debug.Log("베리어: " +status.barrier);
            }  
            else
            {
                Debug.Log(atkDamage);
                AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(atkDamage);
            }
            PlayerPasstive(AtkObj);
            PlayerSynergy(AtkObj);

            return;
        }

        if(AtkObj.CompareTag("Destruct"))
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
            return;
        }
    }

    public void AtkAction(int id)
    {
        if(id == 0 && !movement2D.isGround)
        {
            StartCoroutine(FallDownAtk());
            return;
        }

        if(!isAtkReady[(int)WeaponType]) return;

        if(WeaponType == WeaponTypes.Bow)
        {
            ChargingBowAtk(id != 0);
            StartCoroutine(ReturnAttack());
            return;
        }

        if(id == 0) //좌클릭
        {
            StartCoroutine(Atking());
            if(WeaponType == WeaponTypes.Shield) StartCoroutine(DashGuard());
        }
        else
        {    
            if(WeaponType == WeaponTypes.Shield) StartCoroutine(ShieldGuard());
        }
        // atking = true;
    }

    public IEnumerator Atking()
    {
        rigid2D.velocity = Vector2.zero;
        atking = true;
        isAtkReady[(int)WeaponType] = false;
        animator.SetBool("isAct", true);

        //isSwap = false; 스왑 1안 추가 나중에 주석 풀면 됨
        // yield return new WaitForSeconds(Left_BeforAtkDelay[weaponType]);
        
        if(WeaponType == WeaponTypes.Wand) 
        {
            animator.SetTrigger("Atk");
        }
        else if(WeaponType == WeaponTypes.Sword)
        {
            if(canComboAtk)
                animator.SetTrigger("ComboAtk");
            else
                animator.SetTrigger("Atk");
            canComboAtk = !canComboAtk;
        }
        else
        {
            animator.SetTrigger("Atk");
        }
        // 공격 중인거 종료 

        /*yield return new WaitForSeconds((Left_AtkCoolTime[(int)WeaponType] / (status.atkSpeed * 0.01f)));
        isSwap = true; 나중에 주석 풀면 됨*/
        StartCoroutine(ReturnAttack());

        yield return null;
    }

    public void AtkDetection()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType], 0);
        foreach(Collider2D collider in collider2Ds)
        {
            if(collider.tag == "Monster" || collider.tag == "Destruct")
            {
                Atk(collider.gameObject);
            }
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
        for (int i = 0; i < isAtkReady.Length; i++)
        {
            if (!isAtkReady[i])
            {
                yield return new WaitForSeconds(atkCoolTime[i] / (status.atkSpeed * 0.01f));
                isAtkReady[i] = true;
            }
        }
    }
   
    public void AtkEnd()
    {
        atking = false;
        animator.SetBool("isAct", false);

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
            ProjectileType magic = Magic.GetComponent<ProjectileType>();
            magic.Projectile = Type.Magic;
            magic.Damage =  atkDamage;
            Magic.transform.position = transform.position;
            Magic.transform.localScale = new Vector3(transform.localScale.x, Magic.transform.localScale.y, Magic.transform.localScale.z);

            magic.duration = passive.duration[(int)WeaponType];
            magic.tick = passive.tick[(int)WeaponType];
            magic.per = passive.slowPer;

            Transform target = null;
            float targetDistance = 100;
            
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
            magic.target = target;
        }

    }

    public void PlayerPasstive(GameObject monster = null)
    {
        if(passive.passiveRate[(int)WeaponType] <= 0 || passive.passiveRate[(int)WeaponType] < Random.Range(1, 100 + 1)) return;

        // if(monster != null && WeaponType == WeaponTypes.Wand)
        // {
        //     if(monster.GetComponent<PassiveSystem>().canSlow)
        //         return false;
        // }

    
        if(monster != null)
        {
            passive.ActivePassive(WeaponType , monster.GetComponentInParent<MonsterDebuffBase>());
            //passive.ActiveSynergy(WeaponType, monster.GetComponentInParent<MonsterSynergy>());
        }
    }
    public void PlayerSynergy(GameObject monster = null)
    {
        if (monster != null)
        {
            synergy.ActiveSynergy(WeaponType, monster.GetComponentInParent<MonsterSynergy>());
        }
    }

    #endregion

    #region 특수 공격 함수들
    public IEnumerator FallDownAtk()
    {
        if(!atking)
        {
            // 체공 시간
            // originalScale = rigid2D.gravityScale;
            atking = true;
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

    public void ChargingBowAtk(bool isCharged = false)
    {
        isAtkReady[(int)WeaponType] = false;
        atking = true;
        // 공격 애니메이션 시작

        //화살 소환
        GameObject ThrowArrow = Instantiate(arrow);
        if(isCharged)
            ThrowArrow.GetComponent<ProjectileType>().Damage = atkDamage;
        else
            ThrowArrow.GetComponent<ProjectileType>().Damage = atkDamage / 2;

        ThrowArrow.transform.position = atkPos[(int)WeaponType].position;
        ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
        atking = false;
    }

    public IEnumerator DashGuard()
    {
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = Vector2.zero;
        rigid2D.velocity = new Vector2(transform.localScale.x * AtkDashingPower, 0f);
        yield return new WaitForSeconds(ShieldAtkDashTime);
        // rigid2D.gravityScale = originalGravity;
        rigid2D.gravityScale = 4f;
        if(atking)
            AtkEnd();
    }

    public IEnumerator ShieldGuard()
    {
        atking = true;
        isGuard = true;
        movement2D.isDashing = true;
        float originalGravity = rigid2D.gravityScale;

        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(transform.localScale.x * DashingPower, 0f);

        yield return new WaitForSeconds(ShieldDashingTime * 0.5f);
        Debug.Log("DashGuard Atk");

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(ShieldDashPos.position, ShieldDashAtkSize, 0);
        foreach(Collider2D collider in collider2Ds)
        {
            if(collider.tag == "Monster" || collider.tag == "Destruct")
            {
                Atk(collider.gameObject);
            }
        }

        yield return new WaitForSeconds(ShieldDashingTime * 0.5f);

        rigid2D.gravityScale = originalGravity;
        movement2D.isDashing = false;
        atking = false;
        isGuard = false;
    }
#endregion 

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
        // Gizmos.DrawWireCube(ComboAtkPos.position, ComboAtkSize);    
    }


}
