using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSynergy : MonoBehaviour
{
    [SerializeField] MonsterBase monster;
    [SerializeField] MonsterDebuffBase monsterdebuff;
    [SerializeField] Battle battle;
    [SerializeField] PassiveSystem passive;


    [SerializeField] WeaponTypes[] Synergy;
    [SerializeField] int SynergyIndex;
    [SerializeField] bool HaveSynergy = false;
    [SerializeField] bool CanSynergy = false;
    float DefaultDamage;
    [Tooltip("시너지 효과 후 쿨타임")]
    [SerializeField] float SynergyCoolTime;
    float SynergyHoldingTime;
    [Tooltip("시너지 보유시간")]
    [SerializeField] float SynergyClearTime;

    #region 증발
    [Header("증발")]
    public float EvaporationDamgeIncreseRate;
    #endregion

    #region 가열
    [Header("가열")]
    [Tooltip("화상 데미지 증가율")]
    public float BurnDamgeIncreseRate;
    [Tooltip("가열 시간")]
    public float Heatingduration;
    #endregion

    #region 확산
    [Header("확산")]
    public bool isDiffusion;
    [Tooltip("확산 범위")]
    [SerializeField] Vector3 DiffusionRange;
    #endregion

    #region 부식
    [Header("부식")]
    [Tooltip("몬스터 데미지 감소율")]
    public float DamgeDecreseRate;
    [Tooltip("부식 시간")]
    public float CorrosionDuration;
    #endregion

    #region 소용돌이
    [Header("소용돌이")]
    public bool isBind;
    [Tooltip("소용돌이 시간")]
    public float BindTime;
    #endregion

    #region 풍화
    [Header("풍화")]
    public bool isWeathering;
    [Tooltip("풍화 시간")]
    public float WeateringTime;
    #endregion

    void Start()
    {
        if (!monster) { monster = GetComponent<MonsterBase>(); }
        if (!battle) { battle = GameObject.FindGameObjectWithTag("Player").GetComponent<Battle>(); }
        if (!passive) { passive = battle.GetComponent<PassiveSystem>(); }
        CanSynergy = true;
    }

    void Update()
    {
        DefaultDamage = battle.atkDamage;
        if (HaveSynergy)
        {
            SynergyHoldingTime += Time.deltaTime;
            if (SynergyHoldingTime >= SynergyClearTime)
            {
                HaveSynergy = false;
                SynergyHoldingTime = 0;
                SynergyIndex = 0;
            }
        }
        if (!HaveSynergy)
        {
            for (int i = 0; i < Synergy.Length; i++)
            {
                Synergy[i] = WeaponTypes.None;
                SynergyHoldingTime = 0;
                SynergyIndex = 0;
            }
        }
        if (SynergyIndex == 2)
        {
            SynergyEffect();
            for (int i = 0; i < Synergy.Length; i++)
            {
                Synergy[i] = WeaponTypes.None;
                SynergyHoldingTime = 0;
                SynergyIndex = 0;
            }
            HaveSynergy = false;
            CanSynergy = false;
            StartCoroutine(ReturnCoolTIme());
        }

    }

    public void GetSynergy(WeaponTypes types)
    {
        if (CanSynergy)
        {

            if (Synergy[SynergyIndex] == WeaponTypes.None)
            {
                Synergy[SynergyIndex] = types;
                if (Synergy[0] != Synergy[1])
                {
                    HaveSynergy = true;
                    SynergyIndex++;
                    SynergyHoldingTime = 0;
                }
                else if (Synergy[0] == Synergy[1])
                {
                    Synergy[SynergyIndex] = WeaponTypes.None;
                }
            }
        }
    }
    public IEnumerator ReturnCoolTIme()
    {
        yield return new WaitForSeconds(SynergyCoolTime);
        CanSynergy = true;
    }
    public void SynergyEffect()
    {
        for (int j = 0; j < Synergy.Length; j++)
        {
            for (int i = 0; i < Synergy.Length; i++)
            {
                if (Synergy[j] == WeaponTypes.Sword && Synergy[i] == WeaponTypes.Shield)
                {
                    StartCoroutine(Heating());
                }

                if (Synergy[j] == WeaponTypes.Sword && Synergy[i] == WeaponTypes.Wand)
                {
                    Evaporation();
                }
                if (Synergy[j] == WeaponTypes.Sword && Synergy[i] == WeaponTypes.Bow)
                {
                    StartCoroutine(Diffusion());
                }

                if (Synergy[j] == WeaponTypes.Wand && Synergy[i] == WeaponTypes.Shield)
                {
                    StartCoroutine(Corrosion());
                }

                if (Synergy[j] == WeaponTypes.Wand && Synergy[i] == WeaponTypes.Bow)
                {
                    StartCoroutine(Bind());
                }

                if (Synergy[j] == WeaponTypes.Shield && Synergy[i] == WeaponTypes.Bow)
                {
                    StartCoroutine(Weathering());
    
                }
            }
        }


    }
    public void Evaporation()
    {
        monster.GetDamaged(DefaultDamage *= 1 + (EvaporationDamgeIncreseRate / 100));
        Debug.Log("증발 " + DefaultDamage);
    }

    public IEnumerator Heating()
    {
        float burnDamage = passive.burnDamage;
        passive.burnDamage *= 1 + (BurnDamgeIncreseRate / 100);
        //monsterdebuff.ContinueBuff(passive.burnDamage, passive.duration[(int)BuffTypes.Burn], passive.tick[(int)BuffTypes.Burn], BuffTypes.Burn);
        Debug.Log("가열 " + passive.burnDamage);
        yield return new WaitForSeconds(Heatingduration);
        passive.burnDamage = burnDamage;
    }

    public IEnumerator Bind()
    {
        isBind = true;
        Debug.Log("속박");
        yield return new WaitForSeconds(BindTime);
        isBind = false;
    }

    public IEnumerator Corrosion()
    {
        float monsterDamage = monster.damage;
        monster.damage *= 1 - (DamgeDecreseRate / 100);
        Debug.Log("부식");
        yield return new WaitForSeconds(CorrosionDuration);
        monster.damage = monsterDamage;
    }

    public IEnumerator Diffusion()
    {
        Debug.Log("확산");
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, DiffusionRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Monster" && !isDiffusion)
            {
                collider.GetComponentInParent<MonsterDebuffBase>().ContinueBuff(passive.burnDamage, passive.duration[(int)BuffTypes.Burn], passive.tick[(int)BuffTypes.Burn], BuffTypes.Burn);
                isDiffusion = true;
            }
            yield return new WaitForSeconds(SynergyCoolTime);
            isDiffusion = false;
        }
    }

    public IEnumerator Weathering()
    {
        isWeathering = true;
        Debug.Log("풍화" );
        yield return new WaitForSeconds(WeateringTime);
        isWeathering = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, DiffusionRange);
    }
}
