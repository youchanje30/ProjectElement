using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSynergy : MonoBehaviour
{
    [SerializeField] MonsterBase monster;


    [SerializeField] WeaponTypes[] Synergy;
    [SerializeField] int SynergyIndex;
    [SerializeField] bool HaveSynergy = false;
    [SerializeField] float SynergyHoldingTime;
    [SerializeField] float SynergyClearTime;

    void Start()
    {
        if (!monster) { monster = GetComponent<MonsterBase>(); }
    }

    void Update()
    {
        if(HaveSynergy)
        {
            SynergyHoldingTime += Time.deltaTime;
            if(SynergyHoldingTime >= SynergyClearTime)
            {
                HaveSynergy = false;
                SynergyHoldingTime = 0;
            }
        }

    }

    public void GetSynergy(WeaponTypes types)
    {
        if (Synergy[SynergyIndex] == WeaponTypes.None)
        {
            Synergy[SynergyIndex] = types;
            
            HaveSynergy = true;               
        }
        else
        {
            SynergyIndex++;
            Synergy[SynergyIndex] = types;
            SynergyHoldingTime = 0;
            HaveSynergy = true;
        }
    }
}
