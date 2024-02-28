using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Wildcat : MonsterBase
{
    enum WildcatAtkType { normal, tele };
    

    [SerializeField] WildcatAtkType curAtkType;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int[] atkTypeRate;


    [Header("텔레포트 공격 관련")]
    [SerializeField] bool canAct;
    [SerializeField] float groundCheckRayDistance;
    [SerializeField] float wallCheckRayDistance;
    [SerializeField] Vector3 telePos;
    [SerializeField] int dir;
    [SerializeField] float upY;
    // [Space(20f)]

    [Header("체력 UI 관련")]
    [SerializeField] Slider hpBar;
    [SerializeField] float hpDecreaTime;


    protected override void Start()
    {
        base.Start();
        UIController.instance.SetBossUI();
        hpBar = UIController.instance.bossHpSlider;
        hpBar.maxValue = monsterData.maxHp;
        hpBar.value = curHp;
    }

    protected override void Init()
    {
        base.Init();
        SetAtkType();
    }

    protected override void CheckState()
    {
        base.CheckState();
        
        if(isAtking && isStun)
            AtkEnd();

        if(isDead || isAtking || isKnockback || isStun)
            return;

        if(trackDetect.isEnter && !canAct && curAtkType == WildcatAtkType.tele && curAtkCoolTime <= 0)
            canAct = true;
    }

    void SetAtkType()
    {
        int rateSum = 0;
        for (int i = 0; i < atkTypeRate.Length; i++)
            rateSum += atkTypeRate[i];
        
        int targetNum = Random.Range(0, rateSum + 1);

        if(targetNum <= atkTypeRate[0])
            curAtkType = WildcatAtkType.normal;
        else
            curAtkType = WildcatAtkType.tele;
    }

    protected override void SetState()
    {
        isTracking = false;
        isMove = false;

        if(isDead || isAtking || isKnockback || isStun)
        {
            // animator.SetBool("isStun", isStun);
            canMove = false;
            canAtk = false;
            return;
        }

        if(monsterSynergy.isBind)
        {
            canMove = false;
            return;
        }

        if(canAtk && curAtkType == WildcatAtkType.normal)
        {
            rigid.velocity = Vector2.zero;
            isAtking = true;
            Atk();
            return;
        } 

        if(curAtkType == WildcatAtkType.tele && trackDetect.isEnter && canAct)
        {
            canAct = false;
            TeleportEnter();
            return;
        }

        if(canTrack)
        {
            isTracking = true;
            return;
        }

        if(canMove)
        {
            isMove = true;
            return;
        }
    }


    void TeleportEnter()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)target.position, Vector2.down, groundCheckRayDistance, groundLayer);

        if(!hit) return;

        isAtking = true;
        canAct = false;
        rigid.velocity = Vector2.zero;

        float spawnY = hit.point.y;
        dir = Random.Range(0, 1 + 1) == 0 ? -1 : 1;

        RaycastHit2D wallCheck = Physics2D.Raycast((Vector2)target.position, Vector2.right * dir, wallCheckRayDistance, groundLayer);
        int time = 1;
        float spawnX = target.position.x;

        for(; wallCheck ? time < (int)wallCheck.distance * 4 : time < 16; time++)
        {
            RaycastHit2D here = Physics2D.Raycast(new Vector2(target.position.x + dir * time * 0.25f, target.position.y), Vector2.down, groundCheckRayDistance, groundLayer);
            if(!here || Mathf.Abs(spawnY - here.point.y) > 0.1f)
            {
                time --;
                break;
            }
            spawnY = here.point.y;
        }
        spawnX = target.position.x + dir * time * 0.25f;

        if(wallCheck && dir < 0 && wallCheck.point.x + 1.6f >= spawnX)
                spawnX = wallCheck.point.x + 1.6f;
                
        if(wallCheck && dir > 0 && wallCheck.point.x - 1.6f <= spawnX)
            spawnX = wallCheck.point.x - 1.6f;

        // transform.localScale = new Vector3(dir * monsterData.imageScale, monsterData.imageScale, 1);

        telePos = new Vector3(spawnX, spawnY + upY, transform.position.z);
        animator.SetTrigger("Teleport");
    }

    protected void Teleport()
    {
        transform.position = telePos;
        dir = (target.transform.position.x > telePos.x) ? -1 : 1;
        transform.localScale = new Vector3(dir * monsterData.imageScale, monsterData.imageScale, 1);
        rigid.velocity = Vector2.zero;
    }

    protected override void AtkEnd()
    {
        base.AtkEnd();
        SetAtkType();
    }

    public override void GetDamaged(float getDamage, bool canKncokBack = true)
    {
        if(isDead) return;

        curHp -= getDamage;
        SetUI();

        if(curHp <= 0)
        {
            isDead = true;
            isStun = false;
            animator.SetBool("isStun", false);
            animator.SetTrigger("Dead");
            Invoke("Dead", 1.8f);
        }
    }

    void SetUI()
    {
        DOTween.Kill(hpBar);
        DOTween.To(() => hpBar.value, x => hpBar.value = x, curHp, hpDecreaTime).SetEase(Ease.OutQuart);
    }
}
