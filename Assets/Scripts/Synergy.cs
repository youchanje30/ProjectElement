using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy : MonoBehaviour
{
    [SerializeField] private Battle battle;

    void Start()
    {
        if (!battle)
            battle = GetComponent<Battle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveSynergy(WeaponTypes type, MonsterSynergy monster = null)
    {
         monster.GetSynergy(type);
    }
}
