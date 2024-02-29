using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class Wolf : MonsterBase
{
    enum WolfAtkType { normal, tele };

    [Space(20f)]
    [SerializeField] WolfAtkType curAtkType;
    [SerializeField] int[] atkTypeRate;

    [Space(20f)]
    [SerializeField] bool canAct;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float raycastDistance = 5f;
    [SerializeField] float wallcastDistance = 4;
    [Space(20f)]

    [Header("텔레포트 공격 관련")]
    [SerializeField] Vector3 telePos;
    //int dir { get { return (target.localScale.x > 0) ? -1 : 1; } }
    [SerializeField] int dir;

    [Header("이펙트 위치 관련")]
    [SerializeField] Transform handEffectTrans;
    [SerializeField] Transform biteEffectTrans;
    [SerializeField] Transform teleportEffectTrans;



    protected override void Init()
    {
        base.Init();
        SetAtkType();
    }

    void SetAtkType()
    {
        int rateSum = 0;
        for (int i = 0; i < atkTypeRate.Length; i++)
            rateSum += atkTypeRate[i];
        
        int targetNum = Random.Range(0, rateSum + 1);

        if(targetNum <= atkTypeRate[0])
            curAtkType = WolfAtkType.normal;
        else
            curAtkType = WolfAtkType.tele;
    }

    protected override void CheckState()
    {
        base.CheckState();

        if(isDead || isAtking || isKnockback || isStun)
            return;

        if(trackDetect.isEnter && !canAct && curAtkType == WolfAtkType.tele && curAtkCoolTime <= 0)
            canAct = true;
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

        if(canAtk && curAtkType == WolfAtkType.normal)
        {
            rigid.velocity = Vector2.zero;
            isAtking = true;
            Atk();
            return;
        } 

        if(curAtkType == WolfAtkType.tele && trackDetect.isEnter && canAct)
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

    protected override void RandomDir()
    {
        nextDir = Random.Range(0, 1 + 1) > 0 ? 1 : -1;
        Invoke("RandomDir", Random.Range(2f, 5f));
    }


    void EffectOn(int index)
    {
        switch (index)
        {
            case 0:
                EffectManager.instance.SpawnEffect(handEffectTrans.position, (int)MonsterEffect.Wolf_Hand, Vector2.zero, transform.localScale.x < 0);
                break;

            case 1:
                EffectManager.instance.SpawnEffect(biteEffectTrans.position, (int)MonsterEffect.Wolf_Bite, Vector2.zero, transform.localScale.x < 0);
                break;
            
            case 2:
                EffectManager.instance.SpawnEffect(teleportEffectTrans.position, (int)MonsterEffect.Wolf_Teleport, Vector2.zero, transform.localScale.x < 0);
                break;
            

            default:
                break;
        }
    }

    protected override void AtkDetect(int index = 0)
    {
        base.AtkDetect(index);
        
        switch (index)
        {
            case 0:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Wolf_Hand);
                break;

            case 1:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Wolf_Bite);
                break;

            default:
                break;
        }
    }

    void TeleportEnter()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)target.position, Vector2.down, raycastDistance, groundLayer);
                        
        if(!hit)
        {
            return;
        }

        isAtking = true;
        canAct = false;
        rigid.velocity = Vector2.zero;

        float spawnY = hit.point.y;
        dir = (target.localScale.x > 0) ? -1 : 1;
        RaycastHit2D wallCheck = Physics2D.Raycast((Vector2)target.position, Vector2.right * dir, wallcastDistance, groundLayer);
        int time = 1;
        float spawnX = target.position.x;

        if(wallCheck)
        {
            for (; time < (int)wallCheck.distance * 4; time ++)
            {
                RaycastHit2D here = Physics2D.Raycast(new Vector2(target.position.x + dir * time * 0.25f, target.position.y), Vector2.down, raycastDistance, groundLayer);
                if(!here || Mathf.Abs(spawnY - here.point.y) > 0.1f)
                {
                    time --;
                    break;
                }
                spawnY = here.point.y;
            }
            spawnX = target.position.x + dir * time * 0.25f;
            Debug.Log(wallCheck.distance);

            
            if(dir < 0 && wallCheck.point.x + 1.6f >= spawnX)
            {
                spawnX = wallCheck.point.x + 1.6f;
            }
                

            if(dir > 0 && wallCheck.point.x - 1.6f <= spawnX)
                spawnX = wallCheck.point.x - 1.6f;
            
        }
        else
        {
            for (; time < wallcastDistance * 4; time ++)
            {
                RaycastHit2D here = Physics2D.Raycast(new Vector2(target.position.x + dir * time * 0.25f, target.position.y), Vector2.down, raycastDistance, groundLayer);
                if(!here || Mathf.Abs(spawnY - here.point.y) > 0.1f)
                {
                    time --;
                    break;
                }
                spawnY = here.point.y;
            }
            spawnX = target.position.x + dir * time * 0.25f;
        }

        transform.localScale = new Vector3(dir * monsterData.imageScale, monsterData.imageScale, 1);

        telePos = new Vector3(spawnX, spawnY + 1.5f, transform.position.z);
        animator.SetTrigger("Teleport");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Wolf_Dash);
    }

    protected void Teleport()
    {
        transform.position = telePos;
        dir = (target.localScale.x > 0) ? -1 : 1;
        transform.localScale = new Vector3(dir * monsterData.imageScale, monsterData.imageScale, 1);
        rigid.velocity = Vector2.zero;
    }

    protected override void AtkEnd()
    {
        base.AtkEnd();
        SetAtkType();
    }
}
