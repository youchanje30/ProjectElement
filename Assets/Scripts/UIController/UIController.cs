using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;



    [Header("사운드 관련 UI")]
    public Slider bgmSlider;
    public Slider sfxSlider;
    [Space(20f)]

    [Header("체력 관련 UI")]
    public Slider playerHpBar;
    public Image HpFill;
    public Slider BarrierBar;
    public Image BarrierFill;
    [Space(20f)]

    [Header("오브젝트 관련 UI")]
    public ScrollRect buyScrollRect;
    public ScrollRect sellScrollRect;
    [Space(20f)]

    [Header("스탯 강화 UI")]
    public GameObject statusUpdrageUI;
    [Space(20f)]

    [Header("상점 관련 UI")]
    public GameObject shopUI;
    public GameObject buyPanel;
    public GameObject sellPanel;
    [Space(20f)]

    [Header("대화 관련 UI")]
    public GameObject[] talkBtn;
    public GameObject TalkPanel;
    public TMP_Text talkTxt;
    [Space(20f)]

    [Header("시스템 관련 UI")]
    public GameObject SystemPanel;
    public GameObject SettingPanel;
    public TMP_Dropdown resolutionDropdown;
    public GameObject DataResetCheck;
    public TMP_Text CameraShakeTxt;
    public TMP_Text FullScreenTxt;
    
    void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }


    
    void Update()
    {
        
    }

    public void GameSettingBtn(int btn)
    {
        GameManager.instance.GameSettingBtn(btn);
    }

    public void Delete()
    {
        GameManager.instance.Delete();
    }
    public void TalkBtn(int BtnNum)
    {
        GameManager.instance.TalkBtn(BtnNum);
    }

    public void SaveSettingData()
    {
        GameManager.instance.SaveSettingData();
    }
    public void SystemBtn(int BtnNum)
    {
        GameManager.instance.SystemBtn(BtnNum);
    }


    public void ChangeBGMVol()
    {
        AudioManager.instance.ChangeBGMVol();
    }

    public void ChangeSFXVol()
    {
        AudioManager.instance.ChangeSFXVol();
    }

    public void ToggleBGM()
    {
        AudioManager.instance.ToggleBGM();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void DropboxOptionChanged(int x)
    {
        GameManager.instance.DropboxOptionChanged(x);
    }

    public void ChangeViewShop()
    {
        GameManager.instance.ChangeViewShop();
    }

    public void UpgradeStatus(int id)
    {
        GameManager.instance.UpgradeStatus(id);
    }

    public void DataReset(int BtnNum)
    {
        GameManager.instance.DataReset(BtnNum);
    }
}  
