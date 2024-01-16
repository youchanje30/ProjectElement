using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    private Battle battle;
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    public float[] SkillCoolTime;
    public bool[] SkillReady;
    [Header("스킬 데미지 = 최종 데미지 * 데미지 증가율")]
    [SerializeField]private float SkillDamage;
    public float[] DamageIncreaseRate;
    [Header("불")]
    public GameObject FireFloor;

    [Header("땅")]
    public bool isSouth = false;
    [Tooltip("점프")]
    public float JumpForce;
    [Tooltip("낙하")]
    public float FallForce;
    [Tooltip("상승 시간")]
    public float RisingTime;
    [Tooltip("착지 범위")]
    public Vector3[] RandingRange;
    public Transform[] RandingPos;


    [Header("바람")]
    public GameObject Arrow;
    public Transform Pos;
    public bool isCharging = false;
    [Tooltip("차징 시간")]
    public float ChargeTime;
    [Tooltip("발사 시 무적 시간")]
    public float InvicibleTime;
    [Tooltip("발사 후 반동")]
    public float Delay;
    void Awake()
    {
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
        SkillDamage = battle.atkDamage * DamageIncreaseRate[(int)battle.WeaponType];
        if (isSouth)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(RandingPos[1].position, RandingRange[1], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    SkillAtk(collider.gameObject);
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
                WaterSkill();
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
    public void WaterSkill()
    {

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
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(RandingPos[0].position, RandingRange[0], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" || collider.tag == "Destruct")
                {
                    SkillAtk(collider.gameObject);
                    // collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                }
            }
            battle.isSwap = true;
            isSouth = false;
            battle.isGuard = false;
        }
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
        MagicArrow.GetComponent<ProjectileType>().Damage = SkillDamage;
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
        Gizmos.DrawWireCube(RandingPos[0].position, RandingRange[0]);
        Gizmos.DrawWireCube(RandingPos[1].position, RandingRange[1]);
    }
    public void SkillAtk(GameObject AtkObj)
    {
        CameraController.instance.StartCoroutine(CameraController.instance.Shake());

        if (AtkObj.tag == "Monster")
        {
            // bool isCrt = Random.Range(1, 100 + 1) <= crtRate;
            // float NormalDmg = damage * damagePer * 0.01f;
            // float FinalDmg = isCrt ? NormalDmg * crtDmg * 0.01f : NormalDmg;

            // AtkObj.GetComponent<Monster>().GetDamaged(atkDamage);
            Debug.Log(SkillDamage);
            AtkObj.GetComponentInParent<MonsterBase>().GetDamaged(SkillDamage);
            // AtkObj.GetComponent<Monster>().GetDamaged(meleeDmg);
            // if(WeaponType == WeaponTypes.Wand)
            //     PlayerPasstive(AtkObj);
            // else
            //     PlayerPasstive();

            //PlayerPasstive(AtkObj);
        }

        if (AtkObj.tag == "Destruct")
        {
            AtkObj.GetComponent<DestructObject>().DestroyObj();
        }

    }
}
