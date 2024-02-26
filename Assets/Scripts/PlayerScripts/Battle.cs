using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Rendering;

public class Battle : MonoBehaviour
{
    // 플레이어 기본 정보
    public Animator animator;
    private PlayerStatus status;
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    private PassiveSystem passive;
    private Synergy synergy;
    

    [TitleGroup("전투 관련 정보")]
    public WeaponTypes WeaponType;
    public bool atking;
    public bool isGuard;
    public GameObject arrow;
    public GameObject magic;
    public float originalScale;
    public float atkDamage { get { return status.AtkDamage();} }
    public float WeatheringDamage { get { return atkDamage * ( 1 + (synergy.SouthSpiritDamageIncreaseRate / 100)); } }
    

    [TitleGroup("무기 관련 기본 설정")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    [LabelText("무기 정보")] public List<WeaponData> weaponData;
    [System.Serializable] public class WeaponData
    {
        [LabelText("무기 타입")]
        public WeaponTypes weaponType;


        [Header("공격 관련 설정")]
        [LabelText("공격 위치")] public Transform atkPos;
        [LabelText("공격 범위")] public Vector2 atkSize;

        [LabelText("공격 쿨타임")] public float atkCoolTime;
        /*[LabelText("공격 준비 여부")]*/ public bool isAtkReady;

        [Header("진동 관련 설정")]
        [LabelText("진동 시간")] public float shakeDuration;
        [LabelText("진동 세기")] public float shakeForce;

        [Header("데미지 관련 정보")]
        [LabelText("크리티컬 데미지%")] public float weaponCrtDamage;
        [LabelText("데미지 %")] public float weaponDamage;
    }


    [TitleGroup("추가적인 검 설정")]
    [SerializeField] private bool canComboAtk = false;
    [LabelText("콤보 유지 시간")] [SerializeField] private float comboAtkTime;
    private float curComboTime;


    [TitleGroup("추가적인 방패 설정")]
    [Header("Shield Setting")]
    [LabelText("방패 일반공격 대쉬 시간")] [SerializeField] private float shieldAtkDashTime;
    [LabelText("방패 일반공격 대쉬 세기")] [SerializeField] private float shieldAtkDashPower;
    [LabelText("방패 차징공격 대쉬 시간")] [SerializeField] private float shieldDashAtkDashTime;
    [LabelText("방패 차징공격 대쉬 세기")] [SerializeField] private float shieldDashAtkDashPower;
    [LabelText("방패 차징공격 위치")] [SerializeField] private Transform shieldDashAtkPos;
    [LabelText("방패 차징공격 범위")] [SerializeField] private Vector2 shieldDashAtkSize;
    [SerializeField] BoxCollider2D boxCol;
    public GameObject barrier;
    // public bool 

    [TitleGroup("추가적인 활 설정")]
    [LabelText("활 기본 발사 속도")] [SerializeField] float arrowNormalSpeed;
    [LabelText("활 차징 공격 데미지%")] [SerializeField] float arrowChargeDamage;
    [LabelText("활 차징 발사 속도")] [SerializeField] float arrowChargeSpeed;
    [SerializeField] RuntimeAnimatorController chargingAnimator;
    

    [TitleGroup("추가적인 완드 설정")]
    [LabelText("완드 속도")] [SerializeField] float magicSpeed;
    [SerializeField] Transform wandSpawnPos;



    
    [TitleGroup("낙하 공격 설정")]
    public bool fallAtking;
    [LabelText("낙하 공격 위치")] [SerializeField] Transform fallDownAtkPos;
    [LabelText("낙하 공격 범위")] [SerializeField] Vector2 fallDownAtkSize;
    [LabelText("낙하 공격 후딜레이 시간")] [SerializeField] float fallDownAtkTime;
    float curFallDownAtkTime;
    bool canFallDownHit;
    

    
    [TitleGroup("스왑 설정")]
    [Header("Swap Setting")]
    [SerializeField] public bool isSwap;
    [LabelText("스왑 가능 시간")] [SerializeField] private float swapTime;



    [TitleGroup("추가적인 진동 설정")]
    [LabelText("피격시 진동 지속 시간")] [SerializeField] float DamagedDuration;
    [LabelText("피격시 진동 세기")] [SerializeField] float DamagedForce;
    

    [TitleGroup("피격 관련 설정")]
    [LabelText("피격 시 색깔")] [SerializeField] Color damagedColor;
    [LabelText("색깔 변경 속도")] [SerializeField] float damagedTime;


    void Awake()
    {
        synergy = GetComponent<Synergy>();
        passive = GetComponent<PassiveSystem>();
        status = GetComponent<PlayerStatus>();
        movement2D = GetComponent<Movement2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        
        fallAtking = false;
        originalScale = rigid2D.gravityScale;

        for (int i = 0; i < weaponData.Count; i ++)
            weaponData[i].isAtkReady = true;

        isSwap = true;
        animator = GetComponent<Animator>();
    }



    void Update()
    {
        if(fallAtking && movement2D.isNearFloor)
        {
            if(canFallDownHit)
            {   
                canFallDownHit = false;
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(fallDownAtkPos.position, fallDownAtkSize, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Monster" || collider.tag == "Destruct")
                    {
                        Atk(collider.gameObject);
                        ParticleManager.instance.SpawnParticle(collider.gameObject.transform.position, transform.position.x - collider.gameObject.transform.position.x);
                    }
                }
                AudioManager.instance.PlaySfx(AudioManager.Sfx.fallDownAtk);
            }
            
            if(curFallDownAtkTime <= fallDownAtkTime)
            {
                curFallDownAtkTime += Time.deltaTime;
            }
            else
            {
                animator.SetBool("isGround", true);
                animator.SetBool("isAct", false);
                fallAtking = false;
                atking = false;
                rigid2D.gravityScale = originalScale;
            }
            rigid2D.velocity = Vector2.zero;
        }

        if(passive.isGetBarrier)
            barrier.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f * (status.barrier / (status.maxHp * passive.shieldPer * 0.01f))) / 255;

        UIController.instance.HpFill.fillAmount = Mathf.Lerp(UIController.instance.HpFill.fillAmount, status.curHp / status.maxHp, Time.deltaTime * 5f);
        UIController.instance.BarrierFill.fillAmount = Mathf.Lerp(UIController.instance.BarrierFill.fillAmount, (status.barrier / status.maxHp), Time.deltaTime * 5f);

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
   
    public void GetDamaged(float Damage, bool canShake = true)
    {
        if(isGuard) return;

        bool isMiss = Random.Range(1, 100 + 1) <= status.missRate;
        


        if(isMiss) return;
        
        float getDmg = Damage * (100 - status.defPer) * 0.01f;
        // CameraController.instance.StartCoroutine(CameraController.instance.Shake(DamagedDuration, DamagedForce));
        // DOTween.Kill(transform);
        if(canShake)
        {
            GetComponent<SpriteRenderer>().DOColor(damagedColor, damagedTime).OnComplete(()=>
            GetComponent<SpriteRenderer>().DOColor(Color.white, damagedTime));
            CameraController.instance.ShakeCamera(DamagedDuration, DamagedForce);
        }

        if (status.barrier > getDmg)
        {
            status.barrier -= getDmg;
            getDmg = 0f;
        }
        else
        {
            getDmg -= status.barrier;
            status.barrier = 0f;
            passive.isGetBarrier = false;
            barrier.SetActive(false);
        }

        status.curHp -= getDmg;
    
            

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
        CameraController.instance.ShakeCamera(weaponData[(int)WeaponType].shakeDuration, weaponData[(int)WeaponType].shakeForce);
        
        ParticleManager.instance.SpawnParticle(AtkObj.transform.position, AtkObj.transform.position.x - transform.position.x);
        EffectManager.instance.SpawnEffect(AtkObj.transform.position, 1 + (int)WeaponType, Vector2.one);
        
        if(AtkObj.CompareTag("Monster"))
        {
            if(AtkObj.GetComponentInParent<MonsterSynergy>().isWeathering && WeaponType == WeaponTypes.Shield && passive.isGetBarrier)
            {
                Debug.Log(WeatheringDamage);
                AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(WeatheringDamage);
                status.barrier *= 1 - (synergy.BarrierDecreaseRate / 100);
                Debug.Log("베리어: " + status.barrier);
            }  
            else
            {
                Debug.Log(atkDamage);
                AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(atkDamage);
            }

            PlayerPasstive(AtkObj);
            if (WeaponType == WeaponTypes.Shield || WeaponType == WeaponTypes.Sword)
            {
                PlayerSynergy((int)WeaponType * 1000,AtkObj);
            }
            return;
        }

        if(AtkObj.CompareTag("Destruct"))
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
            return;
        }
    }

