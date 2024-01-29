using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonsterBase
{
    public enum AtkTypes { HandAtk = 0, DownAtk = 1, Rush = 2, };
    
    // [SerializeField] BoxCollider2D rushCol;
    [SerializeField] bool isRushAtk = false;

    [SerializeField] float[] atkChargingTime;
    [SerializeField] DetectPlayer atkCheck;


    protected override void Update()
    {
        base.Update();
        if(isRushAtk && atkCheck.isEnter)
        {
            isRushAtk = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Battle>().GetDamaged(damage);
        }
    }


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

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(isRushAtk && other.CompareTag("Player"))
    //     {
    //         isRushAtk = false;
    //         other.GetComponent<Battle>().GetDamaged(damage);
    //     }
    // }

    #endregion
}
