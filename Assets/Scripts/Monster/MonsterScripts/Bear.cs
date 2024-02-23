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

    [Space(20f)]
    [SerializeField] BoxCollider2D rushCol;
    [SerializeField] BoxCollider2D normalCol;

    [SerializeField] bool isSpawn;
    [SerializeField] Slider hpBar;
    [SerializeField] float hpDecreaTime;

    protected void Shake()
    {
        CameraController.instance.Shake(0.4f, 30);
        
    }

    protected void Spawn()
    {
        isSpawn = true;
        UIController.instance.SetBossUI();
    }

    protected override void Start()
    {
        base.Start();
        hpBar = UIController.instance.bossHpSlider;
        hpBar.maxValue = monsterData.maxHp;
        hpBar.value = curHp;
    }

    protected override void Awake()
    {
        base.Awake();
        isSpawn = false;
    }

    protected override void Update()
    {
        if(!isSpawn) return;

        CheckState();
        SetState();

        if(isMove)
        {
            Move();
        }
        else
        {
            rushCol.enabled = false;
            normalCol.enabled = true;
        }
            
        if(isTracking)
            Tracking();

        TimeProcess();
        if(isRushAtk && atkCheck.isEnter)
        {
            isRushAtk = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Battle>().GetDamaged(damage);
        }
    }

    protected override void Move()
    {
        base.Move();
        
        rushCol.enabled = (nextDir != 0);
        normalCol.enabled = (nextDir == 0);
    }

    protected override void Tracking()
    {
        base.Tracking();
        rushCol.enabled = true;
        normalCol.enabled = false;
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

        int randomAct = Random.Range(1, 100 + 1);
        if(randomAct < 50)
            animator.SetTrigger("HandAtk");
        else if(randomAct < 80)
            animator.SetTrigger("DownAtk");
        else
            animator.SetTrigger("RushAtk");
    }

    protected override void AtkEnd()
    {
        base.AtkEnd();
        // rushCol.enabled = false;
        isRushAtk = false;
    }



    #region 차징
    protected IEnumerator Charging(AtkTypes atkType)
    {
        ChargingAct(atkType);
        float curChargingTime = 0;
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

                break;

            case AtkTypes.DownAtk:
                rigid.velocity = Vector2.up * 10;
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
    #endregion

    void SetUI()
    {
        DOTween.Kill(hpBar);
        DOTween.To(() => hpBar.value, x => hpBar.value = x, curHp, hpDecreaTime).SetEase(Ease.OutQuart);
    }
}
