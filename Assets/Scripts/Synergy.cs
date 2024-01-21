using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy : MonoBehaviour
{
    [SerializeField] private Battle battle;
    [Header("풍화 데미지 증가율")]
    public float SouthSpiritDamageIncreaseRate;
    [Header("베리어 감소율")]
    public float BarrierDecreaseRate;
    void Start()
    {
        if (!battle)
            battle = GetComponent<Battle>();
    }

    // Update is called once per frame
    void Update()
    {
        battle.WeatheringDamage = battle.atkDamage *( 1 +(SouthSpiritDamageIncreaseRate / 100));
    }
    public void ActiveSynergy(WeaponTypes type, MonsterSynergy monster = null)
    {
         monster.GetSynergy(type);
    }
}
