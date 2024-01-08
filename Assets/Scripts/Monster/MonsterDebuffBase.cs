using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDebuffBase : MonoBehaviour
{
    [SerializeField] MonsterBase monster;
    

    #region 지속되는 종류 (중복 방지) 초기화 형식
    [SerializeField] private string[] functionName;
    [SerializeField] private bool[] isActive;
    [SerializeField] private float[] duration;
    [SerializeField] private float[] tick; // 슬로우의 경우 이게 reSlowTime
    [SerializeField] private float[] damage;
    // 예외로 필요한 데이터들
    private bool canSlow = true;

    #endregion



    void Awake()
    {
        if(!monster)
            monster = GetComponent<MonsterBase>();

        canSlow = true;
    }

    /*
    // public IEnumerator GetContinueDamage(float damage = 0f, float duration = 0f, float tick = 0f, BuffTypes type = BuffTypes.Burn)
    // {
    //     float currentTime = 0;

    //     while (currentTime < duration)
    //     {
    //         yield return new WaitForSeconds(tick);
    //         // currentTime += Time.deltaTime;
    //         if(damage > 0)
    //         {
    //             monster.GetDamaged(damage, false);
    //         }
    //     }
        

    // }
    */

    public IEnumerator Burn(float duration, float tick, float damage)
    {
        isActive[0] = true;
        while (duration > 0)
        {
            monster.GetDamaged(damage, false);
            yield return new WaitForSeconds(tick);
            duration -= tick;
        }

        isActive[0] = false;
        this.damage[0] = 0f;
    }

    public IEnumerator Slow(float duration, float reSlowTime, float per)
    {
        isActive[1] = true;
        canSlow = false;
        monster.moveSpeed = monster.monsterData.maxMoveSpeed * (100 - per) * 0.01f;
        yield return new WaitForSeconds(duration);

        monster.moveSpeed = monster.monsterData.maxMoveSpeed;
        isActive[1] = false;
        yield return new WaitForSeconds(reSlowTime);

        canSlow = true;
    }

    public void ContinueBuff(float damage = 0f, float duration = 0f, float tick = 0f, BuffTypes type = BuffTypes.Burn, float etc = 30f)
    {
        if(type == BuffTypes.Slow && !canSlow)
        {
            return;
        }

        if(isActive[(int)type])
        {
            StopCoroutine(functionName[(int)type]);   
        }

        
        if(damage > this.damage[(int)type])
            this.damage[(int)type] = damage;

        this.duration[(int)type] = duration;
        this.tick[(int)type] = tick;
        
        switch (type)
        {
            case BuffTypes.Burn:
                if(damage > this.damage[0])
                    this.damage[0] = damage;

                StartCoroutine(Burn(this.duration[0], this.tick[0], this.damage[0]));
                break;

            // case BuffTypes.Barrier:
            //     break;

            case BuffTypes.Slow:
                StartCoroutine(Slow(this.duration[1], this.tick[1], etc));
                break;


            default:
                Debug.Log("Continue Buff Default Value");
                break;
        }
    }
}
