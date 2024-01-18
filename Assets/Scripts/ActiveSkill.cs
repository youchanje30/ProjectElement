using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    private Battle battle;
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    private PassiveSystem passive;
    public float[] SkillCoolTime;
    public bool[] SkillReady;
    public bool isCharging = false;
    [Header("스킬 데미지 = 최종 데미지 * 데미지 증가율")]
    public float DefaultDamage;
    #region 불
    [Header("불")]
    public GameObject FireFloor;
    #endregion

    #region 물
    [Header("물")]
    public GameObject WaterBall;
    public bool isWater = false;
    [Tooltip("1타 데미지 증가율")]
    public float WaterDamageIncreaseRate;
    [Tooltip("2타 데미지 증가율")]
    public float BombDamageIncreaseRate;
    [Tooltip("공중부양")]
    public float FloatingForce;
    [Tooltip("올라가는 시간")]
    public float FloatingTime;
    [Tooltip("차징 시간")]
    public float WaterChargeTime;
    [Tooltip("물폭탄 날라가는 위치")]
    public Vector2[] BallPos;
    [Tooltip("물폭탄 중력")]
    public float BallGravity;
    [Tooltip("물폭탄 속도")]
    public float BallSpeed;
    [Tooltip("2타 범위")]
    public Vector2 BombRange;
    [Tooltip("2타 터지기 전 시간")]
    public float BombChargeTime;
    #endregion

    #region 땅 
    [Header("땅")]
    public bool isSouth = false;
    [Tooltip("점프 중 데미지 증가율")]
    public float JumpDamageIncreaseRate;
    [Tooltip("착지 시 데미지 증가율")]
    public float LandDamageIncreaseRate;
    [Tooltip("점프")]
    public float JumpForce;
    [Tooltip("낙하")]
    public float FallForce;
    [Tooltip("스턴 시간")]
    public float StunTime;
    [Tooltip("상승 시간")]
    public float RisingTime;
    [Tooltip("착지 범위")]
    public Vector3[] LandingRange;
    public Transform[] LandingPos;
    private int count;
    private bool CanLanding;
    #endregion

    #region 바람 
    [Header("바람")]
    public GameObject Arrow;
    public Transform Pos;
    [Tooltip("스킬 데미지 증가율 %")]
    public float ArrowDamageIncreaseRate;
    [Tooltip("관통 후 데미지 감소율 %")]
    public float DeclindRate;
    [Tooltip("차징 시간")]
    public float ChargeTime;
    [Tooltip("발사 시 무적 시간")]
    public float InvicibleTime;
    [Tooltip("발사 후 반동")]
    public float Delay;
    #endregion

    void Awake()
    {
        passive = GetComponent<PassiveSystem>();
        battle = GetComponent<Battle>();
        rigid2D = GetComponent<Rigidbody2D>();
        movement2D = GetComponent<Movement2D>();
        for (int i = 0; i < SkillReady.Length; i++)
        {
            SkillReady[i] = true;
        }
    }
    void Update()
    {
        DefaultDamage = battle.atkDamage;
        if (isSouth)
        {
            movement2D.curDashCnt = -1;
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[1].position, LandingRange[1], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" && collider.GetComponentInParent<MonsterBase>().isHit == false)
                {
                    StartCoroutine(Hit(collider.gameObject, 0.5f));
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (JumpDamageIncreaseRate / 100));
                }
                if( collider.tag == "Destruct")
                {
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (JumpDamageIncreaseRate / 100));
                }
                DefaultDamage = battle.atkDamage;
            }
            count = 0;
            Invoke("RandingSet", RisingTime);
        }
    }

    public void TriggerSkill(WeaponTypes weapontype)
    {
        switch (weapontype)
        {
            case WeaponTypes.Sword:
                FIreSkill();
                break;
            case WeaponTypes.Wand:
                StartCoroutine(WaterSkill());
                break;
            case WeaponTypes.Shield:
                StartCoroutine(SouthSkill());
                break;
            case WeaponTypes.Bow:
                StartCoroutine(WindSkill());
                break;
        }
    }
    #region 불 정령
    public void FIreSkill()
    {
        //바닥에만 불 장판이 깔려야함 
        Instantiate(FireFloor);

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
            ball.GetComponent<ProjectileType>().Damage = DefaultDamage *= 1 + (WaterDamageIncreaseRate / 100);
            ball.GetComponent<Rigidbody2D>().gravityScale = BallGravity;
            ball.transform.position = transform.position;
            ball.GetComponent<Rigidbody2D>().velocity = BallPos[i] * BallSpeed;                        
        }
        battle.isSwap = true;
        isCharging = false;
        isWater = false;
        SkillReady[(int)battle.WeaponType] = true;
        rigid2D.gravityScale = gravity;
        StartCoroutine(ReturnAttack());  
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

        StartCoroutine(ReturnAttack());
    }
    public void RandingSet()
    {
        if (movement2D.isGround && CanLanding)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[0].position, LandingRange[0], 0);
            foreach (Collider2D collider in collider2Ds)
            {               
                if (collider.tag == "Monster" && collider.GetComponentInParent<MonsterBase>().isHit == false)
                {
                    StartCoroutine(Hit(collider.gameObject, 0.5f));
                    StartCoroutine(Stun(collider.gameObject, StunTime));
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (LandDamageIncreaseRate / 100));
                }
                if (collider.tag == "Destruct")
                {
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (LandDamageIncreaseRate / 100));
                }
                DefaultDamage = battle.atkDamage;
               
            }
            CanLanding = false;
            battle.isSwap = true;
            isSouth = false;
            battle.isGuard = false;
            StartCoroutine(movement2D.DashCoolDown(0.01f));
           
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
    public IEnumerator Hit(GameObject monster, float time)
    {
        // monster.GetComponentInParent<MonsterBase>().moveSpeed = 0;
        monster.GetComponentInParent<MonsterBase>().isHit = true;
        yield return new WaitForSeconds(time);
        // monster.GetComponentInParent<MonsterBase>().moveSpeed = monster.GetComponentInParent<MonsterBase>().monsterData.maxMoveSpeed;
        monster.GetComponentInParent<MonsterBase>().isHit = false;
    }
    #endregion

    #region 바람 정령 
    public IEnumerator WindSkill()
    {
        isCharging = true;
        SkillReady[(int)battle.WeaponType] = false;
        yield return new WaitForSeconds(ChargeTime);
        battle.isGuard = true;
        GameObject MagicArrow = Instantiate(Arrow);
        MagicArrow.GetComponent<ProjectileType>().Damage = DefaultDamage *= 1 + (ArrowDamageIncreaseRate/100);
        MagicArrow.GetComponent<ProjectileType>().DeclineRate = DeclindRate;
        MagicArrow.transform.position = Pos.position;
        MagicArrow.transform.localScale = new Vector3(transform.localScale.x, MagicArrow.transform.localScale.y, MagicArrow.transform.localScale.z);
        StartCoroutine(ReturnAttack());
        StartCoroutine(delay());
        yield return new WaitForSeconds(InvicibleTime);
        battle.isGuard = false;
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(Delay);
    }
    #endregion




    public IEnumerator ReturnAttack()
    {

        isCharging = false;
        for (int i = 0; i < SkillReady.Length; i++)
        {
            if (!SkillReady[i])
            {
                yield return new WaitForSeconds(SkillCoolTime[i]);
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
        CameraController.instance.StartCoroutine(CameraController.instance.Shake());

        if (AtkObj.tag == "Monster")
        {
            Debug.Log(Damage);
            AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
        }

        if (AtkObj.tag == "Destruct")
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
        }

    }
    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (isSouth)
    //    {
    //        if (collider.tag == "Monster" || collider.tag == "Destruct")
    //        {
    //            SkillAtk(collider.gameObject, DefaultDamage *= 1 + (JumpDamageIncreaseRate / 100));
    //        }
    //    }
    //}
}
