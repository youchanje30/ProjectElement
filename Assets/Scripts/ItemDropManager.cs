using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager instance;
    [System.Serializable] public class DropBucket
    {
        [Range (1, 10)]
        public int DropWeight;
        [Range (0, 10000)]
        public int MinDropItem;
        [Range (0, 10000)]
        public int MaxDropItem;
    }

    
    [Header("Drop Item Info")]
    public GameObject Coin; // 코인
    public GameObject SpiritSoul; // 정령 정수
    [Space(20f)]

    // 코인 정보
    [Header("Coin Drop Setting")]
    [Range (0, 100)]
    public int CoinDropRate;
    public DropBucket[] CoinBuckets;
    // [Space(30f)]

    public int CoinDrop()
    {
        int DropCoins = 0;
        bool GetCoin = Random.Range(1, 100 + 1) <= CoinDropRate;
        if(!GetCoin) return 0;

        int DropWeights = 0;
        for (int i = 0; i < CoinBuckets.Length; i++)
        {
            DropWeights += CoinBuckets[i].DropWeight;
        }

        int GetBucket = Random.Range(1, DropWeights + 1);
        int curRate = 0;
        for (int i = 0; i < CoinBuckets.Length; i++)
        {
            curRate += CoinBuckets[i].DropWeight;
            if(GetBucket <= curRate)
            {
                DropCoins = Random.Range(CoinBuckets[i].MinDropItem, CoinBuckets[i].MaxDropItem + 1);
                break;
            }
        }

        Debug.Log(DropCoins);
        return DropCoins;
    }

    void Awake()
    {
        instance = this;
    }



}
