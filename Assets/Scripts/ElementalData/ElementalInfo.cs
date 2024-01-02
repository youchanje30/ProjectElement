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
        EleName.Add(1000, new string[] { "���� ����" });
        EleName.Add(2000, new string[] { "���� ����"});
        EleName.Add(3000, new string[] { "���� ����" });
        EleName.Add(4000, new string[] { "�ٶ��� ����" });
        EleInfo.Add(1000, new string[] { "���� ���Ҹ� ����ϴ� �����̴�" });
        EleInfo.Add(2000, new string[] { "���� ���Ҹ� ����ϴ� �����̴�" });
        EleInfo.Add(3000, new string[] { "���� ���Ҹ� ����ϴ� �����̴�" });
        EleInfo.Add(4000, new string[] { "�ٶ��� ���Ҹ� ����ϴ� �����̴�" });

    }

   

}
