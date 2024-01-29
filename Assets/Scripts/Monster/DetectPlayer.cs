using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectStates { Track, TryAtk , AtkCheck }

public class DetectPlayer : MonoBehaviour
{
    #region 필요 컴포넌트
    MonsterBase monster;
    CheckEnterExit[] checks;
    #endregion

    #region 판별 종류 및 결과
    public DetectStates detectStates;
    [SerializeField] int checkedTrigger;
    public bool isEnter { get { return checkedTrigger > 0; } }
    #endregion


    void Awake()
    {
        if(!monster)
            monster = GetComponentInParent<MonsterBase>();

        checks = GetComponentsInChildren<CheckEnterExit>();
    }

    public bool Check()
    {
        for (int i = 0; i < checks.Length; i++)
        {
            if(checks[i].isEnterTrigger)
            {
                return true;
            }
        }
        return false;
    }


    void DetectVerification()
    {
        int cnt = 0;
        for (int i = 0; i < checks.Length; i++)
        {
            if(checks[i].isEnterTrigger)
            {
                cnt ++;
            }
        }

        if(cnt != checkedTrigger)
        {
            Debug.Log("오차 발생");
        }

        checkedTrigger = cnt;
    }

    
    public void AddCheck()
    {
        checkedTrigger ++;
    }
    
    public void SubtractCheck()
    {
        checkedTrigger --;
        if(checkedTrigger <= 2)
        {
            DetectVerification();
        }
    }
}
