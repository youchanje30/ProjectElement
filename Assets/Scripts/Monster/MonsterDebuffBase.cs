using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDebuffBase : MonoBehaviour
{
    [SerializeField] MonsterBase monster;
    [SerializeField] SpriteRenderer sprite;
    
    

    #region 지속되는 종류 (중복 방지) 초기화 형식

    [System.Serializable]
    struct Debuff
    {
        public string functionName;
        public bool isActive;
        public float duration;
        public float tick; // 슬로우의 경우 이게 reSlowTime
        public float damage;
        public Color debuffColor;
    }

    [SerializeField] Debuff[] debuffs;


    // 예외로 필요한 데이터들
    private bool canSlow = true;

    #endregion

    void Awake()
    {
        if(!monster)
            monster = GetComponent<MonsterBase>();
        if(!sprite)
            sprite = GetComponent<SpriteRenderer>();

        canSlow = true;
    }

    /*
    // public IEnumerator GetContinueDamage(float damage = 0f, float duration = 0f, float tick = 0f, BuffTypes type = BuffTypes.Burn)
    // {
    //     float currentTime = 0;
    //     while (currentTime < duration)
    //     {
    //         yield return new WaitForSeconds(tick);
    //         // currentTime += Time.deltaTime;
    //         if(damage > 0)
    //         {
    //             monster.GetDamaged(damage, false);
    //         }
    //     }
    // }
    */

    public IEnumerator Burn()
    {   
        debuffs[0].isActive = true;
        sprite.color = debuffs[0].debuffColor;
        
        while (debuffs[0].duration > 0)
        {
            monster.GetDamaged(debuffs[0].damage, false);
            yield return new WaitForSeconds(debuffs[0].tick);
            Debug.Log("Hit Burn");
            debuffs[0].duration -= debuffs[0].tick;
        }
        
        debuffs[0].isActive = false;
        debuffs[0].damage = 0f;
        sprite.color = new Color(1, 1, 1, 1);
    }   
    //public IEnumerator Stun(float duration)
    //{
    //    debuffs[3].isActive = true;
    //    monster.isStun = true;
    //    yield return new WaitForSeconds(duration);
    //    debuffs[3].isActive = false;
    //    monster.isStun = false;
    //}
    public IEnumerator Slow(float duration, float reSlowTime, float per)
    {
        debuffs[1].isActive = true;
        canSlow = false;
        monster.animator.SetFloat("animSpeed", (100 - per) * 0.01f);
        monster.moveSpeed = monster.monsterData.maxMoveSpeed * (100 - per) * 0.01f;
        // sprite.color = new Color(180f, 180f, 255f, 255f) / 255;
        sprite.color = debuffs[1].debuffColor;

        yield return new WaitForSeconds(duration);
        monster.animator.SetFloat("animSpeed", 1);
        monster.moveSpeed = monster.monsterData.maxMoveSpeed;
        debuffs[1].isActive = false;
        sprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(reSlowTime);

        canSlow = true;
    }

    public void ContinueBuff(float damage = 0f, float duration = 0f, float tick = 0f, BuffTypes type = BuffTypes.Burn, float etc = 30f)
    {
        if(type == BuffTypes.Slow && !canSlow)
        {
            return;
        }
        
        if(damage > debuffs[(int)type].damage)
            debuffs[(int)type].damage = damage;

        if(duration > debuffs[(int)type].duration)
            debuffs[(int)type].duration = duration;

        debuffs[(int)type].tick = tick;
        
        switch (type)
        {
            case BuffTypes.Burn:
                if(!debuffs[(int)type].isActive)
                    StartCoroutine(debuffs[(int)type].functionName);
                break;

            // case BuffTypes.Barrier:
            //     break;

            case BuffTypes.Slow:
                StartCoroutine(Slow(debuffs[1].duration, debuffs[1].tick, etc));
                break;

            //case BuffTypes.Stun:
            //    StartCoroutine(Stun(debuffs[3].duration));
            //    break;

            default:
                Debug.Log("Continue Buff Default Value");
                break;
        }
    }
}
