using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject timerUI;
    public TMP_Text TimeTxt;
    [SerializeField] TMP_Text TimerOnOff;
    [SerializeField] private int OnOff = 1;

    void Awake()
    {       
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        timerUI.SetActive(gameManager.isTimer);
    }
    public void timerSetting()
    {
        OnOff++;
        OnOff %= 2;
        TimerOnOff.text = OnOff == 0 ? "ÄÑÁü" : "²¨Áü";
        
        if(OnOff == 0)
        {
            gameManager.isTimer = true;
        }
        else
        {
            gameManager.isTimer = false;
        }
    }
  
}
