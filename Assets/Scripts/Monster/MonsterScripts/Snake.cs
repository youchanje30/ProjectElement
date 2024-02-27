using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonsterBase
{
    [Header("독 관련 정보")]
    [SerializeField] float poisonDuration;
    [SerializeField] float poisonTick;
    [SerializeField] float poisonDamage;
    [Space(20f)]

    [Header("이펙트 위치 정보")]
    [SerializeField] Transform poisonPos;

    protected override void AtkDetect(int index = 0)
    {
        Vector3 detectPos = transform.position;
        detectPos.x += atkInfo[index].atkPos.x * (transform.localScale.x > 0 ? 1 : -1);
        detectPos.y += atkInfo[index].atkPos.y;
        
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(detectPos, atkInfo[index].atkSize, 0, LayerMask.GetMask("Player"));
        foreach(Collider2D collider in collider2Ds)
        {
            Debug.Log(collider.tag);
            if(!collider.CompareTag("Player")) continue;
                
            collider.GetComponent<Battle>().GetDamaged(damage);
            collider.GetComponent<DebuffSystem>().ContinueBuff(poisonDamage, poisonDuration, poisonTick);
        }
    }

    void Effect(int index = 0)
    {
        Vector3 detectPos = poisonPos.position;
        EffectManager.instance.SpawnEffect(detectPos, (int)MonsterEffect.poison, atkInfo[index].atkSize, transform.localScale.x < 0);
    }
}
