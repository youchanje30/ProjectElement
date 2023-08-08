using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // public static StageManager stageManager;

    public GameObject[] Monsters;
    public GameObject Portal;
    public Transform PortalSpawnPos;
    public int remainMonster;

    void Awake()
    {
        CheckStageMonster();
    }


    public void CheckStageMonster()
    {
        Monsters = GameObject.FindGameObjectsWithTag("Monster");
        remainMonster = Monsters.Length;
    }

    void Clear()
    {
        GameManager.instance.clearStage++;
        SaveManager.instance.Save();
        
        GameObject spawnPortal = Instantiate(Portal);
        spawnPortal.transform.position = PortalSpawnPos.position;
    }

    public void DeadMonster()
    {
        remainMonster--;
        if(remainMonster <= 0)
        {
            Clear();
        }
    }
}
