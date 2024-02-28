using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    // public GameObject[] Monsters;
    public List<GameObject> monsters;
    public GameObject Portal;
    public Transform PortalSpawnPos;
    // public int remainMonster;

    void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);


                    
    }


    // public void CheckStageMonster()
    // {
    //     Monsters = GameObject.FindGameObjectsWithTag("Monster");
    //     remainMonster = Monsters.Length;
    // }

    public void AddMonster(GameObject monster)
    {
        monsters.Add(monster);
    }

    public void DeadMonster(GameObject monster)
    {
        monsters.Remove(monster);

        if(monsters.Count <= 0)
            Clear();
    }

    void Clear()
    {
        GameManager.instance.clearStage++;
        SaveManager.instance.Save();
        
        GameObject spawnPortal = Instantiate(Portal);
        spawnPortal.transform.position = PortalSpawnPos.position;
    }
}
