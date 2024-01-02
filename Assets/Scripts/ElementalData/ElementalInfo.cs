using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalInfo : MonoBehaviour
{
    Dictionary<int, string[]> EleInfo;
    Dictionary<int, string[]> EleName;
    private void Awake()
    {
        EleInfo = new Dictionary<int, string[]>(); 
        GenerateData();
    }

    void GenerateData()
    {
        EleName.Add(1000, new string[] { "불의 정령" });
        EleName.Add(2000, new string[] { "물의 정령"});
        EleName.Add(3000, new string[] { "땅의 정령" });
        EleName.Add(4000, new string[] { "바람의 정령" });
        EleInfo.Add(1000, new string[] { "불의 원소를 사용하는 정령이다" });
        EleInfo.Add(2000, new string[] { "물의 원소를 사용하는 정령이다" });
        EleInfo.Add(3000, new string[] { "땅의 원소를 사용하는 정령이다" });
        EleInfo.Add(4000, new string[] { "바람의 원소를 사용하는 정령이다" });

    }

   

}
