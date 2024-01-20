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


    [SerializeField] WeaponTypes[] Synergy;
    [SerializeField] int SynergyIndex;
    [SerializeField] bool HaveSynergy = false;
    [SerializeField] bool CanSynergy = false;
    float DefaultDamage;
    [Tooltip("�ó��� ȿ�� �� ��Ÿ��")]
    [SerializeField] float SynergyCoolTime;
    float SynergyHoldingTime;
    [Tooltip("�ó��� �����ð�")]
    [SerializeField] float SynergyClearTime;

    #region ����
    [Header("����")]
    public float EvaporationDamgeIncreseRate;
    #endregion

    #region ����
    [Header("����")]
    public float BurnDamgeIncreseRate;
    public float BurnDamgeIncreseduration;
    #endregion

    #region Ȯ��
    #endregion

    #region �ν�
    [Header("�ν�")]
    public float DamgeDecreseRate;
    public float DamgeDecreseduration;
    #endregion

    #region �ҿ뵹��
    [Header("�ҿ뵹��")]
    public bool isBind;
    public float BindTime;
    #endregion

    #region ǳȭ
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
                    Diffusion();
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
                    Weathering();
    
                }
            }
        }


    }
    public void Evaporation()
    {
        monster.GetDamaged(DefaultDamage *= 1 + (EvaporationDamgeIncreseRate / 100));
        Debug.Log("���� " + DefaultDamage);
    }

    public IEnumerator Heating()
    {
        float burnDamage = passive.burnDamage;
        passive.burnDamage *= 1 + (BurnDamgeIncreseRate / 100);
        //monsterdebuff.ContinueBuff(passive.burnDamage, passive.duration[(int)BuffTypes.Burn], passive.tick[(int)BuffTypes.Burn], BuffTypes.Burn);
        Debug.Log("���� " + passive.burnDamage);
        yield return new WaitForSeconds(BurnDamgeIncreseduration);
        passive.burnDamage = burnDamage;
    }

    public IEnumerator Bind()
    {
        isBind = true;
        Debug.Log("�ӹ�");
        yield return new WaitForSeconds(BindTime);
        isBind = false;
    }

    public IEnumerator Corrosion()
    {
        float monsterDamage = monster.damage;
        monster.damage *= 1 - (DamgeDecreseRate / 100);
        Debug.Log("�ν�");
        yield return new WaitForSeconds(DamgeDecreseduration);
        monster.damage = monsterDamage;
    }

    public void Diffusion()
    {
        Debug.Log("Ȯ��");
    }

    public void Weathering()
    {
        Debug.Log("ǳȭ" );
    }
}
