using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Goat : MonsterBase
{
    
    [Header("Goat Jump Status")]
    float jumpVec;
    
    enum GoatAtkType { rush, jump };

    [SerializeField] GoatAtkType curAtkType;
    [SerializeField] int[] atkTypeRate;

    
    public float jumpY;
    public float jumpForce;

    [SerializeField] bool isJump;
    [SerializeField] bool isRush;
    [SerializeField] bool canHit;
    [SerializeField] DetectPlayer atkBox;

    [SerializeField] float rushSpeed;

    [SerializeField] Vector2 groundSize;
    [SerializeField] Vector2 groundOffset;
    [SerializeField] float maxAbsJumpX;

    
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
        if(!IsOnGround())
            animator.SetBool("isGround", false);

        isTracking = false;
        isMove = false;
        
        if(rigid.velocity.y < 0 && IsOnGround() && isJump)
        {
            animator.SetBool("isGround", true);
            canAtk = false;
            isAtking = false;
            isJump = false;
            AtkEnd();
        }
        
        if(isDead || isAtking || isKnockback || isStun)
        {
            canAtk = false;
            canMove = false;
            canTrack = false;
            return;
        }

        canAtk = false;
        canTrack = false;
        canMove = false;

        if(curAtkCoolTime <= 0)
        {
            canAtk = true;
            return;
        }
        
        if(trackDetect.isEnter)
            canTrack = true;
        else
            canMove = true;
    }

    protected override void SetState()
    {
        isTracking = false;
        isMove = false;

        if(isDead || isAtking || isKnockback || isStun)
        {
        
            canMove = false;
            canAtk = false;
            return;
        }

        if(canAtk)
        {
            Atk();
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

    protected override bool IsOnGround()
    {
        Debug.DrawRay((Vector2)transform.position, Vector2.down, new Color(0,1,0));
        bool check = Physics2D.OverlapBox((Vector2)transform.position + groundOffset, groundSize, 0f, LayerMask.GetMask("Platform"));
        return check;
    }

    protected override void Atk()
    {
        isAtking = true;
        canAtk = false;

        int randomSum = 0;
        for (int i = 0; i < atkTypeRate.Length; i++)
            randomSum += atkTypeRate[i];

        int randomAct = UnityEngine.Random.Range(0, randomSum + 1);

        if(randomAct <= atkTypeRate[0])
        {
            isJump = true;
            canHit = true;
            animator.SetTrigger("JumpAtk");
            JumpAtk();
        }
        else
        {
            animator.SetTrigger("RushAtk");
        }

    }

    void RushAtk()
    {
        isRush = true;
        canHit = true;
        rigid.velocity = new Vector2(-transform.localScale.x, 0).normalized * rushSpeed;
    }

    void JumpAtk()
    {
        
        int targetDir = target.position.x > transform.position.x ? 1 : -1;
        ChangeLocalScale(targetDir);

        float jumpTargetX = (target.transform.position.x - transform.position.x) / 2f;
        float fixedDistanceX = Mathf.Clamp(jumpTargetX, -maxAbsJumpX , maxAbsJumpX);

        Vector2 inflectionPoint = new Vector2(fixedDistanceX, jumpY);

        float v_y = Mathf.Sqrt(2 * rigid.gravityScale * -Physics2D.gravity.y * inflectionPoint.y);
        float v_x = inflectionPoint.x * v_y / (2 * inflectionPoint.y);

        Vector2 force = rigid.mass * (new Vector2(v_x, v_y) - rigid.velocity);
        rigid.AddForce(force, ForceMode2D.Impulse);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(canHit && other.CompareTag("Player") && atkBox.isEnter)
        {
            canHit = false;
            other.GetComponent<Battle>().GetDamaged(damage);
        }
    }

    protected override void AtkEnd()
    {
        base.AtkEnd();
        isJump = false;
        isRush = false;
        canHit = false;
        SetAtkType();
        rigid.velocity = Vector2.zero;
    }

    void SetAtkType()
    {
        int rateSum = 0;
        for (int i = 0; i < atkTypeRate.Length; i++)
            rateSum += atkTypeRate[i];
        
        int targetNum = UnityEngine.Random.Range(0, rateSum + 1);

        if(targetNum <= atkTypeRate[0])
            curAtkType = GoatAtkType.rush;
        else
            curAtkType = GoatAtkType.jump;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawCube((Vector2)transform.position + groundOffset, groundSize);
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