using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;
    public DamageNumber numberPrefab;

    void Awake()
    {
        if(!instance)
            instance = this;
        else
        {
            instance.Init();
            Destroy(this);
        }
    }

    public void Init()
    {
        if(!numberPrefab)
            numberPrefab = GetComponentInChildren<DamageNumber>();
    }

    public void SpawnDamage(Vector3 pos, float damage)
    {
        DamageNumber damageNumber = numberPrefab.Spawn(pos, (int)damage);
    }
}
