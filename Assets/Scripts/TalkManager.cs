using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, string[]> ActionTalkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        ActionTalkData = new Dictionary<int, string[]>();
        GenerateData();
    }


    void GenerateData()
    {
        talkData.Add(1000, new string[] {"불의 정령", "불의 원소를 사용하는 정령이다"});
        talkData.Add(2000, new string[] { "물의 정령", "물의 원소를 사용하는 정령이다" });
        talkData.Add(3000, new string[] {"땅의 정령", "땅의 원소를 사용하는 정령이다"});
        talkData.Add(4000, new string[] {"바람의 정령", "바람의 원소를 사용하는 정령이다"});       
        talkData.Add(5000, new string[] {"지팡이 테스트", "지팡이 테스트"});

        ActionTalkData.Add(1000, new string[] {"불의 정령 이프리트", "타격 시 낮은 확률로 대상에게 화상 효과를 부여한다."});

        ActionTalkData.Add(2000, new string[] {"물의 정령 운디네", "타격 시 행동을 느리게 한다."});


        ActionTalkData.Add(3000, new string[] {"땅의 정령 노움", "일정 시간 동안 피해를 받지 않으면 보호막을 생성한다."});

        ActionTalkData.Add(4000, new string[] {"바람의 정령 실프", "쿨타임 감소량에 따른 치명타 피해량이 증가한다."});


        ActionTalkData.Add(5000, new string[] {"지팡이 테스트", "지팡이 테스트"});
    }
    

    public string GetTalk(int id, int talkIndex)
    {   
        if(talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }


    public string GetFirstTalk(int id)
    {
        return ActionTalkData[id][Random.Range(0, ActionTalkData[id].Length)];
    }

}
