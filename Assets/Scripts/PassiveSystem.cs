using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffTypes { Burn = 0, Slow, Barrier/*, Stun*/ }

public class PassiveSystem : MonoBehaviour
{    
    [SerializeField] private Battle battle;
    [SerializeField] private PlayerStatus status;

    public int[] passiveRate;

    [Header("현재 상태")]
    public bool isGetBarrier;
    // 활은 상시로 켜져있음


    [Header("시간 관련")]
    public float[] duration;
    public float[] tick;
    
    [Header("수치 관련")]
    public float burnDamage;
    public float slowPer;
    public float shieldPer;
    public float bowPer;
    public float unAtkedTime;

    void Awake()
    {
        if(!battle)
            battle = GetComponent<Battle>();
        if(!status)
            status = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        if(battle.WeaponType == WeaponTypes.Shield && !isGetBarrier)
        {
            unAtkedTime += Time.deltaTime;
            if(unAtkedTime >= duration[(int)WeaponTypes.Shield])
            {              
                isGetBarrier = true;
                unAtkedTime = 0f;
                ActivePassive(WeaponTypes.Shield);
            }
        }       
    }

    public void ActivePassive(WeaponTypes type , MonsterDebuffBase monster = null)
    {
        switch (type)
        {
            case WeaponTypes.Sword: // 불
                monster.ContinueBuff(burnDamage, duration[(int)BuffTypes.Burn], tick[(int)BuffTypes.Burn], BuffTypes.Burn);
                break;


            case WeaponTypes.Shield:
                status.barrier = status.maxHp * shieldPer * 0.01f;
                break;

            case WeaponTypes.Wand:
                monster.ContinueBuff(0f, duration[(int)BuffTypes.Slow], tick[(int)BuffTypes.Slow], BuffTypes.Slow);
                break;            

            default:
                Debug.Log("Active Passive Default Case");
                break;
        }
    }

    public void Swaped()
    {
        if(battle.WeaponType != WeaponTypes.Shield)
        {
            status.barrier = 0f;
            unAtkedTime = 0f;
            isGetBarrier = false;
        }
        
    }

    //public void ActiveSynergy(WeaponTypes type , MonsterSynergy monster = null)
    //{
    //    monster.GetSynergy(type);
    //}
    public void Atked()
    {
        unAtkedTime = 0f;
        isGetBarrier = false;
    }
}
