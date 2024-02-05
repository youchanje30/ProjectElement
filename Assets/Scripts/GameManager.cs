using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Xml.Linq;
using Unity.VisualScripting;
using static ObjectController;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager Item;
    public ElementalManager Elemental;
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public SwapUI swapUI;
    public AudioManager audioManager;
    public Battle battle;

    [SerializeField] private PlayerController player;

    [Header("Game Info")]
    public int clearStage;

    [Header("System Panel")]
    [SerializeField] private GameObject SystemPanel;
    [SerializeField] private TalkManager talkManager;
    [SerializeField] private bool isSystem;

    [Header("Setting Panel")]
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private GameObject DataResetCheck;

    [Header("Graphic Setting")]
    [SerializeField] private TMP_Text CameraShakeTxt;
    [SerializeField] private TMP_Text FullScreenTxt;


    public string[] Scene;

    //private bool timeStop;

    public int Chapter; //챕터 정보

    [Header("Talk Panel")]
    public GameObject[] talkBtn;
    public GameObject TalkPanel;
    public TMP_Text talkTxt;
    public int talkIndex;

    public bool isAction;
    public bool isSelected;

    public ObjectController ObjData;

    [Header("Timer Setting")]
    //[SerializeField] private TimerUI timeUI;
    public float TimerVal;
    public bool isTimer = false;

    [Header("Shop Setting")]
    public GameObject ShopUI;
    public bool isShop = false;
    public bool viewBuy = true;
    public GameObject buyPanel;
    public GameObject sellPanel;

    [Header("SpiritAwake Setting")]
    public GameObject SpiritAwakeUI;
    public bool isSpiritAwake = false;

    [Header("Status Updrage Setting")]
    public GameObject statusUpdrageUI;
    public bool isStatusUpgrade = false;
    public int statusUpgradeTimes = 0;

    [Header("SlotSwap Setting")]
    public bool isSlotSwap = false;

    [Header("Inventory Setting")]
    public bool isInven = false;

    [Header("Elemental")]
    public GameObject[] Elements;
    public GameObject[] ElementImg;

    void Awake()
    {
        TimerVal = 0;
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventory = player.GetComponent<Inventory>();
        Item = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        Elemental = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ElementalManager>();
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
        swapUI = GameObject.Find("Canvas").GetComponent<SwapUI>();
        battle = player.GetComponent <Battle>();
        // timeUI = GameObject.Find("Canvas").GetComponent<TimerUI>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        talkManager  = GameObject.Find("Talk Manager").GetComponent<TalkManager>();
        Screen.SetResolution(1920,1080,0);
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        statusUpdrageUI = UIController.instance.statusUpdrageUI;

        ShopUI = UIController.instance.shopUI;
        buyPanel = UIController.instance.buyPanel;
        sellPanel = UIController.instance.sellPanel;

        talkBtn = UIController.instance.talkBtn;
        TalkPanel = UIController.instance.TalkPanel;
        talkTxt = UIController.instance.talkTxt;

        
        SystemPanel = UIController.instance.SystemPanel;
        SettingPanel = UIController.instance.SettingPanel;
        resolutionDropdown = UIController.instance.resolutionDropdown;
        DataResetCheck = UIController.instance.DataResetCheck;
        CameraShakeTxt = UIController.instance.CameraShakeTxt;
        FullScreenTxt = UIController.instance.FullScreenTxt;
        for (int i = 0; i < ElementImg.Length; i++)
        {
            ElementImg[i] = UIController.instance.ElementalImage[i];
        }
        // if (PlayerPrefs.HasKey("FullScreenData")) { SetSettingData();}
        
        SetResolution();

        for (int i = 0; i < player.inventory.HasWeapon.Length; i++)
        {
            player.inventory.HavingElement[i] = Elemental.AddElement(0);
        }
        

        SaveManager.instance.Load();
        SaveManager.instance.AutoSave();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            isAction = true;
            isStatusUpgrade = true;
            statusUpgradeTimes = 3;
            statusUpdrageUI.SetActive(true);
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0 && !isAction && !isShop && !isSlotSwap && !isSpiritAwake && !isInven && !isSystem)
        {
            Invoke("OpenSystem", 0.01f);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f && isSystem)
        {
            isSystem = !isSystem;
            SystemPanel.SetActive(isSystem);
            SettingPanel.SetActive(false);
            //GraphicSetting();
            SetSettingData();
            Time.timeScale = 1f;
        }
        if (isStatusUpgrade)
        {
            isInven = false;
            inventoryUI.InvenUI.SetActive(false);
        }
        TimerSetting();
        LogStat();
        SetImage();
    }

    public void OpenSystem()
    {
        isSystem = !isSystem;
        SystemPanel.SetActive(isSystem);
        Time.timeScale = 0f;
    }

    public void TimerSetting()
    {
        TimerVal += Time.deltaTime;
        UIController.instance.TimeTxt.text = ((int)TimerVal / 3600).ToString("D2") + ":" + ((int)TimerVal / 60 % 60).ToString("D2") + ":" + ((int)TimerVal % 60).ToString("D2");
        //(TimerVal / 3600).ToString("D2") + ":" + (TimerVal / 60 % 60).ToString("D2") + ":" + (TimerVal % 60).ToString("D2");
        // TimerVal += Time.deltaTime;
        // string Txt = string.Format("{0:D2}:{1:D2}:{2:D3}", TimerVal % 3600, TimerVal % 60 , TimerVal % 1);
        // TimeTxt.text = "Time: " + (int)TimerVal / 1; // Txt; // 
    }
    
    public void OptionSetting()
    {
        FullScreenTxt.text = PlayerPrefs.GetInt("FullScreenData") == 0 ? "전체 화면" : "창모드";
        //UIController.instance.TimerOnOff.text = PlayerPrefs.GetInt("TimerData") == 0 ? "켜짐" : "꺼짐";
        audioManager.ChangeBGMVol();
        audioManager.ChangeSFXVol();
    }


    //public void init()
    //{
    //    CameraShakeTxt.text = CanCameraShake ? "켜짐" : "꺼짐";
    //    FullScreenTxt.text = fullScreen == 0 ? "전체 화면" : "창모드";
    //}

    public void GameSettingBtn(int BtnNum)
    {
        switch (BtnNum)
        {
            case 1:
                resolutionDropdown.value++;

                break;

            case 2:
                resolutionDropdown.value--;

                break;
            // 해상도 다운 업

            case 3:
                fullScreen++;
                fullScreen %= 2;
                FullScreenTxt.text = fullScreen == 0 ? "전체 화면" : "창모드";

                break;
            // 화면 비율 (전체 화면 변경)

            case 4: //카메라 흔들림
                //if (CameraShakeOnOff == 0) { CanCameraShake = true; }
                //else { CanCameraShake = false; }
                break;
            //카메라 흔들림
            case 5: // 데이터 초기화
               DataResetCheck.SetActive(true);
                break;
        }
    }
    public void Delete()
    {
        if (File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            File.Delete(Application.dataPath + "/DataXML.xml");
        }
    }

    public void SaveSettingData()
    {
        PlayerPrefs.SetInt("ResolutionData", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullScreenData", fullScreen);
        PlayerPrefs.SetInt("TimerData", UIController.instance.OnOff);
        PlayerPrefs.SetFloat("BGMData", audioManager.BGMVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXData", audioManager.SFXVolumeSlider.value);
    }
    public void SetSettingData()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionData");
        fullScreen = PlayerPrefs.GetInt("FullScreenData");
        isTimer = UIController.instance.OnOff == 0 ? true : false;
        audioManager.BGMVolumeSlider.value = PlayerPrefs.GetFloat("BGMData");
        audioManager.SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXData");
    }

    public void SystemBtn(int BtnNum)
    {
        switch (BtnNum)
        {
            case 1: //Resume Btn
                SystemPanel.SetActive(false);
                SettingPanel.SetActive(false);
                GraphicSetting();
                isSystem = !isSystem;
                Time.timeScale = 1f;
                break;

            case 2: //Reset Btn
                SaveManager.instance.ResetData();
                SceneManager.LoadScene("Main Scene");
                Time.timeScale = 1f;
                break;

            case 3: // Setting Btn
                SystemPanel.SetActive(false);
                SettingPanel.SetActive(true);
                OptionSetting();
                SetResolution();
                break;

            case 4: //Exit Btn
                PlayerPrefs.Save();
                SaveManager.instance.Save();
                Application.Quit();
                break;

            case 5: //Setting Resume Btn
                SettingPanel.SetActive(false);
                SystemPanel.SetActive(true);
                GraphicSetting();
                SaveSettingData();
                OptionSetting();
                SetSettingData();
                break;

            case 6: //Setting Exit Btn
                SettingPanel.SetActive(false);
                SystemPanel.SetActive(true);
                SetSettingData();
                break;
        }
    }

    public void RandomStageRoad()
    {
        SaveManager.instance.Save();
        if(clearStage > 0 && clearStage % 5 == 0)
            SceneManager.LoadScene("BossStage");
        else
            SceneManager.LoadScene(Scene[Random.Range(0, Scene.Length)]);
    }

    public void MainstageRoad()
    {
        SaveManager.instance.Save();
        SceneManager.LoadScene("Main Scene");
    }



    public void Talk(int id)
    {
        string talkingData = talkManager.GetTalk(id, talkIndex);

        if(talkingData == null)
        {
            isAction = false;
            TalkPanel.SetActive(false);
            talkIndex = 0;
            
            
            return;
        }

        talkTxt.text = talkingData;
        talkIndex++;
        isAction = true;

    }

    public void ViewTalkPanel(int id)
    {
        for(int i = 0; i < talkBtn.Length; i++)
        {
            talkBtn[i].SetActive(true);
        }

        string firstTalkData = talkManager.GetFirstTalk(id);
        talkTxt.text = firstTalkData;
        isSelected = true;
        TalkPanel.SetActive(true);
    }
    

    public void Action(GameObject Obj)
    {
        ObjData = Obj.GetComponent<ObjectController>();
        // Debug.Log("Manager Action");

        if(isAction && !isSelected)
        {
            Talk(ObjData.objectID);
            // Debug.Log("Talk Work");
        }
        else if(!isAction && !isSelected)
        {
            ViewTalkPanel(ObjData.objectID);
            isAction = true;
            // Debug.Log("ViewTalk Work");
        }
    }

    public void ChangeViewShop()
    {
        viewBuy = !viewBuy;
        buyPanel.SetActive(viewBuy);
        sellPanel.SetActive(!viewBuy);
    }

    public void DataReset(int BtnNum)
    {
        switch (BtnNum)
        {
            case 0:
                Delete();
                SceneManager.LoadScene("Main Scene");
                Time.timeScale = 1f;
                break;
            case 1:
                DataResetCheck.SetActive(false);
                 break;

        }
    }

    public void TalkBtn(int BtnNum)
    {
        isSelected = false;
        switch (BtnNum)
        {
            case 1:
                Talk(ObjData.objectID);
                talkBtn[1].SetActive(false);
                talkBtn[2].SetActive(false);
                // Debug.Log("Talk Start");
                break;
            
            case 2: //
                //정령 선택 등의 선택기능
                isSelected = true;
                Active(ObjData.objectID);                
                isAction = false;
                TalkPanel.SetActive(false);
                
                // 플레이어 상태 변경 해야함              
                break;

            case 3:
                isAction = false;
                // 플레이어 상태 변경 해야함
                TalkPanel.SetActive(false);
                break;
        }

    }
    
      
   
    public void Active(int objectID) //정령 2번 선택지
    {
        switch (objectID)
        {
            case 1000:
                // player.GetElement((int)WeaponTypes.Sword,(int)Elements.Fire);          
                player.GetElement((int)WeaponTypes.Sword);
                break;

            case 2000:
                // player.GetElement((int)WeaponTypes.Sword,(int)Elements.Fire);          
                player.GetElement((int)WeaponTypes.Wand);

                break;
    
            case 3000:
                // player.GetElement((int)WeaponTypes.Sword,(int)Elements.Fire);          
                player.GetElement((int)WeaponTypes.Shield);
                break;
                
            case 4000:
                // player.GetElement((int)WeaponTypes.Sword,(int)Elements.Fire);          
                player.GetElement((int)WeaponTypes.Bow);
                break;
        }
    }


    public void UpgradeStatus(int id)
    {
        switch (id)
        {
            case 0:
                player.GetComponent<PlayerStatus>().strength++;
                break;
            case 1:
                player.GetComponent<PlayerStatus>().dexterity++;
                break;
            case 2:
                player.GetComponent<PlayerStatus>().luck++;
                break;
        }

        statusUpgradeTimes --;
        Debug.Log("Upgrade Status");

        if(statusUpgradeTimes <= 0)
        {
            player.SetEquipment();
            isStatusUpgrade = false;
            isAction = false;
            statusUpdrageUI.SetActive(false);
        }
    }


 
    [SerializeField] private int fullScreen = 0 ; //0 전체화면 , 1 창모드

    List<Resolution> resolutions;
    List<Resolution> checkedResolutions;
    private int resolutionNum;
    
    public void SetResolution()
    {

        resolutions = new List<Resolution>(Screen.resolutions);
        checkedResolutions = new List<Resolution>();
        resolutions.Reverse(); // 높은 것 부터 표시

        resolutionDropdown.options.Clear();
        
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            //option.text = item.width + " x " + item.height + "  " + item.refreshRateRatio + "hz";
           option.text = item.width + " x " + item.height + "  ";
           if(item.width <= 1920 && item.height <=1080 && item.refreshRateRatio.value == 60)
            {
                resolutionDropdown.options.Add(option);
                checkedResolutions.Add(item);
            }
            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;                      
        }

    }

    public void OpenSwap()
    {
        swapUI.ElementImg();    
        isSlotSwap = true;
        swapUI.SlotSwapUI.SetActive(isSlotSwap);
    }

    public void DropboxOptionChanged(int x)
    {
        resolutionNum = x; 
    }

    public void GraphicSetting()
    {
        // Screen.SetResolution(resolutions[resolutionNum].width, 
        // resolutions[resolutionNum].height, 
        // fullScreen == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        Screen.SetResolution(checkedResolutions[resolutionNum].width, 
        checkedResolutions[resolutionNum].height, 
        fullScreen == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    public void LogStat()
    {
        inventoryUI.Stat.text = "힘: " + player.GetComponent<PlayerStatus>().strength + "\n" + "민첩: " + player.GetComponent<PlayerStatus>().dexterity + "\n" + "운: " + player.GetComponent<PlayerStatus>().luck + "\n" + "\n" + "최대 체력: " + player.GetComponent<PlayerStatus>().maxHp + "\n" + "데미지: " + player.GetComponent<PlayerStatus>().damage + "\n" + "크리티컬 확률: " + player.GetComponent<PlayerStatus>().crtRate + "\n" + "공격 속도: " + player.GetComponent<PlayerStatus>().atkSpeed + "\n" + "회피 확률: " + player.GetComponent<PlayerStatus>().missRate + "\n" + "쿨타임 감소율: " + player.GetComponent<PlayerStatus>().coolDownReductionPer + "\n" + "데미지 감소율: " + player.GetComponent<PlayerStatus>().defPer + "\n" + "이동 속도: " + player.GetComponent<PlayerStatus>().playerSpeed + "\n" + "점프력: " + player.GetComponent<PlayerStatus>().jumpForce;
    }
    public void SetImage()
    {
        for (int i = 0; i < ElementImg.Length; i++)
        {
            if (inventory.HavingElement[i].ElementalID != 0)
            {          
                Color color = ElementImg[i].GetComponent<Image>().color;
                color.a = 1f;
                ElementImg[i].GetComponent<Image>().color = color;
            }
            else
            {
                Color color = ElementImg[i].GetComponent<Image>().color;
                color.a = 0f;
                ElementImg[i].GetComponent<Image>().color = color;
            }
            if(inventory.HavingElement[i].WeaponTypes == battle.WeaponType && inventory.HavingElement[i].ElementalID != 0)
            {
                ElementImg[i].transform.parent.GetComponent<Image>().sprite = UIController.instance.SelectImg.sprite;
                ElementImg[i].GetComponent<Image>().sprite = inventory.HavingElement[i].elementalIcon;
            }
            else
            {
                ElementImg[i].transform.parent.GetComponent<Image>().sprite = UIController.instance.unSelectImg.sprite;
                ElementImg[i].GetComponent<Image>().sprite = inventory.HavingElement[i].UnSelcIcon;
            }
        }
    }
}