using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ActiveSkill : MonoBehaviour
{
    private Battle battle;
    private Animator animator;
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    public PassiveSystem passive;
    public PlayerStatus status;
    public float[] SkillCoolTime;
    public bool[] SkillReady;
    public bool isCharging = false;
    public float DefaultDamage { get { return battle.atkDamage; } }
    public bool  isUnderTheSea;
    [Space(20f)]

    #region 불
    [Header("불")]
    public GameObject FireFloor;
    [Tooltip("스킬 길이")]
    public int RangeCount;
    [Tooltip("스킬 발동 시 데미지")]
    public float DiffusionDamageIncreaseRate;
    [Tooltip("바닥 감지 길이")]
    public int detectlength;
    public LayerMask layer;
    [Tooltip("발동 시간")]
    public float ActiveTime;
    [Tooltip("스킬 발동 시 진동 시간")]
    public float FireShakeTime;
    [Tooltip("스킬 발동 시 진동 세기")]
    public float FireShakeMagnitude;
    RaycastHit2D hit;
    RaycastHit2D hit1;
    #endregion
    [Space(20f)]

    #region 물
    [Header("물")]
    public GameObject WaterBall;
    public bool isWater = false;
    [Tooltip("1타 데미지 증가율")]
    public float WaterDamageIncreaseRate;
    [Tooltip("2타 데미지 증가율")]
    public float BombDamageIncreaseRate;
    [Tooltip("올라가는 속도")]
    public float FloatingForce;
    [Tooltip("올라가는 시간")]
    public float FloatingTime;
    [Tooltip("차징 시간")]
    public float WaterChargeTime;
    public Vector2[] BallPos;
    public float BallGravity;
    public float BallSpeed;
    public Vector2 BombRange;
    [Tooltip("2타 차징 시간")]
    public float BombChargeTime;
    public float WaterY;
    [Tooltip("폭탄 발동 시간")]
    public float activeTime;
    public float BombShakeTime;
    public float BombShakeMagnitude;
    #endregion
    [Space(20f)]

    #region 땅
    [Header("땅")]
    public bool isSouth = false;
    [Tooltip("점프 시 데미지 증가율")]
    public float JumpDamageIncreaseRate;
    [Tooltip("착지 시 데미지 증가율")]
    public float LandDamageIncreaseRate;
    [Tooltip("점프 세기")]
    public float JumpForce;
    [Tooltip("낙하 세기")]
    public float FallForce;
    [Tooltip("스턴 시간")]
    public float StunTime;
    [Tooltip("점프 시간")]
    public float RisingTime;
    public Vector3[] LandingRange;
    public Transform[] LandingPos;
    [Tooltip("점프 시 진동 시간 ")]
    public float JumpShakeTime;
    [Tooltip("점프 시 진동 강도")]
    public float JumpShakeMagnitude;
    [Tooltip("타격 시 진동 시간")]
    public float SouthShakeTime;
    [Tooltip("타격 시 진동 강도")]
    public float SouthShakeMagnitude;
    [Tooltip("착지 시 진동 시간")]
    public float LandingShakeTime;
    [Tooltip("착지 시 진동 당도 ")]
    public float LandingShakeMagnitude;
    private bool CanLanding;
    #endregion
    [Space(20f)]

    #region 바람
    [Header("바람")]
    public GameObject Arrow;
    public Transform Pos;
    [Tooltip("화살 데미지 증가율 %")]
    public float ArrowDamageIncreaseRate;
    //[Tooltip("���� �� ������ ������ %")]
    //public float DeclindRate;
    [Tooltip("차징 시간")]
    public float ChargeTime;
    [Tooltip("무적 시간")]
    public float InvicibleTime;
    [Tooltip("발사 후 딜레이")]
    public float Delay;
    [Tooltip("발사 시 진동 시간 ")]
    public float WindShakeTime;
    [Tooltip("발사 시 진동 강도")]
    public float WindShakeMagnitude;
    #endregion

    void Awake()
    {
        animator =  GetComponent<Animator>();
        passive = GetComponent<PassiveSystem>();
        battle = GetComponent<Battle>();
        rigid2D = GetComponent<Rigidbody2D>();
        movement2D = GetComponent<Movement2D>();
        status = GetComponent<PlayerStatus>();
        for (int i = 0; i < SkillReady.Length; i++)
        {
            SkillReady[i] = true;
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
                    CameraController.instance.ShakeCamera(SouthShakeTime, SouthShakeMagnitude);

                    SkillAtk(collider.gameObject, DefaultDamage * (1 + (JumpDamageIncreaseRate / 100)));
                }
                if( collider.CompareTag("Destruct"))
                {
                    SkillAtk(collider.gameObject, DefaultDamage * (1 + (JumpDamageIncreaseRate / 100)));
                }
            }
            Invoke("RandingSet", RisingTime);
         
        }
        for (int i = 0; i < RangeCount; i++)
        {
            Debug.DrawRay(new Vector2(transform.position.x + i, transform.position.y), transform.up * -detectlength, Color.red);
            Debug.DrawRay(new Vector2(transform.position.x - i, transform.position.y), Vector3.down * detectlength, Color.red);
        }

    }

    public void TriggerSkill(WeaponTypes weapontype)
    {
        switch (weapontype)
        {
            case WeaponTypes.Sword:
                // CameraController.instance.StartCoroutine(CameraController.instance.Shake(FireShakeTime, FireShakeMagnitude));
                CameraController.instance.ShakeCamera(FireShakeTime, FireShakeMagnitude);
                FIreSkill();
                break;
            case WeaponTypes.Wand:
                WaterY = transform.position.y;
                StartCoroutine(WaterSkill());
                break;
            case WeaponTypes.Shield:
                // CameraController.instance.StartCoroutine(CameraController.instance.Shake(JumpShakeTime, JumpShakeMagnitude));
                CameraController.instance.ShakeCamera(JumpShakeTime, JumpShakeMagnitude);
                StartCoroutine(SouthSkill());
                break;
            case WeaponTypes.Bow:
                animator.SetBool("isCharge", true);
                animator.SetTrigger("Charging");      
                StartCoroutine(WindSkill());
                break;
        }
    }

    #region 불정령
    public void FIreSkill()
    {
        SkillReady[(int)battle.WeaponType] = false;
        //�ٴڿ��� �� ������ ������� 
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
        StartCoroutine(ReturnSkill());
    }
    #endregion

    #region 물 정령
    public IEnumerator WaterSkill()
    {
        float gravity;
        SkillReady[(int)battle.WeaponType] = false;
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
        SkillReady[(int)battle.WeaponType] = true;
        rigid2D.gravityScale = gravity;
        StartCoroutine(ReturnSkill());  
    }
    #endregion

    #region 땅 정령
    public IEnumerator SouthSkill()
    {
        SkillReady[(int)battle.WeaponType] = false;
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
                StartCoroutine(movement2D.DashCoolDown(0.01f));
                StartCoroutine(ReturnSkill());
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
        SkillReady[(int)battle.WeaponType] = false;
        yield return new WaitForSeconds(ChargeTime);
        animator.SetBool("isCharge", false);
        battle.isGuard = true;
        GameObject MagicArrow = Instantiate(Arrow);
        // CameraController.instance.StartCoroutine(CameraController.instance.Shake(WindShakeTime, WindShakeMagnitude));
        CameraController.instance.ShakeCamera(WindShakeTime, WindShakeMagnitude);
        MagicArrow.GetComponent<ProjectileType>().Damage = DefaultDamage * (1 + (ArrowDamageIncreaseRate/100));
       // MagicArrow.GetComponent<ProjectileType>().DeclineRate = DeclindRate;
        MagicArrow.transform.position = Pos.position;
        MagicArrow.transform.localScale = new Vector3(transform.localScale.x, MagicArrow.transform.localScale.y, MagicArrow.transform.localScale.z);
        StartCoroutine(ReturnSkill());
        StartCoroutine(delay());
        yield return new WaitForSeconds(InvicibleTime);
        battle.isGuard = false;
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(Delay);
    }
    #endregion


    public IEnumerator ReturnSkill()
    {
        isCharging = false;
        for (int i = 0; i < SkillReady.Length; i++)
        {
            if (!SkillReady[i])
            {
                yield return new WaitForSeconds(SkillCoolTime[i] *= 1 - (status.coolDownReductionPer/100));
                SkillReady[i] = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(LandingPos[0].position, LandingRange[0]);
        Gizmos.DrawWireCube(LandingPos[1].position, LandingRange[1]);       
    }
    public void SkillAtk(GameObject AtkObj, float Damage)
    {
        //CameraController.instance.StartCoroutine(CameraController.instance.Shake());

        if (AtkObj.CompareTag("Monster"))
        {
            Debug.Log(Damage);
            AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
        }

        if (AtkObj.CompareTag("Destruct"))
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
        }

    }
}
