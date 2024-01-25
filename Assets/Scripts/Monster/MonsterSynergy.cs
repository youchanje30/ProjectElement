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
    float DefaultDamage { get { return battle.atkDamage; } }
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
    [Tooltip("ȭ�� ������ ������")]
    public float BurnDamgeIncreseRate;
    [Tooltip("���� �ð�")]
    public float Heatingduration;
    #endregion

    #region Ȯ��
    [Header("Ȯ��")]
    public bool isDiffusion;
    [Tooltip("Ȯ�� ����")]
    [SerializeField] Vector3 DiffusionRange;
    #endregion

    #region �ν�
    [Header("�ν�")]
    [Tooltip("���� ������ ������")]
    public float DamgeDecreseRate;
    [Tooltip("�ν� �ð�")]
    public float CorrosionDuration;
    #endregion

    #region �ҿ뵹��
    [Header("�ҿ뵹��")]
    public bool isBind;
    [Tooltip("�ҿ뵹�� �ð�")]
    public float BindTime;
    #endregion

    #region ǳȭ
    [Header("ǳȭ")]
    public bool isWeathering;
    [Tooltip("ǳȭ �ð�")]
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
            }
        }
        // if (!HaveSynergy)
        else
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
        if(monster.isHit == true)
        {
            StartCoroutine(HitFalse());
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
        float evaporationDmg = DefaultDamage * ( 1 + (EvaporationDamgeIncreseRate / 100));
        monster.GetDamaged(evaporationDmg);
        Debug.Log("���� " + evaporationDmg);
    }

    public IEnumerator Heating()
    {
        float burnDamage = passive.burnDamage;
        passive.burnDamage *= 1 + (BurnDamgeIncreseRate / 100);
        //monsterdebuff.ContinueBuff(passive.burnDamage, passive.duration[(int)BuffTypes.Burn], passive.tick[(int)BuffTypes.Burn], BuffTypes.Burn);
        Debug.Log("���� " + passive.burnDamage);
        yield return new WaitForSeconds(Heatingduration);
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
        yield return new WaitForSeconds(CorrosionDuration);
        monster.damage = monsterDamage;
    }

    public IEnumerator Diffusion()
    {
        Debug.Log("Ȯ��");
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, DiffusionRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Monster") && !isDiffusion)
            {
                collider.GetComponentInParent<MonsterDebuffBase>().ContinueBuff(passive.burnDamage, passive.duration[(int)BuffTypes.Burn], passive.tick[(int)BuffTypes.Burn], BuffTypes.Burn);
                isDiffusion = true;
            }
            // yield return new WaitForSeconds(SynergyCoolTime);
            // isDiffusion = false;
        }
        yield return new WaitForSeconds(SynergyCoolTime);
        isDiffusion = false;
    }

    public IEnumerator Weathering()
    {
        isWeathering = true;
        Debug.Log("ǳȭ" );
        yield return new WaitForSeconds(WeateringTime);
        isWeathering = false;
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
