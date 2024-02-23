using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSynergy : MonoBehaviour
{
    [SerializeField] MonsterBase monster;
    [SerializeField] MonsterDebuffBase monsterdebuff;
    [SerializeField] Battle battle;
    [SerializeField] PassiveSystem passive;


    [SerializeField] ElementalData[] Synergy;
    [SerializeField] GameObject[] SynergyImage;
    [SerializeField] int SynergyIndex;
    [SerializeField] bool HaveSynergy = false;
    [SerializeField] bool CanSynergy = false;
    float DefaultDamage { get { return battle.atkDamage; } }
    [Tooltip("시너지 쿨타임")]
    [SerializeField] float SynergyCoolTime;
    float SynergyHoldingTime;
    [Tooltip("시너지 보유 시간")]
    [SerializeField] float SynergyClearTime;

    #region 증발
    [Header("증발")]
    public float EvaporationDamgeIncreseRate;
    #endregion
     
    #region 가열
    [Header("가열")]
    public float BurnDamgeIncreseRate;
    public float Heatingduration;
    #endregion

    #region 확산
    [Header("확산")]
    public bool isDiffusion;
    [SerializeField] Vector3 DiffusionRange;
    #endregion

    #region 부식
    [Header("부식")]
    public float DamgeDecreseRate;
    public float CorrosionDuration;
    #endregion

    #region 소용돌이
    [Header("소용돌이")]
    public bool isBind;
    public float BindTime;
    #endregion

    #region 풍화
    [Header("풍화")]
    public bool isWeathering;
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
        if (HaveSynergy)
        {
            SynergyHoldingTime += Time.deltaTime;
            if (SynergyHoldingTime >= SynergyClearTime)
            {
                HaveSynergy = false;
                SynergyHoldingTime = 0;
                SynergyIndex = 0;
                for (int i = 0; i < SynergyImage.Length; i++)
                {
                    SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
                }
            }
        }
        // if (!HaveSynergy)
        else
        {
            for (int i = 0; i < Synergy.Length; i++)
            {
                Synergy[i] = null;
                SynergyHoldingTime = 0;
                SynergyIndex = 0;
            }
        }

        if (SynergyIndex == 2)
        {
            SynergyEffect();
            for (int i = 0; i < Synergy.Length; i++)
            {
                Synergy[i] = null;
                SynergyHoldingTime = 0;
                SynergyIndex = 0;
            }
            HaveSynergy = false;
            CanSynergy = false;
            //StartCoroutine(ReturnCoolTIme());
        }
        if (monster.isHit == true)
        {

        }
 
      
    }

    public void GetSynergy(ElementalData types)
    {
        if (CanSynergy)
        {       
            if (Synergy[SynergyIndex] == null)
            {
                Synergy[SynergyIndex] = types;
                SynergyImage[SynergyIndex].GetComponent<SpriteRenderer>().sprite = Synergy[SynergyIndex].SynergyIcon;
                if (Synergy[0] != Synergy[1])
                {
                    HaveSynergy = true;
                    SynergyIndex++;
                    SynergyHoldingTime = 0;
                }
                else if (Synergy[0] == Synergy[1])
                {
                    Synergy[SynergyIndex] = null;
                    SynergyImage[SynergyIndex].GetComponent<SpriteRenderer>().sprite = null;
                }
               
            }
        }
    }
    public IEnumerator ReturnCoolTIme(float i)
    {
        yield return new WaitForSeconds(SynergyCoolTime - i);
        CanSynergy = true;
    }
    public void SynergyEffect()
    {
        for (int j = 0; j < Synergy.Length; j++)
        {
            for (int i = 0; i < Synergy.Length; i++)
            {
                if (Synergy[j].WeaponTypes == WeaponTypes.Sword && Synergy[i].WeaponTypes == WeaponTypes.Shield)
                {
                    StartCoroutine(Heating());
                }

                if (Synergy[j].WeaponTypes == WeaponTypes.Sword && Synergy[i].WeaponTypes == WeaponTypes.Wand)
                {
                    StartCoroutine(Evaporation());
                }
                if (Synergy[j].WeaponTypes == WeaponTypes.Sword && Synergy[i].WeaponTypes == WeaponTypes.Bow)
                {
                    StartCoroutine(Diffusion());
                }

                if (Synergy[j].WeaponTypes == WeaponTypes.Wand && Synergy[i].WeaponTypes == WeaponTypes.Shield)
                {
                    StartCoroutine(Corrosion());
                }

                if (Synergy[j].WeaponTypes == WeaponTypes.Wand && Synergy[i].WeaponTypes == WeaponTypes.Bow)
                {
                    StartCoroutine(Bind());
                }

                if (Synergy[j].WeaponTypes == WeaponTypes.Shield && Synergy[i].WeaponTypes == WeaponTypes.Bow)
                {
                    StartCoroutine(Weathering());
                }
            }
        }
    }
    public IEnumerator Evaporation()
    {
        float evaporationDmg = DefaultDamage * ( 1 + (EvaporationDamgeIncreseRate / 100));
        monster.GetDamaged(evaporationDmg);
        Debug.Log("증발 " + evaporationDmg);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < SynergyImage.Length; i++)
        {
            SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        StartCoroutine(ReturnCoolTIme(2f));
    }

    public IEnumerator Heating()
    {
        float burnDamage = passive.burnDamage;
        passive.burnDamage *= 1 + (BurnDamgeIncreseRate / 100);
        //monsterdebuff.ContinueBuff(passive.burnDamage, passive.duration[(int)BuffTypes.Burn], passive.tick[(int)BuffTypes.Burn], BuffTypes.Burn);
        Debug.Log("가열 " + passive.burnDamage);
        yield return new WaitForSeconds(Heatingduration);
        passive.burnDamage = burnDamage;
        for (int i = 0; i < SynergyImage.Length; i++)
        {
            SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        StartCoroutine(ReturnCoolTIme(Heatingduration));
    }

    public IEnumerator Bind()
    {
        isBind = true;
        Debug.Log("소용돌이");
        yield return new WaitForSeconds(BindTime);
        isBind = false;
        for (int i = 0; i < SynergyImage.Length; i++)
        {
            SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        StartCoroutine(ReturnCoolTIme(BindTime));
    }

    public IEnumerator Corrosion()
    {
        float monsterDamage = monster.damage;
        monster.damage *= 1 - (DamgeDecreseRate / 100);
        Debug.Log("부식");
        yield return new WaitForSeconds(CorrosionDuration);
        monster.damage = monsterDamage;
        for (int i = 0; i < SynergyImage.Length; i++)
        {
            SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        StartCoroutine(ReturnCoolTIme(CorrosionDuration));
    }

    public IEnumerator Diffusion()
    {
        Debug.Log("확산");
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, DiffusionRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Monster") && !isDiffusion)
            {
                collider.GetComponentInParent<MonsterDebuffBase>().ContinueBuff(passive.burnDamage, passive.passiveData[1].duration, passive.passiveData[1].tick, BuffTypes.Burn);
                Debug.Log(collider.name + "Diffusion Hit");
                isDiffusion = true;
            }
            // yield return new WaitForSeconds(SynergyCoolTime);
            // isDiffusion = false;
        }
        yield return new WaitForSeconds(2f);
        isDiffusion = false;
        for (int i = 0; i < SynergyImage.Length; i++)
        {
            SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        StartCoroutine(ReturnCoolTIme(2f));
    }

    public IEnumerator Weathering()
    {
        isWeathering = true;
        Debug.Log("풍화" );
        yield return new WaitForSeconds(WeateringTime);
        isWeathering = false;
        for (int i = 0; i < SynergyImage.Length; i++)
        {
            SynergyImage[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        StartCoroutine(ReturnCoolTIme(WeateringTime));
    }

    public IEnumerator HitFalse()
    {
        yield return new WaitForSeconds(0.5f);
        monster.GetComponentInParent<MonsterBase>().isHit = false;
        Debug.Log("isHit false");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, DiffusionRange);
    }
}
