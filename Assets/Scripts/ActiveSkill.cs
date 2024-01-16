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
    [Header("��ų ������ = ���� ������ * ������ ������")]
    [SerializeField]private float SkillDamage;
    public float[] DamageIncreaseRate;
    [Header("��")]
    public GameObject FireFloor;

    [Header("��")]
    public bool isSouth = false;
    [Tooltip("����")]
    public float JumpForce;
    [Tooltip("����")]
    public float FallForce;
    [Tooltip("��� �ð�")]
    public float RisingTime;
    [Tooltip("���� ����")]
    public Vector3[] RandingRange;
    public Transform[] RandingPos;


    [Header("�ٶ�")]
    public GameObject Arrow;
    public Transform Pos;
    public bool isCharging = false;
    [Tooltip("��¡ �ð�")]
    public float ChargeTime;
    [Tooltip("�߻� �� ���� �ð�")]
    public float InvicibleTime;
    [Tooltip("�߻� �� �ݵ�")]
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
    #region �� ����
    public void FIreSkill()
    {
        //�ٴڿ��� �� ������ ������� 
        Instantiate(FireFloor);

    }
    #endregion

    #region �� ����
    public void WaterSkill()
    {

    }
    #endregion

    #region �� ���� 
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

    #region �ٶ� ���� 
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