    public void AtkAction(int id, bool isCharging = false)
    {
        if(id == 0 && !movement2D.isGround)
        {
            if(WeaponType != WeaponTypes.Bow && WeaponType != WeaponTypes.Wand)
            {
                StartCoroutine(FallDownAtk());
                return;
            }
            
            if(WeaponType == WeaponTypes.Wand || (WeaponType == WeaponTypes.Bow && !isCharging))
            {
                return;
            }
        }

        if(WeaponType == WeaponTypes.Bow)
            AudioManager.instance.StopSfx(AudioManager.Sfx.BowCharging);
        if(!weaponData[(int)WeaponType].isAtkReady) return;

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
    }

    public IEnumerator Atking()
    {
        rigid2D.velocity = Vector2.zero;
        atking = true;
        weaponData[(int)WeaponType].isAtkReady = false;
        animator.SetBool("isAct", true);
        
        if(WeaponType == WeaponTypes.Wand) 
        {
            animator.SetTrigger("Atk");
        }
        else if(WeaponType == WeaponTypes.Sword)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordAtk);
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

        StartCoroutine(ReturnAttack());

        yield return null;
    }

    public void AtkDetection()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(weaponData[(int)WeaponType].atkPos.position, weaponData[(int)WeaponType].atkSize, 0);
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
        for (int i = 0; i < weaponData.Count; i++)
        {
            if (!weaponData[i].isAtkReady)
            {
                yield return new WaitForSeconds(weaponData[i].atkCoolTime / (status.atkSpeed * 0.01f));
                weaponData[i].isAtkReady = true;
            }
        }
    }
   
    public void AtkEnd()
    {
        atking = false;
        animator.SetBool("isAct", false);

        if(WeaponType == WeaponTypes.Wand)
        {
            GameObject Magic = Instantiate(magic);
            ProjectileType magicInfo = Magic.GetComponent<ProjectileType>();
            Magic.GetComponent<ProjectileType>().weaponType = WeaponType;
            magicInfo.Projectile = Type.Magic;
            magicInfo.Damage = atkDamage;
            Magic.transform.position = wandSpawnPos.position;
            Magic.transform.localScale = new Vector3(transform.localScale.x < 0 ? -Magic.transform.localScale.x : Magic.transform.localScale.x,
            Magic.transform.localScale.y, Magic.transform.localScale.z);
            
            magicInfo.moveSpeed = magicSpeed;
            magicInfo.duration = passive.passiveData[(int)WeaponType].duration;
            magicInfo.tick = passive.passiveData[(int)WeaponType].tick;
            magicInfo.per = passive.slowPer;

            Transform target = null;
            float targetDistance = 100;
            
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(weaponData[(int)WeaponType].atkPos.position, weaponData[(int)WeaponType].atkSize, 0);
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
                // Debug.Log("colliderDistance = " + colliderDistance);
                if(colliderDistance < targetDistance)
                    target = collider.transform;
            }
            magicInfo.target = target;
        }

    }

    public void PlayerPasstive(GameObject monster = null)
    {
        // if(passive.passiveRate[(int)WeaponType] <= 0 || passive.passiveRate[(int)WeaponType] < Random.Range(1, 100 + 1)) return;
        if(passive.passiveData[(int)WeaponType].rate <= 0 || passive.passiveData[(int)WeaponType].rate < Random.Range(1, 100 + 1)) return;
        
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
    public void PlayerSynergy(int WeaponID, GameObject monster = null )
    {
        if (monster != null )
        {
            synergy.ActiveSynergy(ElementalManager.instance.AddElement(WeaponID), monster.GetComponentInParent<MonsterSynergy>());
        }
    }

    #endregion

    #region 특수 공격 함수들
    public IEnumerator FallDownAtk()
    {
        if(!atking)
        {
            // 체공 시간
            curFallDownAtkTime = 0f;
            canFallDownHit = true;
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
        weaponData[(int)WeaponType].isAtkReady = false;
        atking = true;
        // 공격 애니메이션 시작

        //화살 소환
        GameObject shootArrow = Instantiate(arrow);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BowAtk);
        // AudioManager.instance.StopSfx(AudioManager.Sfx.BowCharging);

        if(isCharged)
        {
            shootArrow.GetComponent<ProjectileType>().Damage = atkDamage * arrowChargeDamage * 0.01f;
            shootArrow.GetComponent<ProjectileType>().moveSpeed = arrowChargeSpeed;
            shootArrow.GetComponent<Animator>().runtimeAnimatorController = chargingAnimator;
        }
        else
        {
            shootArrow.GetComponent<ProjectileType>().Damage = atkDamage;
            shootArrow.GetComponent<ProjectileType>().moveSpeed = arrowNormalSpeed;
        }
        shootArrow.GetComponent<ProjectileType>().weaponType = WeaponType;

        shootArrow.transform.position = weaponData[(int)WeaponType].atkPos.position;
        shootArrow.transform.localScale = new Vector3(transform.localScale.x, shootArrow.transform.localScale.y, shootArrow.transform.localScale.z);
        atking = false;
    }

    public IEnumerator DashGuard()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ShieldAtk);
        atking = true;
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = Vector2.zero;
        rigid2D.velocity = new Vector2(transform.localScale.x * shieldAtkDashPower, 0f);
        boxCol.enabled = true;
        yield return new WaitForSeconds(shieldAtkDashTime);
        
        boxCol.enabled = false;
        // rigid2D.gravityScale = originalGravity;
        rigid2D.gravityScale = 4f;
        if(atking)
        {
            AtkEnd();
            Debug.Log("Atking Cancel");
        }
            
    }

    public IEnumerator ShieldGuard()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ShieldAtk);
        atking = true;
        isGuard = true;
        movement2D.isDashing = true;
        float originalGravity = rigid2D.gravityScale;

        rigid2D.gravityScale = 0f;
        rigid2D.velocity = new Vector2(transform.localScale.x * shieldDashAtkDashPower, 0f);
        boxCol.enabled = true;
        yield return new WaitForSeconds(shieldDashAtkDashTime * 0.5f);

        yield return new WaitForSeconds(shieldDashAtkDashTime * 0.5f);
        boxCol.enabled = false;
        rigid2D.gravityScale = originalGravity;
        movement2D.isDashing = false;
        atking = false;
        isGuard = false;
    }
#endregion 

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
            
        // Gizmos.DrawWireCube(atkPos[(int)WeaponType].position, atkSize[(int)WeaponType]);
        Gizmos.DrawWireCube(weaponData[(int)WeaponType].atkPos.position, weaponData[(int)WeaponType].atkSize);
        
            
        Gizmos.DrawWireCube(fallDownAtkPos.position, fallDownAtkSize);
        if(WeaponType == WeaponTypes.Shield)
            Gizmos.DrawWireCube(shieldDashAtkPos.position, shieldDashAtkSize);
    }
}