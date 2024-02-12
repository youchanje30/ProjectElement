using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum BuffTypes { Burn, Slow, Barrier /*, Stun*/ }

public class PassiveSystem : MonoBehaviour
{
    [SerializeField] private Battle battle;
    [SerializeField] private PlayerStatus status;


    [Header("현재 상태")]
    public bool isGetBarrier;
    // 활은 상시로 켜져있음


    // [Header("시간 관련")]
    // public int[] passiveRate;
    // public float[] duration;
    // public float[] tick;

    

    
    [TitleGroup("패시브 관련 기본 정보")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    [LabelText("패시브 관련 데이터")] public List<PassiveData> passiveData;
    [System.Serializable] public class PassiveData
    {
        [LabelText("패시브 이름")] public string passiveName;
        [LabelText("패시브 확률")] public int rate;
        [LabelText("패시브 지속 시간")] public float duration;
        [LabelText("패시브 틱")]public float tick;
    }


    [TitleGroup("패시브 관련 수치 정보")]
    [LabelText("화상 데미지")] public float burnDamage;
    [LabelText("슬로우 정도%")] public float slowPer;
    [LabelText("쉴드량 최대 체력 비례 %")] public float shieldPer;
    [LabelText("쿨타임 감소 비례 치명타 확률 %")] public float bowPer;
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
            // if(unAtkedTime >= duration[(int)WeaponTypes.Shield])
            if(unAtkedTime >= passiveData[(int)WeaponTypes.Shield].duration)
            {
                isGetBarrier = true;
                unAtkedTime = 0f;
                battle.barrier.SetActive(true);
                battle.barrier.GetComponent<Animator>().SetTrigger("GetBarrier");
                ActivePassive(WeaponTypes.Shield);
            }
        }       
    }

    public void ActivePassive(WeaponTypes type , MonsterDebuffBase monster = null)
    {
        switch (type)
        {
            case WeaponTypes.Sword: // 불
                monster.ContinueBuff(burnDamage, passiveData[1].duration, passiveData[1].tick, BuffTypes.Burn);
                break;


            case WeaponTypes.Shield:
                status.barrier = status.maxHp * shieldPer * 0.01f;
                isGetBarrier = true;
                battle.barrier.SetActive(true);
                battle.barrier.GetComponent<Animator>().SetTrigger("GetBarrier");
                break;

            case WeaponTypes.Wand:
                monster.ContinueBuff(0f, passiveData[2].duration, passiveData[2].tick, BuffTypes.Slow, slowPer);
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
            battle.barrier.SetActive(false);
        }
        
    }

    //public void ActiveSynergy(WeaponTypes type , MonsterSynergy monster = null)
    //{
    //    monster.GetSynergy(type);
    //}
    public void Atked()
    {
        unAtkedTime = 0f;
        // isGetBarrier = false;
    }
}
