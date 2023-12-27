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
        talkData.Add(2000, new string[] {"땅의 정령", "땅의 원소를 사용하는 정령이다"});
        talkData.Add(3000, new string[] {"바람의 정령", "바람의 원소를 사용하는 정령이다"});
        talkData.Add(4000, new string[] {"물의 정령", "물의 원소를 사용하는 정령이다"});
        talkData.Add(5000, new string[] {"지팡이 테스트", "지팡이 테스트"});

        ActionTalkData.Add(1000, new string[] {"나는 불의 정령 이프립트라고 해", "너 혹시 불 좋아하니?"
        , "불은 정말 최고야!", "불의 정령의 모습에 불만이라도 있어?"});
        
        ActionTalkData.Add(2000, new string[] {"나는 땅의 정령 노움라고 해", "너 혹시 땅 좋아하니?"
        , "땅은 정말 최고야!", "땅의 정령의 모습에 불만이라도 있어?"});

        ActionTalkData.Add(3000, new string[] {"나는 바람의 정령 실프라고 해", "너 혹시 바람 좋아하니?"
        , "바람은 정말 최고야!", "바람의 정령의 모습에 불만이라도 있어?"});

        ActionTalkData.Add(4000, new string[] {"나는 물의 정령 운디네라고 해", "너 혹시 물 좋아하니?"
        , "물은 정말 최고야!", "물의 정령의 모습에 불만이라도 있어?"});

        ActionTalkData.Add(5000, new string[] {"지팡이 테스트", "지팡이 테스트"
        , "지팡이 테스트", "지팡이 테스트"});
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
