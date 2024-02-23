using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpanwer : MonoBehaviour
{
    [SerializeField] GameObject bossObj;
    [SerializeField] Transform target;
    [SerializeField] bool isSpawn;
    [SerializeField] float spawnDistance;

    void Awake()
    {
        bossObj.SetActive(false);
        isSpawn = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        Check();
    }

    void Check()
    {
        if(isSpawn) return;

        if(Vector2.Distance(target.position, bossObj.transform.position) <= spawnDistance)
        {
            bossObj.SetActive(true);
            isSpawn = true;
            return;
        }
        else
            Invoke("Check", 0.1f);
    }
}
