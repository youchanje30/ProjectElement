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
    public GameObject[] statusUpButton;
    public GameObject statusUpPoint;
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
    public GameObject[] SystemButton;
    public GameObject[] SettingButton;
    public GameObject[] TalkButton;
    public GameObject SystemPoint;
    public GameObject SettingPoint;
    public GameObject TalkPoint;
    public int Slot;
    public int settingslot;


    [Header("재산 UI")]
    public GameObject WealthUI;
    public TMP_Text Gold;

    [Header("타이머 UI")]
    public GameObject timerUI;
    public TMP_Text TimeTxt;
    public TMP_Text TimerOnOff;
    public int OnOff;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        GetComponent<Canvas>().worldCamera = Camera.main;
        OnOff = 1;
    }
    void Update()
    {
        Gold.text = GameManager.instance.inventory.Gold.ToString();
        timerUI.SetActive(OnOff == 0);
        if (SystemPanel.activeSelf == true && SettingPanel.activeSelf == false)
        {
            SystemKeyboardCon();
           
        }
        else if(SettingPanel.activeSelf == true)
        {
            SettingKeyboardCon();

        }
        else if(TalkPanel.activeSelf == true)
        {
            Invoke("TalkKeyboardCon", 0.05f);

        }
        else if(statusUpdrageUI.activeSelf == true)
        {
            Invoke("StatusUPKeyboardCon", 0.05f);
        }
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
    public void SystemKeyboardCon()
    {
       
        if (Input.GetKeyDown(KeyCode.UpArrow) && Slot != 0)
        {
            SystemPoint.transform.position = new Vector3(SystemPoint.transform.position.x, SystemButton[Slot - 1].transform.position.y);
                    Slot -= 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && Slot != 3)
        {
            SystemPoint.transform.position = new Vector3(SystemPoint.transform.position.x, SystemButton[Slot + 1].transform.position.y);
                    Slot += 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.SystemBtn(Slot + 1);
           
        }
    }
    public void SettingKeyboardCon()
    {
        if (DataResetCheck.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && settingslot != 10)
            {
                if (settingslot == 0 || settingslot == 2 || settingslot == 7)
                {
                    SettingPoint.transform.position = new Vector3(SettingButton[settingslot + 1].transform.position.x + 40, SettingButton[settingslot + 1].transform.position.y);
                    settingslot += 1;

                }
                else if (settingslot == 3 || settingslot == 4 || settingslot == 5 || settingslot == 8 || settingslot == 9)
                {
                    SettingPoint.transform.position = new Vector3(SettingButton[settingslot + 1].transform.position.x - 120, SettingButton[settingslot + 1].transform.position.y);
                    settingslot += 1;
                }
                else
                {
                    SettingPoint.transform.position = new Vector3(SettingButton[settingslot + 1].transform.position.x - 40, SettingButton[settingslot + 1].transform.position.y);
                    settingslot += 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && settingslot != 0)
            {
                if (settingslot == 2 || settingslot == 4 || settingslot == 9)
                {
                    SettingPoint.transform.position = new Vector3(SettingButton[settingslot - 1].transform.position.x + 40, SettingButton[settingslot - 1].transform.position.y);
                    settingslot -= 1;
                }
                else if (settingslot == 5 || settingslot == 6 || settingslot == 7 || settingslot == 10)
                {
                    SettingPoint.transform.position = new Vector3(SettingButton[settingslot - 1].transform.position.x - 120, SettingButton[settingslot - 1].transform.position.y);
                    settingslot -= 1;
                }
                else
                {
                    SettingPoint.transform.position = new Vector3(SettingButton[settingslot - 1].transform.position.x - 40, SettingButton[settingslot - 1].transform.position.y);
                    settingslot -= 1;
                }
            }         
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && settingslot != 12)
            {
                SettingPoint.transform.position = new Vector3(SettingButton[12].transform.position.x - 150, SettingButton[12].transform.position.y);
                settingslot  = 12;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && settingslot != 11)
            {
                SettingPoint.transform.position = new Vector3(SettingButton[11].transform.position.x - 150, SettingButton[11].transform.position.y);
                settingslot = 11;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (settingslot)
            {
                case 0:
                    GameManager.instance.GameSettingBtn(1);
                    break;
                case 1:
                    GameManager.instance.GameSettingBtn(2);
                    break;
                case 2:
                case 3:
                    GameManager.instance.GameSettingBtn(3);
                    break;
                case 6:
                    GameManager.instance.GameSettingBtn(5);
                    break;
                case 7:
                case 8:
                    timerSetting();
                    break;
                case 9:
                    GameManager.instance.SystemBtn(5);
                    break;
                case 10:
                    GameManager.instance.SystemBtn(6);
                    break;
                case 11:
                    GameManager.instance.DataReset(0);
                    break;
                case 12:
                    GameManager.instance.DataReset(1);
                    SettingPoint.transform.position = new Vector3(SettingButton[6].transform.position.x - 120, SettingButton[6].transform.position.y);
                    settingslot = 6;
                    break;
            }
   
        }
    }
    public void TalkKeyboardCon()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && Slot != 0 && GameManager.instance.talkBtn[1].activeSelf == true && GameManager.instance.talkBtn[2].activeSelf == true)
        {
            TalkPoint.transform.position = new Vector3(TalkPoint.transform.position.x, TalkButton[Slot - 1].transform.position.y);
            Slot -= 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && Slot != 2 && GameManager.instance.talkBtn[1].activeSelf == true && GameManager.instance.talkBtn[2].activeSelf == true)
        {
            TalkPoint.transform.position = new Vector3(TalkPoint.transform.position.x, TalkButton[Slot + 1].transform.position.y);
            Slot += 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Invoke("activeTalk", 0.1f);
        }
    }
    public void StatusUPKeyboardCon()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && Slot != 2)
        {
            statusUpPoint.transform.position = new Vector3(statusUpButton[Slot + 1].transform.position.x, statusUpButton[Slot + 1].transform.position.y + 40);
            Slot += 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && Slot != 0)
        {
            statusUpPoint.transform.position = new Vector3(statusUpButton[Slot - 1].transform.position.x, statusUpButton[Slot + 1].transform.position.y + 40);
            Slot -= 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Invoke("Upgradestatus", 0.05f);
        }
    }
    public void activeTalk()
    {
        GameManager.instance.TalkBtn(Slot + 1);
    }
    public void Upgradestatus()
    {
        GameManager.instance.UpgradeStatus(Slot);
    }

}
