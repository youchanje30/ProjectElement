using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DestructTypes
{
    Chest

}


public class DestructObject : MonoBehaviour
{
    public DestructTypes DestructType;

    public GameObject coin;

    public void DestroyObj()
    {

        switch(DestructType){
            case DestructTypes.Chest:
                BreakChest();
                break;



        }
    }

    public void BreakChest()
    {
        for (int i = 0; i < Random.Range(3,7 + 1); i++)
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