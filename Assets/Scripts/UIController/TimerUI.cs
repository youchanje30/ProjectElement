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
    public TMP_Text TimerOnOff;
    public int OnOff;

    void Awake()
    {
        OnOff = 1;
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
        //gameManager.SaveSettingData();
    }
  
}
