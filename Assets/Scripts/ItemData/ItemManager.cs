using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private List<ItemData> ItemDatas;
    [SerializeField] private GameObject ItemPrefab;

    
    void Awake()
    {
        instance = this;
    }

    

}
