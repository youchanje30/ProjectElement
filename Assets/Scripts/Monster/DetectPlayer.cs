using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectStates { Track, TryAtk }

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

    // 해당 방식을 통해 플레이어가 추적 범위 내에 들어왔는지 판별하려고 했으나, 이를 위해서는 적어도 2개의 오브젝트가 필요할 것 같아 보임.
    // 2개로 분류한다면 해당 오브젝트에서 bool값을 반환하는 함수를 통해 플레이어가 근처에 있다면 두 번째의 오브젝트를 실행하는 것이 방법 중 하나로 보임
    // 만일 단일 오브젝트에서 들어온 콜라이더가 어떤 것인지를 구분할 수 있다면 좋을 것 같은데..

    // 나가는 거랑 들어오는 것을 계산해서 하는 방법도 떠오르긴 하는데 어떤 것이 더 좋은 방법인지는 확인해 볼 필요성이 있다고 생각함.

    // 오브젝트를 여러개로 나뉘는 것이 좋다는 판단을 했음

    // 한 오브젝트에서 나간 상태를 다른 오브젝트에서 그렇지 않다는 판단이 필요함
    // stay로 계산하기에는 좋지 못한 것 같음

    // DetectPlayer를 부모로 받는 여러 오브젝트를 생성하고, 이들의 참과 거짓 값을 기준으로 판별하는 식으로 만드는 것이 좋아 보임
    // DetectPlayer를 위한 하위 스크립트 2종.. 내지 1종을 생성
    // 이들의 Trigger에서 Enter Exit를 기준으로 bool값을 반환하고
    // 상위 오브젝트에서 이들을 모두 검사해서 하나라도 bool값이 true라면 true로 반환..

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
