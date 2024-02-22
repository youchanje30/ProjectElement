using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
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
    [SerializeField] bool isMissState;

    

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
        if(isDead || isAtking || isKnockback || isStun)
        {
            canAct = false;
            canMove = false;
            canAtk = false;
            return;
        }
        base.CheckState();

        if(canTrack && !canAct && curAtkType == WolfAtkType.tele && curAtkCoolTime <= 0)
            canAct = true;
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

        if(curAtkType == WolfAtkType.tele && canTrack && canAct)
        {
            canAct = false;
            TeleportAtk();
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


    void TeleportAtk()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)target.position, Vector2.down, raycastDistance, groundLayer);
                        
        if(!hit)
        {
            Invoke("ReturnAct", 0.1f);
            return;
        }

        isAtking = true;
        canAct = false;
        isMissState = true;
        rigid.velocity = Vector2.zero;

        float spawnY = hit.point.y;
        int dir = (target.localScale.x > 0) ? -1 : 1;
        RaycastHit2D wallCheck = Physics2D.Raycast((Vector2)target.position, Vector2.right * dir, 5, groundLayer);
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
                Debug.Log("?");
            }
                

            if(dir > 0 && wallCheck.point.x - 1.6f <= spawnX)
                spawnX = wallCheck.point.x - 1.6f;
            
        }
        else
        {
            for (; time < 16; time ++)
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

        transform.position = new Vector3(spawnX, spawnY + 1.5f, transform.position.z);
        transform.localScale = new Vector3(dir * monsterData.imageScale, monsterData.imageScale, 1);
        
        animator.SetTrigger("Atk");
    }

    protected override void AtkEnd()
    {
        base.AtkEnd();
        SetAtkType();
        isMissState = false;
    }

    public override void GetDamaged(float getDamage, bool canKncokBack = true)
    {
        if(isMissState) return;
        base.GetDamaged(getDamage, canKncokBack);
    }

    void ReturnAct()
    {
        canAct = true;
    }
}
