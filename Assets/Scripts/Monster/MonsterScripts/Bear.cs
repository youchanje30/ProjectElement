using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bear : MonsterBase
{
    public enum AtkTypes { HandAtk = 0, DownAtk = 1, Rush = 2, };
    
    // [SerializeField] BoxCollider2D rushCol;
    [SerializeField] bool isRushAtk = false;

    [SerializeField] float[] atkChargingTime;
    [SerializeField] DetectPlayer atkCheck;
    [SerializeField] int[] atkRate;

    [Space(20f)]
    [SerializeField] BoxCollider2D rushCol;
    [SerializeField] BoxCollider2D normalCol;


    [Header("점프 찍기 공격 관련")]
    [Header("상승 힘")] [SerializeField] float upForce;
    [Header("상승 시간")] [SerializeField] float upTime;
    [Header("낙하 힘")] [SerializeField] float downForce;


    [Header("소환 및 UI 관련")]
    [SerializeField] bool isSpawn;
    [SerializeField] Slider hpBar;
    [SerializeField] float hpDecreaTime;
    

    [Header("이펙트 관련 설정")]
    [SerializeField] Transform chargeEffect;
    [SerializeField] Transform kuangEffect;
    [SerializeField] Transform downEffect;
    [SerializeField] GameObject redeyeEffect;
    bool isRedeye;

    protected void Shake()
    {
        CameraController.instance.ShakeCamera(0.4f, 20);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Appear);
        
    }

    protected void Spawn()
    {
        isSpawn = true;
        UIController.instance.SetBossUI();
    }

    protected override void Start()
    {
        base.Start();
        isRedeye = false;
        hpBar = UIController.instance.bossHpSlider;
        hpBar.maxValue = monsterData.maxHp;
        hpBar.value = curHp;
        rushCol.enabled = true;
        normalCol.enabled = false;
    }

    protected override void Awake()
    {
        base.Awake();
        isSpawn = false;
    }

    void Effect(int index = 0)
    {
        Vector3 detectPos = transform.position + atkInfo[index].atkPos * (Mathf.Abs(transform.localScale.x) / transform.localScale.x);
        switch (index)
        {
            
            case 0: // HankAtk
                EffectManager.instance.SpawnEffect(detectPos, (int)BossEffect.handDown, atkInfo[index].atkSize, transform.localScale.x < 0);
                break;
            
            case 1: // Down
                // EffectManager.instance.SpawnEffect(detectPos, (int)BossEffect.fallDown, atkInfo[index].atkSize);
                break;
            default:
                break;
        }
    }

    protected override void Update()
    {
        if(!isSpawn) return;

        TimeProcess();
        CheckState();
        SetState();

        if(isMove)
        {
            Move();
        }
        // else
        // {
        //     rushCol.enabled = false;
        //     normalCol.enabled = true;
        // }
            
        if(isTracking)
            Tracking();

        if(isRushAtk && atkCheck.isEnter)
        {
            isRushAtk = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Battle>().GetDamaged(damage);
        }
    }

    protected override void TimeProcess()
    {
        base.TimeProcess();
        if(!isRedeye && curAtkCoolTime <= 0.5f)
        {
            isRedeye = true;
            redeyeEffect.SetActive(true);
        }
    }

    protected override void Move()
    {
        base.Move();
        
        // rushCol.enabled = (nextDir != 0);
        // normalCol.enabled = (nextDir == 0);
    }

    protected override void Tracking()
    {
        base.Tracking();
        // rushCol.enabled = true;
        // normalCol.enabled = false;
    }

    public override void GetDamaged(float getDamage, bool canKncokBack = true)
    {
        if(isKnockback || isDead || !isSpawn) return;

        curHp -= getDamage;
        SetUI();

        if(curHp <= 0)
        {
            isDead = true;
            animator.SetTrigger("Dead");
            Invoke("Dead", 1.8f);
        }
    }

    protected override void Atk()
    {
        isAtking = true;
        canAtk = false;
        redeyeEffect.SetActive(false);
        // EffectManager.instance.SpawnEffect(redeyeEffect.position, (int)BossEffect.redeye, Vector2.zero);

        int randomSum = 0;
        for (int i = 0; i < atkRate.Length; i++)
            randomSum += atkRate[i];
            
        int randomAct = Random.Range(1, randomSum + 1);
        if(randomAct <= atkRate[0])
        {   
            rushCol.enabled = false;
            normalCol.enabled = true;
            animator.SetTrigger("HandAtk");
        }
        else if(randomAct <= atkRate[1] + atkRate[0])
        {
            animator.SetTrigger("DownAtk");
            EffectManager.instance.SpawnEffect(transform.position, (int)BossEffect.fallDownBack, Vector2.zero);
            EffectManager.instance.SpawnEffect(transform.position, (int)BossEffect.fallDownFront, Vector2.zero);
            
            rushCol.enabled = false;
            normalCol.enabled = true;
        }
        else
        {
            animator.SetTrigger("RushAtk");
            rushCol.enabled = true;
            normalCol.enabled = false;
        }
            
    }

    protected override void AtkEnd()
    {
        base.AtkEnd();
        isRedeye = false;
        isRushAtk = false;
        rushCol.enabled = true;
        normalCol.enabled = false;
    }



    #region 차징
    protected IEnumerator Charging(AtkTypes atkType)
    {
        ChargingAct(atkType);
        float curChargingTime = 0;
        while (curChargingTime <= upTime)
        {
            curChargingTime += Time.deltaTime;
            yield return null;
        }
        FallDown();
        while (curChargingTime <= atkChargingTime[(int)atkType])
        {
            curChargingTime += Time.deltaTime;
            yield return null;
        }
        animator.SetTrigger("ChargingFin");
    }

    protected void ChargingAct(AtkTypes atkType)
    {
        switch (atkType)
        {
            case AtkTypes.HandAtk:
                EffectManager.instance.SpawnEffect(chargeEffect.position, (int)BossEffect.charge, Vector2.zero);
                break;

            case AtkTypes.DownAtk:
                rigid.velocity = Vector2.up * upForce;
                break;

            case AtkTypes.Rush:
                // rushCol.enabled = true;
                rigid.velocity = new Vector2(-transform.localScale.x, 0).normalized * 10;
                isRushAtk = true;
                break;

            default:
                Debug.Log("Called ChargingAct : default");
                break;
        }
    }

    protected void FallDown()
    {
        rigid.velocity = Vector2.down * downForce;
    }
    #endregion

    void SetUI()
    {
        DOTween.Kill(hpBar);
        DOTween.To(() => hpBar.value, x => hpBar.value = x, curHp, hpDecreaTime).SetEase(Ease.OutQuart);
    }
}
