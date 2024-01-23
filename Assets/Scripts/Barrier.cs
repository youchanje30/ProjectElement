using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject barrier;
    private PassiveSystem passiveSystem;
    private PlayerStatus status;

    void Awake()
    {
        passiveSystem = GetComponent<PassiveSystem>();
        status = GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        barrier.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, (status.barrier/ (status.maxHp * passiveSystem.shieldPer * 0.01f )));
    }
}
