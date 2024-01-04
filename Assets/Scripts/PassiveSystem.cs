using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffTypes { Burn = 1, Slow, Barrier }

public class PassiveSystem : MonoBehaviour
{    
    [SerializeField] private Battle playerBattle;
    [SerializeField] private Monster monster;


    [Header("현재 상태")]
    // None은 없을 듯?
    public bool isBurn;
    public bool isSlow;
    public bool canSlow;
    public bool isGetBarrier;
    // 활은 상시로 켜져있음

    [Header("상태 이상 시간 관련")]
    public float curBurnTime;
    [SerializeField] float maxBurnTime;
    public float curSlowTime;
    [SerializeField] float maxSlowTime;

    [Header("틱 관련")]
    public float curBurnTick;
    [SerializeField] float maxBurnTick;
    [SerializeField] float slowTime;
    [SerializeField] float reSlowTime;
    
    // public float curSlowTick;
    // [SerializeField] float maxSlowTick;

    [Header("데미지 관련")]
    public float burnDamage;


    void Awake()
    {
        switch (gameObject.tag)
        {
            case "Player":
                if(!playerBattle)
                    playerBattle = GetComponent<Battle>();
                break;

            case "Enemy":
                if(!monster)
                    monster = GetComponent<Monster>();
                break;

            default:
                break;
        }
    }



    // 현재는 burn 밖에 없음
    public void EnemyPassive(BuffTypes buffType, float damage = 0f)
    {
        switch (buffType)
        {
            case BuffTypes.Burn:
                curBurnTime = maxBurnTime;
                isBurn = true;
                StopCoroutine("Burn");
                burnDamage = damage;
                StartCoroutine(Burn());
                break;

            case BuffTypes.Slow:

                break;

            case BuffTypes.Barrier:

                break;


            default:
                break;
        }
    }

    
    IEnumerator Burn()
    {
        while (curBurnTime > 0)
        {
            monster.GetDamaged(burnDamage, false);
            yield return new WaitForSeconds(curBurnTick);
            curBurnTime -= curBurnTick;
        }
    }

    IEnumerator Slow()
    {
        isSlow = true;
        canSlow = true;
        // monster.moveSpeed = 
        yield return new WaitForSeconds(slowTime);
        isSlow = false;
        yield return new WaitForSeconds(reSlowTime);
        canSlow = false;
    }
}
