using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffSystem : MonoBehaviour
{
    private Battle battle;

    [System.Serializable]
    struct Debuff
    {
        public string functionName;
        public bool isActive;
        public float duration;
        public float tick; // 슬로우의 경우 이게 reSlowTime
        public float damage;
    }
    
    [SerializeField] Debuff[] debuffs;

    void Awake()
    {
        battle = GetComponent<Battle>();
    }

    public IEnumerator Poison()
    {   
        debuffs[0].isActive = true;
        
        while (debuffs[0].duration > 0)
        {
            battle.GetDamaged(debuffs[0].damage, false);
            yield return new WaitForSeconds(debuffs[0].tick);
            Debug.Log("Get Poison");
            debuffs[0].duration -= debuffs[0].tick;
        }
        
        debuffs[0].isActive = false;
        debuffs[0].damage = 0f;
    }

    public void ContinueBuff(float damage = 0f, float duration = 0f, float tick = 0f)
    {
        
        if(damage > debuffs[0].damage)
            debuffs[0].damage = damage;

        if(duration > debuffs[0].duration)
            debuffs[0].duration = duration;

        debuffs[0].tick = tick;
        
        
        if(!debuffs[0].isActive)
            StartCoroutine(debuffs[0].functionName);
            
    }
}
