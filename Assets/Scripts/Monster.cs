using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    // Start is called before the first frame update

    private void Awake() {
        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamaged(float Damage)
    {
        Debug.Log("Get Damage");
        curHp -= Damage;
        if(curHp <= 0)
        {
            Destroy(gameObject);
            // Dead();
        }
    }
}
