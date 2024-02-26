using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillEffect { Fire, South }
public enum AtkEffect { Fire = 2, Water, South, Wind , Crt }
public enum BossEffect { charge = 7, handDown, fallDownFront, fallDownBack }
public enum HandAtk { one = 11, two, three, four }
public enum MonsterEffect { poison = 15 }

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject[] effectPrefabs;
    public List<GameObject>[] effectPools;

    public bool isCrt;


    void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);

        effectPools = new List<GameObject>[effectPrefabs.Length];
        for (int i = 0; i < effectPools.Length; i++)
        {
            effectPools[i] = new List<GameObject>();
        }

    }

    void Update()
    {
        
    }


    public void SpawnEffect(Vector3 spawnPos, int effect, Vector2 size, bool reverse = false)
    {
        GameObject ef = null;

        foreach (var item in effectPools[effect])
        {
            if(!item.activeSelf)
            {
                ef = item;
                break;
            }
        }

        if(!ef)
        {
            ef = Instantiate(effectPrefabs[effect], transform);
            effectPools[effect].Add(ef);
        }

        if(ef.GetComponent<EffectController>().isSizeFix)
            ef.GetComponent<EffectController>().size = size;

        if(reverse)
            ef.transform.localScale = new Vector3(Mathf.Abs(ef.transform.localScale.x) * -1, ef.transform.localScale.y, ef.transform.localScale.z);
        else
            ef.transform.localScale = new Vector3(Mathf.Abs(ef.transform.localScale.x), ef.transform.localScale.y, ef.transform.localScale.z);
        

        if(isCrt && effect >= 2 && effect < 7)
        {
            isCrt = false;
            SpawnEffect(spawnPos, (int)AtkEffect.Crt, size);
        }

        ef.transform.position = spawnPos;
        ef.SetActive(true);
    }
}
