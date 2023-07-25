using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] Monsters;
    public GameObject Portal;
    public Transform PortalSpawnPos;

    public void CheckStageMonster()
    {
        Monsters = GameObject.FindGameObjectsWithTag("Monster");
        if(Monsters.Length == 0)
        {
            Clear();
        }
    }

    void Clear()
    {
        GameObject spawnPortal = Instantiate(Portal);
        spawnPortal.transform.position = PortalSpawnPos.position;
    }
}
