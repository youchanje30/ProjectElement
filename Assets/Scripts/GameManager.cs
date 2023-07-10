using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    [Header("System Panel")]
    [SerializeField] private GameObject SystemPanel;



    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Time.timeScale = 0f;


    }

    // public void 

}