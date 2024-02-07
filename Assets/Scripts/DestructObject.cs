using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// public enum DestructTypes
// {
//     Chest

// }


public class DestructObject : MonoBehaviour
{
    // public DestructTypes DestructType;

    public GameObject coin;
    [SerializeField] bool canDropCoin;

    public void DestroyObj()
    {
        if(canDropCoin)
            BreakChest();
        else
            Destroy(gameObject);


        // switch(DestructType)
        // {
        //     case DestructTypes.Chest:
        //         BreakChest();
        //         break;



        //     default:
        //         break;
        // }
    }

    public void DeadMonster()
    {

    }

    public void BreakChest()
    {
        coin = ItemDropManager.instance.Coin;
        for (int i = 0; i < ItemDropManager.instance.CoinDrop(); i++)
        {
            SpawnCoin();
        }
        Destroy(gameObject);
    }


    public void SpawnCoin()
    {
        GameObject spawn = Instantiate(coin);
        spawn.transform.position = transform.position;
    }



}