using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ActiveSkill : MonoBehaviour
{
    // 기본 설정
    private Animator animator;
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    private Battle battle;
    public PassiveSystem passive;
    public PlayerStatus status;


    
    // 스킬 관련 설정
    [TitleGroup("스킬 관련 기본 정보")]
    [System.Serializable] public class SkillData
    {
        [LabelText("무기 이름")] public string weaponName;
        [LabelText("스킬 쿨타임")] public float skillCoolTime;
        public bool isSkillReady;
    }
    [LabelText("스킬 데이터")] public List<SkillData> skillData;
    // public float[] SkillCoolTime;
    // public bool[] SkillReady;
    public bool isCharging = false;
    public float DefaultDamage { get { return battle.atkDamage; } }
    public bool  isUnderTheSea;
    [Space(20f)]

    #region 불
    [TitleGroup("불 스킬 정보")]
    public GameObject FireFloor;
    [LabelText("스킬 길이")] public int RangeCount;
    [LabelText("스킬 발동 시 데미지")] public float DiffusionDamageIncreaseRate;
    [Tooltip("바닥 감지 길이")] public int detectlength;
    public LayerMask layer;
    [LabelText("스킬 발동 시간")] public float ActiveTime;
    [LabelText("스킬 발동 시 진동 시간")] public float FireShakeTime;
    [LabelText("스킬 발동 시 진동 세기")]  public float FireShakeMagnitude;
    public Transform FirePos;
    public Vector2 FireRange;
    [LabelText("불 스킬 데미지 증가율")] public float FIreDamage;
    RaycastHit2D hit;
    RaycastHit2D hit1;
    #endregion
    [Space(20f)]

    #region 물
    [TitleGroup("물 스킬 정보")]
    public GameObject WaterBall;
    public bool isWater = false;
    [LabelText("1타 데미지 증가율")] public float WaterDamageIncreaseRate;
    [LabelText("2타 데미지 증가율")] public float BombDamageIncreaseRate;
    [LabelText("올라가는 속도")] public float FloatingForce;
    [LabelText("올라가는 시간")] public float FloatingTime;
    [LabelText("차징 시간")] public float WaterChargeTime;
    [LabelText("투사체 발사 위치")] public Vector2[] BallPos;
    [LabelText("투사체 중력 세기")] public float BallGravity;
    [LabelText("투사체 속도")] public float BallSpeed;
    [LabelText("폭파 공격 범위")] public Vector2 BombRange;
    [LabelText("2타 차징 시간")] public float BombChargeTime;
    public float WaterY;
    [LabelText("폭탄 발동 시간")] public float activeTime;
    [LabelText("폭탄 진동 시간")] public float BombShakeTime;
    [LabelText("폭탄 진동 세기")] public float BombShakeMagnitude;
    #endregion
    [Space(20f)]

    #region 땅
    [TitleGroup("땅 스킬 정보")]
    public bool isSouth = false;
    [LabelText("점프 시 데미지 증가율")] public float JumpDamageIncreaseRate;
    [LabelText("착지 시 데미지 증가율")] public float LandDamageIncreaseRate;
    [LabelText("점프 세기")] public float JumpForce;
    [LabelText("낙하 세기")] public float FallForce;
    [LabelText("스턴 시간")] public float StunTime;
    [LabelText("점프 시간")] public float RisingTime;
    [LabelText("위: 착지 공격 위치 | 아래: 점프 공격 위치")] public Transform[] LandingPos;
    [LabelText("위: 착지 공격 범위 | 아래: 점프 공격 범위")] public Vector3[] LandingRange;
    [LabelText("점프 시 진동 시간 ")] public float JumpShakeTime;
    [LabelText("점프 시 진동 세기")] public float JumpShakeMagnitude;
    [LabelText("타격 시 진동 시간")] public float SouthShakeTime;
    [LabelText("타격 시 진동 세기")] public float SouthShakeMagnitude;
    [LabelText("착지 시 진동 시간")] public float LandingShakeTime;
    [LabelText("착지 시 진동 세기 ")] public float LandingShakeMagnitude;
    private bool CanLanding;
    #endregion
    [Space(20f)]
    
    #region 바람
    [TitleGroup("바람 스킬 정보")]
    public GameObject Arrow;
    [LabelText("화살 소환 위치")] public Transform Pos;
    [LabelText("화살 데미지 증가율 %")] public float ArrowDamageIncreaseRate;
    //[Tooltip("���� �� ������ ������ %")]
    //public float DeclindRate;
    [LabelText("차징 시간")] public float ChargeTime;
    [LabelText("무적 시간")] public float InvicibleTime;
    [LabelText("발사 후 딜레이")] public float Delay;
    [LabelText("발사 시 진동 시간 ")] public float WindShakeTime;
    [LabelText("발사 시 진동 세기")] public float WindShakeMagnitude;
    [LabelText("활 스킬 발사 속도")] [SerializeField] float arrowSkillSpeed;
    #endregion

    void Awake()
    {
        // 제공 기능들
        animator =  GetComponent<Animator>();
        movement2D = GetComponent<Movement2D>();
        rigid2D = GetComponent<Rigidbody2D>();

        // 제작된 스크립트
        passive = GetComponent<PassiveSystem>();
        battle = GetComponent<Battle>();
        
        status = GetComponent<PlayerStatus>();

        // for (int i = 0; i < SkillReady.Length; i++)
        // {
        //     SkillReady[i] = true;
        // }
        for (int i = 0; i < skillData.Count; i++)
        {
            skillData[i].isSkillReady = true;
        }
    }
    void Update()
    {      
        if (isSouth)
        {
            Debug.Log("isSouth Atk");
            movement2D.curDashCnt = -1;
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[1].position, LandingRange[1], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.CompareTag("Monster") && collider.GetComponentInParent<MonsterBase>().isHit == false)
                {                   
                    collider.GetComponentInParent<MonsterBase>().isHit = true;
                    StartCoroutine(collider.GetComponentInParent<MonsterSynergy>().HitFalse());
                    // StartCoroutine(Hit(collider.gameObject, 0.5f));
                    // CameraController.instance.StartCoroutine(CameraController.instance.Shake(SouthShakeTime, SouthShakeMagnitude));
                    SkillAtk(collider.gameObject, DefaultDamage * (1 + (FIreDamage / 100)));              
                }
                if( collider.CompareTag("Destruct"))
                {
                    SkillAtk(collider.gameObject, DefaultDamage * (1 + (FIreDamage / 100)));
                }
            }
            Invoke("RandingSet", RisingTime);
         
        }
        for (int i = 0; i < RangeCount; i++)
        {
            Debug.DrawRay(new Vector2(transform.position.x + i, transform.position.y ), transform.up * -detectlength, Color.red);
            Debug.DrawRay(new Vector2(transform.position.x - i, transform.position.y ), Vector3.down * detectlength, Color.red);
        }

    }

    public void TriggerSkill(WeaponTypes weapontype)
    {
        switch (weapontype)
        {
            case WeaponTypes.Sword:
                //CameraController.instance.ShakeCamera(FireShakeTime, FireShakeMagnitude);
                //FireSkill();
                break;
            case WeaponTypes.Wand:
                animator.SetTrigger("Skill");
                WaterY = transform.position.y;
                StartCoroutine(WaterSkill());
                break;
            case WeaponTypes.Shield:
                // CameraController.instance.StartCoroutine(CameraController.instance.Shake(JumpShakeTime, JumpShakeMagnitude));
                animator.SetTrigger("Skill");CameraController.instance.ShakeCamera(JumpShakeTime, JumpShakeMagnitude);
                StartCoroutine(SouthSkill());
                break;
            case WeaponTypes.Bow:
                // animator.SetBool("isCharge", true);
                // animator.SetTrigger("Charging");
                animator.SetTrigger("Skill");
                StartCoroutine(WindSkill());
                break;
        }
    }

    #region 불 정령
    public void FireSkill()
    {
        CameraController.instance.ShakeCamera(FireShakeTime, FireShakeMagnitude);
        skillData[(int)battle.WeaponType].isSkillReady = false;
        
        EffectManager.instance.SpawnEffect(FirePos.position, (int)SkillEffect.Fire, FireRange);

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(FirePos.position, FireRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Monster") && collider.GetComponentInParent<MonsterBase>().isHit == false)
            {
                collider.GetComponentInParent<MonsterBase>().isHit = true;
                StartCoroutine(collider.GetComponentInParent<MonsterSynergy>().HitFalse());
                CameraController.instance.ShakeCamera(SouthShakeTime, SouthShakeMagnitude);

                SkillAtk(collider.gameObject, DefaultDamage * (1 + (JumpDamageIncreaseRate / 100)));
            }
            if (collider.CompareTag("Destruct"))
            {
                SkillAtk(collider.gameObject, DefaultDamage * (1 + (JumpDamageIncreaseRate / 100)));
            }
        }
        for (int i = 0; i < RangeCount; i++)
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x + i, transform.position.y), transform.up * -20, detectlength, layer);
            if (hit.collider != null)
            {
                GameObject Fire = Instantiate(FireFloor);
                Fire.transform.position = new Vector2(transform.position.x + i, transform.position.y - hit.distance);
                Fire.GetComponent<ProjectileType>().Damage = DefaultDamage * (1 + (DiffusionDamageIncreaseRate / 100));
            }
            hit1 = Physics2D.Raycast(new Vector2(transform.position.x - i, transform.position.y), transform.up * -20, detectlength, layer);
            if (hit1.collider != null)
            {
                GameObject Fire = Instantiate(FireFloor);
                Fire.transform.position = new Vector2(transform.position.x - i, transform.position.y - hit1.distance);
                Fire.GetComponent<ProjectileType>().Damage = DefaultDamage * (1 + (DiffusionDamageIncreaseRate / 100));
            }
        }
        battle.isSwap = true;
        battle.atking = false;
        StartCoroutine(ReturnSkill(1));
    }
    #endregion

    #region 물 정령
    public IEnumerator WaterSkill()
    {
        float gravity;
        skillData[(int)battle.WeaponType].isSkillReady = false;
    
        // SkillReady[(int)battle.WeaponType] = false;
        isWater = true;
        battle.isSwap = false;
        isCharging = true;
        gravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0;
        rigid2D.velocity = new Vector2(0, FloatingForce);
        yield return new WaitForSeconds(FloatingTime); 
        rigid2D.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(WaterChargeTime);
        GameObject ball;
        for (int i = 0; i < 3; i++)
        {
            ball = Instantiate(WaterBall);
            ball.GetComponent<ProjectileType>().Damage = DefaultDamage * (1 + (WaterDamageIncreaseRate / 100));
            ball.GetComponent<Rigidbody2D>().gravityScale = BallGravity;
            ball.transform.position = transform.position;
            ball.GetComponent<Rigidbody2D>().velocity = BallPos[i] * BallSpeed;                        
        }
        battle.isSwap = true;
        isCharging = false;
        isWater = false;
        // SkillReady[(int)battle.WeaponType] = true;
        //skillData[(int)battle.WeaponType].isSkillReady = true;
        rigid2D.gravityScale = gravity;
        StartCoroutine(ReturnSkill(2));  
    }
    #endregion

    #region 땅 정령
    public IEnumerator SouthSkill()
    {
        // SkillReady[(int)battle.WeaponType] = false;
        skillData[(int)battle.WeaponType].isSkillReady = false;
        battle.isSwap = false;
        isSouth = true;
        battle.isGuard = true;
        CanLanding = true;
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, JumpForce);
        yield return new WaitForSeconds(RisingTime);
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, -FallForce);

        
    }
    public void RandingSet()
    {

        if (Physics2D.Raycast(transform.position,Vector2.down,2,layer) && CanLanding)
            {
            // CameraController.instance.StartCoroutine(CameraController.instance.Shake(LandingShakeTime, LandingShakeMagnitude));
            rigid2D.velocity = new Vector2(0, -10);
                CameraController.instance.ShakeCamera(LandingShakeTime, LandingShakeMagnitude);

                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[0].position, LandingRange[0], 0);

                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.CompareTag("Monster") && collider.GetComponentInParent<MonsterBase>().isHit == false)
                    {
                        //StartCoroutine(Hit(collider.gameObject, 0.5f));
                        collider.GetComponentInParent<MonsterBase>().isHit = true;
                        StartCoroutine(collider.GetComponentInParent<MonsterSynergy>().HitFalse());
                        StartCoroutine(Stun(collider.gameObject, StunTime));
                        SkillAtk(collider.gameObject, DefaultDamage * (1 + (LandDamageIncreaseRate / 100)));
                    }
                    if (collider.CompareTag("Destruct"))
                    {
                        SkillAtk(collider.gameObject, DefaultDamage * (1 + (LandDamageIncreaseRate / 100)));
                    }
                }
                CanLanding = false;
                battle.isSwap = true;
                isSouth = false;
                battle.isGuard = false;
                EffectManager.instance.SpawnEffect(transform.position, (int)SkillEffect.South, LandingRange[0]);
                StartCoroutine(movement2D.DashCoolDown(0.01f));
                StartCoroutine(ReturnSkill(3));
        }
        
    }
    public IEnumerator Stun(GameObject monster , float Stuntime)
    {
       // monster.GetComponentInParent<MonsterBase>().moveSpeed = 0;
        monster.GetComponentInParent<MonsterBase>().isStun = true;
        yield return new WaitForSeconds(Stuntime);
        // monster.GetComponentInParent<MonsterBase>().moveSpeed = monster.GetComponentInParent<MonsterBase>().monsterData.maxMoveSpeed;
        monster.GetComponentInParent<MonsterBase>().isStun = false;
    }
    //public IEnumerator Hit(GameObject monster, float time)
    //{
    //    // monster.GetComponentInParent<MonsterBase>().moveSpeed = 0;
    //    monster.GetComponentInParent<MonsterBase>().isHit = true;
    //    yield return new WaitForSeconds(time);
    //    // monster.GetComponentInParent<MonsterBase>().moveSpeed = monster.GetComponentInParent<MonsterBase>().monsterData.maxMoveSpeed;
    //    monster.GetComponentInParent<MonsterBase>().isHit = false;
    //}
    #endregion

    #region 바람 정령
    public IEnumerator WindSkill()
    {
        isCharging = true;
        battle.isSwap = false;
        // SkillReady[(int)battle.WeaponType] = false;
        skillData[(int)battle.WeaponType].isSkillReady = false;

        yield return new WaitForSeconds(ChargeTime);
        animator.SetBool("isCharge", false);
        battle.isGuard = true;
        GameObject MagicArrow = Instantiate(Arrow);
        CameraController.instance.ShakeCamera(WindShakeTime, WindShakeMagnitude);
        MagicArrow.GetComponent<ProjectileType>().Damage = DefaultDamage * (1 + (ArrowDamageIncreaseRate/100));
        MagicArrow.GetComponent<ProjectileType>().moveSpeed = arrowSkillSpeed;

        MagicArrow.transform.position = Pos.position;
        MagicArrow.transform.localScale = new Vector3(transform.localScale.x, MagicArrow.transform.localScale.y, MagicArrow.transform.localScale.z);
        StartCoroutine(ReturnSkill(4));
        StartCoroutine(delay());
        Debug.Log("현재 무적시간 입니다.");
        yield return new WaitForSeconds(InvicibleTime);
        battle.isSwap = true;
        battle.isGuard = false;
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(Delay);
    }
    #endregion


    public IEnumerator ReturnSkill(int i)
    {
        isCharging = false;
        // for (int i = 0; i < SkillReady.Length; i++)
        // {
        //     if (!SkillReady[i])
        //     {
        //         yield return new WaitForSeconds(SkillCoolTime[i] *= 1 - (status.coolDownReductionPer/100));
        //         SkillReady[i] = true;
        //     }
        // }
                yield return new WaitForSeconds(skillData[i].skillCoolTime * (1 - (status.coolDownReductionPer/100)));
                // SkillReady[i] = true;
                skillData[i].isSkillReady = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(LandingPos[0].position, LandingRange[0]);
        Gizmos.DrawWireCube(LandingPos[1].position, LandingRange[1]);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(FirePos.position, FireRange);
    }
    public void SkillAtk(GameObject AtkObj, float Damage)
    {
        //CameraController.instance.StartCoroutine(CameraController.instance.Shake());

        if (AtkObj.CompareTag("Monster"))
        {
            Debug.Log(Damage);
            AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
            EffectManager.instance.SpawnEffect(AtkObj.transform.position, 1 + (int)battle.WeaponType, Vector2.one);
        }

        if (AtkObj.CompareTag("Destruct"))
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
        }

    }
}
