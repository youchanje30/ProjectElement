using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening.Core.Easing;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("플레이어 UI")]
    public Image HpFill;
    public GameObject[] ElementalImage;
    public Image SelectImg;
    public Image unSelectImg;
    [Space(20f)]

    [Header("사운드 관련 UI")]
    public Slider bgmSlider;
    public Slider sfxSlider;
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

    [Header("재산 UI")]
    public GameObject WealthUI;
    public TMP_Text Gold;

    [Header("타이머 UI")]
    public GameObject timerUI;
    public TMP_Text TimeTxt;
    public TMP_Text TimerOnOff;
    public int OnOff;

    //[Header("카메라 포인트")]
    //CameraController controller;
    //Camera Camera;
    //Transform Target;
    //public Transform StartPoint;
    //public Transform EndPoint;

    void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);

        GetComponent<Canvas>().worldCamera = Camera.main;
        //Camera = Camera.main;
        //controller = Camera.transform.parent.GetComponent<CameraController>();
        //Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }


     void Start()
    {
        OnOff = 1;
    }
    void Update()
    {
        Gold.text = GameManager.instance.inventory.Gold.ToString();
        timerUI.SetActive(GameManager.instance.isTimer);
        //StartPoint.position = new Vector3(StartPoint.position.x, Target.position.y, StartPoint.position.z);
        //EndPoint.position = new Vector3(EndPoint.position.x, Target.position.y, EndPoint.position.z);
        //EndCamera();
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
    public void timerSetting()
    {
        OnOff++;
        OnOff %= 2;
        TimerOnOff.text = OnOff == 0 ? "켜짐" : "꺼짐";
        //gameManager.SaveSettingData();
    }

    //public void EndCamera()
    //{
    //    if (StartPoint.position.x >= Target.position.x)
    //    {
    //        controller.cinemachineCam.Follow = StartPoint;
    //    }
    //    else if (Target.position.x >= EndPoint.position.x)
    //    {
    //        controller.cinemachineCam.Follow = EndPoint;
    //    }
    //    else
    //    {
    //        controller.cinemachineCam.Follow = Target;
    //    }
    //}

}
