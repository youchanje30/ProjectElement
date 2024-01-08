using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffTypes { Burn = 0, Slow, Barrier }

public class PassiveSystem : MonoBehaviour
{    
    [SerializeField] private Battle playerBattle;

    public float[] passiveRate;

    [Header("현재 상태")]
    public bool isGetBarrier;
    // 활은 상시로 켜져있음

    [Header("시간 관련")]
    public float[] duration;
    public float[] tick;
    
    // public float curSlowTick;
    // [SerializeField] float maxSlowTick;

    [Header("수치 관련")]
    public float burnDamage;
    public float slowPer;

    void Awake()
    {
        if(!playerBattle)
            playerBattle = GetComponent<Battle>();
    }



    public void ActivePassive(WeaponTypes type , MonsterDebuffBase monster = null)
    {
        switch (type)
        {
            case WeaponTypes.Sword: // 불
                monster.ContinueBuff(burnDamage, duration[(int)BuffTypes.Burn], tick[(int)BuffTypes.Burn], BuffTypes.Burn);
                break;


            case WeaponTypes.Shield:
                // 쉴드 얻는거 만들어야함
                break;


            case WeaponTypes.Wand:
                monster.ContinueBuff(0f, duration[(int)BuffTypes.Slow], tick[(int)BuffTypes.Slow], BuffTypes.Slow);
                break;
            

            default:
                Debug.Log("Active Passive Default Case");
                break;
        }
    }


    // 현재는 burn 밖에 없음
    // public void EnemyPassive(BuffTypes buffType, float damage = 0f)
    // {
    //     switch (buffType)
    //     {
    //         case BuffTypes.Burn:
    //             curBurnTime = maxBurnTime;
    //             isBurn = true;
    //             StopCoroutine("Burn");
    //             burnDamage = damage;
    //             StartCoroutine(Burn());
    //             break;

    //         case BuffTypes.Slow:

    //             break;

    //         case BuffTypes.Barrier:

    //             break;


    //         default:
    //             break;
    //     }
    // }
}
