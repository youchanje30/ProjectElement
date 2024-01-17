using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField]private float DefaultDamage;
    [Header("불")]
    public GameObject FireFloor;


    [Header("물")]
    public GameObject WaterBall;
    public bool isWater = false;
    public float WaterDamageIncreaseRate;
    public float FloatingForce;
    public float FloatingTime;
    public float WaterChargeTime;
    public Vector2[] BallPos;
    public float BallGravity;
    public float BallSpeed;


    [Header("땅")]
    public bool isSouth = false;
    public float JumpDamageIncreaseRate;
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
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[1].position, LandingRange[1], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (JumpDamageIncreaseRate / 100));
                    // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                }
            }
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
    
        for (int i = 0; i < 3; i++)
        {
            GameObject Ball = Instantiate(WaterBall);
            Ball.GetComponent<ProjectileType>().Damage = DefaultDamage *= 1 + (WaterDamageIncreaseRate / 100);
            Ball.GetComponent<Rigidbody2D>().gravityScale = BallGravity;
            Ball.transform.position = transform.position;
            Ball.GetComponent<Rigidbody2D>().velocity = BallPos[i] * BallSpeed;
        }
        battle.isSwap = true;
        isCharging = false;
        isWater = false;
        SkillReady[(int)battle.WeaponType] = true;
        rigid2D.gravityScale = gravity;
    }
    #endregion

    #region 땅 정령 
    public IEnumerator SouthSkill()
    {
        SkillReady[(int)battle.WeaponType] = false;
        battle.isSwap = false;
        isSouth = true;
        battle.isGuard = true;
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, JumpForce);
        yield return new WaitForSeconds(RisingTime);
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, -FallForce);

        StartCoroutine(ReturnAttack());
    }
    public void RandingSet()
    {
        if (movement2D.isGround)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[0].position, LandingRange[0], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" || collider.tag == "Destruct")
                {              
                    StartCoroutine(Stun(collider.gameObject));
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (LandDamageIncreaseRate / 100));
                }
            }
            battle.isSwap = true;
            isSouth = false;
            battle.isGuard = false;
        }
    }
    public IEnumerator Stun(GameObject monster)
    {
       // monster.GetComponentInParent<MonsterBase>().moveSpeed = 0;
        monster.GetComponentInParent<MonsterBase>().isStun = true;
        yield return new WaitForSeconds(StunTime);
        // monster.GetComponentInParent<MonsterBase>().moveSpeed = monster.GetComponentInParent<MonsterBase>().monsterData.maxMoveSpeed;
        monster.GetComponentInParent<MonsterBase>().isStun = false;
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
}
