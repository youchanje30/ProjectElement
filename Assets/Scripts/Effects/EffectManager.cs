using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effect { Fire, South}

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject[] effectPrefabs;
    public List<GameObject>[] effectPools;


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

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnEffect(Vector3 spawnPos, Effect effect, Vector2 size)
    {
        GameObject ef = null;

        foreach (var item in effectPools[(int)effect])
        {
            if(!item.activeSelf)
            {
                ef = item;
                break;
            }
        }

        if(!ef)
        {
            ef = Instantiate(effectPrefabs[(int)effect], transform);
            effectPools[(int)effect].Add(ef);
        }
        ef.GetComponent<EffectController>().size = size;
        ef.transform.position = spawnPos;
        ef.SetActive(true);
    }
}
