using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ActiveSkill : MonoBehaviour
{
    private Battle battle;
    private Movement2D movement2D;
    private Rigidbody2D rigid2D;
    public PassiveSystem passive;
    public PlayerStatus status;
    public float[] SkillCoolTime;
    public bool[] SkillReady;
    public bool isCharging = false;
    [Header("��ų ������ = ���� ������ * ������ ������")]
    public float DefaultDamage;
    [Space(20f)]

    #region ��
    [Header("��")]
    public GameObject FireFloor;
    public int RangeCount;
    [Tooltip("Ȯ�� ������ ������")]
    public float DiffusionDamageIncreaseRate;
    [Tooltip("�ٴڰ��� ����")]
    public int detectlength;
    [Tooltip("�����ϴ� ���̾�")]
    public LayerMask layer;
    [Tooltip("�ߵ� �ð�")]
    public float ActiveTime;
    [Tooltip("��ų �ߵ� �� ī�޶� ��鸲 �ð� ")]
    public float FireShakeTime;
    [Tooltip("��ų �ߵ� �� ī�޶� ��鸲 ���� ")]
    public float FireShakeMagnitude;
    RaycastHit2D hit;
    RaycastHit2D hit1;
    #endregion
    [Space(20f)]

    #region ��
    [Header("��")]
    public GameObject WaterBall;
    public bool isWater = false;
    [Tooltip("1Ÿ ������ ������")]
    public float WaterDamageIncreaseRate;
    [Tooltip("2Ÿ ������ ������")]
    public float BombDamageIncreaseRate;
    [Tooltip("���ߺξ�")]
    public float FloatingForce;
    [Tooltip("�ö󰡴� �ð�")]
    public float FloatingTime;
    [Tooltip("��¡ �ð�")]
    public float WaterChargeTime;
    [Tooltip("����ź ���󰡴� ��ġ")]
    public Vector2[] BallPos;
    [Tooltip("����ź �߷�")]
    public float BallGravity;
    [Tooltip("����ź �ӵ�")]
    public float BallSpeed;
    [Tooltip("2Ÿ ����")]
    public Vector2 BombRange;
    [Tooltip("2Ÿ ������ �� �ð�")]
    public float BombChargeTime;
    public float WaterY;
    [Tooltip("�ߵ� �ð�")]
    public float activeTime;
    [Tooltip("���� �� ī�޶� ��鸲 �ð� ")]
    public float BombShakeTime;
    [Tooltip("���� �� ī�޶� ��鸲 ���� ")]
    public float BombShakeMagnitude;
    #endregion
    [Space(20f)]

    #region �� 
    [Header("��")]
    public bool isSouth = false;
    [Tooltip("���� �� ������ ������")]
    public float JumpDamageIncreaseRate;
    [Tooltip("���� �� ������ ������")]
    public float LandDamageIncreaseRate;
    [Tooltip("����")]
    public float JumpForce;
    [Tooltip("����")]
    public float FallForce;
    [Tooltip("���� �ð�")]
    public float StunTime;
    [Tooltip("��� �ð�")]
    public float RisingTime;
    [Tooltip("���� ����")]
    public Vector3[] LandingRange;
    public Transform[] LandingPos;
    [Tooltip("���� �� ī�޶� ��鸲 �ð� ")]
    public float JumpShakeTime;
    [Tooltip("���� �� ī�޶� ��鸲 ���� ")]
    public float JumpShakeMagnitude;
    [Tooltip("���� �� Ÿ�� �� ī�޶� ��鸲 �ð� ")]
    public float SouthShakeTime;
    [Tooltip("���� �� Ÿ�� �� ī�޶� ��鸲 ���� ")]
    public float SouthShakeMagnitude;
    [Tooltip("���� �� ī�޶� ��鸲 �ð� ")]
    public float LandingShakeTime;
    [Tooltip("���� �� ī�޶� ��鸲 ���� ")]
    public float LandingShakeMagnitude;
    private bool CanLanding;
    #endregion
    [Space(20f)]

    #region �ٶ� 
    [Header("�ٶ�")]
    public GameObject Arrow;
    public Transform Pos;
    [Tooltip("��ų ������ ������ %")]
    public float ArrowDamageIncreaseRate;
    //[Tooltip("���� �� ������ ������ %")]
    //public float DeclindRate;
    [Tooltip("��¡ �ð�")]
    public float ChargeTime;
    [Tooltip("�߻� �� ���� �ð�")]
    public float InvicibleTime;
    [Tooltip("�߻� �� �ݵ�")]
    public float Delay;
    [Tooltip("�߻� �� ī�޶� ��鸲 �ð� ")]
    public float WindShakeTime;
    [Tooltip("�߻� �� ī�޶� ��鸲 ���� ")]
    public float WindShakeMagnitude;
    #endregion

    void Awake()
    {
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
                if (collider.tag == "Monster" && collider.GetComponentInParent<MonsterBase>().isHit == false)
                {
                    collider.GetComponentInParent<MonsterBase>().isHit = true;
                    // StartCoroutine(Hit(collider.gameObject, 0.5f));
                    CameraController.instance.StartCoroutine(CameraController.instance.Shake(SouthShakeTime, SouthShakeMagnitude));
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (JumpDamageIncreaseRate / 100));
                }
                if( collider.tag == "Destruct")
                {
                    SkillAtk(collider.gameObject, DefaultDamage *= 1 + (JumpDamageIncreaseRate / 100));
                }
                DefaultDamage = battle.atkDamage;
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
        DefaultDamage = battle.atkDamage;
        switch (weapontype)
        {
            case WeaponTypes.Sword:
                CameraController.instance.StartCoroutine(CameraController.instance.ShakeR(FireShakeTime, FireShakeMagnitude));
                FIreSkill();
                break;
            case WeaponTypes.Wand:
                WaterY = transform.position.y;
                StartCoroutine(WaterSkill());
                break;
            case WeaponTypes.Shield:
                CameraController.instance.StartCoroutine(CameraController.instance.Shake(JumpShakeTime, JumpShakeMagnitude));
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
        SkillReady[(int)battle.WeaponType] = false;
        //�ٴڿ��� �� ������ ������� 
        for (int i = 0; i < RangeCount; i++)
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x + i, transform.position.y), transform.up * -20, detectlength, layer);
            if (hit.collider != null)
            {
                GameObject Fire = Instantiate(FireFloor);
                Fire.transform.position = new Vector2(transform.position.x + i, transform.position.y - hit.distance);
                Fire.GetComponent<ProjectileType>().Damage = DefaultDamage *= 1 + (DiffusionDamageIncreaseRate / 100);
            }
            hit1 = Physics2D.Raycast(new Vector2(transform.position.x - i, transform.position.y), transform.up * -20, detectlength, layer);
            if (hit1.collider != null)
            {
                GameObject Fire = Instantiate(FireFloor);
                Fire.transform.position = new Vector2(transform.position.x - i, transform.position.y - hit1.distance);
                Fire.GetComponent<ProjectileType>().Damage = DefaultDamage *= 1 + (DiffusionDamageIncreaseRate / 100);
            }
        }
        StartCoroutine(ReturnSkill());
    }
    #endregion

    #region �� ����
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
        StartCoroutine(ReturnSkill());  
    }
    #endregion

    #region �� ���� 
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

        StartCoroutine(ReturnSkill());
    }
    public void RandingSet()
    {
        if (movement2D.isGround && CanLanding)
        { 
            CameraController.instance.StartCoroutine(CameraController.instance.ShakeR(LandingShakeTime, LandingShakeMagnitude));
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(LandingPos[0].position, LandingRange[0], 0);
            
            foreach (Collider2D collider in collider2Ds)
            {               
                if (collider.tag == "Monster" && collider.GetComponentInParent<MonsterBase>().isHit == false)
                {
                    //StartCoroutine(Hit(collider.gameObject, 0.5f));
                    collider.GetComponentInParent<MonsterBase>().isHit = true;
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
    //public IEnumerator Hit(GameObject monster, float time)
    //{
    //    // monster.GetComponentInParent<MonsterBase>().moveSpeed = 0;
    //    monster.GetComponentInParent<MonsterBase>().isHit = true;
    //    yield return new WaitForSeconds(time);
    //    // monster.GetComponentInParent<MonsterBase>().moveSpeed = monster.GetComponentInParent<MonsterBase>().monsterData.maxMoveSpeed;
    //    monster.GetComponentInParent<MonsterBase>().isHit = false;
    //}
    #endregion

    #region �ٶ� ���� 
    public IEnumerator WindSkill()
    {
        isCharging = true;
        SkillReady[(int)battle.WeaponType] = false;
        yield return new WaitForSeconds(ChargeTime);
        battle.isGuard = true;
        GameObject MagicArrow = Instantiate(Arrow);
        CameraController.instance.StartCoroutine(CameraController.instance.Shake(WindShakeTime, WindShakeMagnitude));
        MagicArrow.GetComponent<ProjectileType>().Damage = DefaultDamage *= 1 + (ArrowDamageIncreaseRate/100);
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
