using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum weaponTypes { shield, sword ,bow };
public class Battle : MonoBehaviour
{
    [Header("Player Status")]
    public float maxHp = 10f;
    public float curHp;
    public float def;
    public float meleeDmg;
    public float skillDmg;
    public float atkSpeed;
    public float crtRate;
    public float crtDmg;
    [Space(20f)]
    


    [Header("Weapon Setting")]
    public int weaponType;
    public bool Atking;
    public bool isGuard;
    public float[] Left_BeforAtkDelay;
    public float[] Left_AfterAtkDelay;
    public float[] Right_BeforAtkDelay;
    public float[] Right_AfterAtkDelay;
    public float[] Left_AtkCoolTime;
    public float[] Right_AtkCoolTime;
    public bool[] isAtkReady;
    public Transform[] atkPos;
    public Vector2[] atkSize;
    // public Transform[] atkPos;
    public GameObject arrow;

    void Awake()
    {
        // weaponType = 0;    
    }


    public void Atk(int id)
    {
        // if(!atk) return;

        if(id == 0) //좌클릭
        {
            if(weaponType == 2) return; //활은 좌클릭이 없어요
            StartCoroutine(LeftAtk());
        }
        else if (id == 1)
        {
            if(weaponType == 1) return; //칼은 우클릭이 없어요
            // StartCoroutine(RightAtk());
            if(weaponType == 2) StartCoroutine(BowAtk());
            if(weaponType == 0) StartCoroutine(ShieldGuard());
        }
        // Atking = true;
    }



    public IEnumerator LeftAtk()
    {
        if(isAtkReady[weaponType])
        {
            Atking = true;
            isAtkReady[weaponType] = false;

            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Left_BeforAtkDelay[weaponType]);

            // 공격 피격
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos[weaponType].position, atkSize[weaponType], 0);
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Monster")
                {
                    Debug.Log("Atk Succed");
                    collider.gameObject.GetComponent<Monster>().GetDamaged(meleeDmg);
                }
            }
            // 공격 중인거 종료
            Atking = false;

            yield return new WaitForSeconds(Left_AtkCoolTime[weaponType]);
            //애니메이션 종료 및 공격 종료
            isAtkReady[weaponType] = true;
        }
    }
    public IEnumerator BowAtk()
    {
        if(isAtkReady[weaponType])
        {
            Atking = true;
            isAtkReady[weaponType] = false;
            // 공격 애니메이션 시작
            // yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);

            //화살 소환
            GameObject ThrowArrow = Instantiate(arrow);
            ThrowArrow.transform.position = atkPos[weaponType].position; 
            ThrowArrow.transform.localScale =  new Vector3(transform.localScale.x, ThrowArrow.transform.localScale.y, ThrowArrow.transform.localScale.z);
            Atking = false;

            yield return new WaitForSeconds(Right_AtkCoolTime[weaponType]);

            isAtkReady[weaponType] = true;
        } 
    }

    public IEnumerator ShieldGuard()
    {
        Atking = true;
        // 차징 애니메이션 시작

        yield return new WaitForSeconds(Right_BeforAtkDelay[weaponType]);
        
        isGuard = true;
        Atking = false;
    }
    

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(atkPos[weaponType].position, atkSize[weaponType]);    
    }


   



    public void GetDamaged(float Damage)
    {
        if(isGuard) return;

        StopCoroutine(ShieldGuard());
    }
}
