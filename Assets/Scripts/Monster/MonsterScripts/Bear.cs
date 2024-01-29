using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonsterBase
{
    public enum AtkTypes { HandAtk = 0, DownAtk = 1, };
    


    [SerializeField] float[] atkChargingTime;


    public override void GetDamaged(float getDamage, bool canKncokBack = true)
    {
        if(isKnockback || isDead) return;

        curHp -= getDamage;

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
                
        if(Random.Range(1, 100 + 1) < 50)
            animator.SetTrigger("HandAtk");
        else
            animator.SetTrigger("DownAtk");
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

            default:
                Debug.Log("Called ChargingAct : default");
                break;
        }
    }


    #endregion
}
