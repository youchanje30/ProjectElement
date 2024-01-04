using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Xml.Linq;
using Unity.VisualScripting;
using static ObjectController;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager Item;
    public ElementalManager Elemental;
    public Inventory inventory;
    private InventoryUI inventoryUI;
    public  SwapUI swapUI;

    [SerializeField] private PlayerController player;

    [Header("Game Setting")]
    public bool CanCameraShake;

    [Header("Game Info")]
    public int clearStage;

    [Header("System Panel")]
    [SerializeField] private GameObject SystemPanel;
    [SerializeField] private TalkManager talkManager;   
    [Header("Setting Panel")]
    [SerializeField] private GameObject SettingPanel;



    [Header("Graphic Setting")]
    [SerializeField] private TMP_Text CameraShakeTxt;
    [SerializeField] private TMP_Text FullScreenTxt;


    public string[] Scene;

    private bool timeStop;

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
    public float TimerVal;
    [SerializeField] private TMP_Text TimeTxt;


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
 

    void Awake()
    {
        // Time.timeScale = 0.3f;
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventory = player.GetComponent<Inventory>();
        Item = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        Elemental = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ElementalManager>();
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
        swapUI = GameObject.Find("Canvas").GetComponent<SwapUI>();
    }


    void Start()
    {
        SetResolution();    
        SetNoneItem();
        SaveManager.instance.Load();
        SaveManager.instance.AutoSave();

        if(SceneManager.GetActiveScene().name == "Je_Maintown" || SceneManager.GetActiveScene().name == "Maintown")
        {
            isAction = true;
            isStatusUpgrade = true;
            statusUpgradeTimes = 3;
            statusUpdrageUI.SetActive(true);
        }
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0 && !isAction && !isShop && !isSlotSwap && !isSpiritAwake && !isInven)
        {
            SystemPanel.SetActive(true);
            Time.timeScale = 0f;
        }   
        TimeSetting();
        LogStat();
    }

  
    public void TimeSetting()
    {
        // TimerVal += Time.deltaTime;
        // string Txt = string.Format("{0:D2}:{1:D2}:{2:D3}", TimerVal % 3600, TimerVal % 60 , TimerVal % 1);
        // TimeTxt.text = "Time: " + (int)TimerVal / 1; // Txt; // 
    } 

    public void init()
    {
        CameraShakeTxt.text = CanCameraShake ? "켜짐" : "꺼짐";
        FullScreenTxt.text = fullScreen == 0 ? "전체 화면" : "창모드";
    }
    
    public void GameSettingBtn(int BtnNum)
    {
        switch (BtnNum)
        {
            case 1: 
                
                break;

            case 2: 

                break;
            // 해상도 다운 업

            case 3: 
                fullScreen ++;
                fullScreen %= 2;
                FullScreenTxt.text = fullScreen == 0 ? "전체 화면" : "창모드";
                SaveSettingData();
                break;
            // 화면 비율 (전체 화면 변경)

            case 4: //카메라 흔들림
                CanCameraShake = !CanCameraShake;
                CameraShakeTxt.text = CanCameraShake ? "켜짐" : "꺼짐";
                SaveSettingData();
                break;
            //카메라 흔들림

        }
    }


    public void SaveSettingData()
    {
        
    }


    public void SystemBtn(int BtnNum)
    {
        switch (BtnNum)
        {
            case 1: //Resume Btn
                SystemPanel.SetActive(false);
                SettingPanel.SetActive(false);
                GraphicSetting();
                Time.timeScale = 1f;
                break;

            case 2: //Reset Btn

                break;

            case 3: // Setting Btn
                SystemPanel.SetActive(false);
                SettingPanel.SetActive(true);

                break;

            case 4: //Exit Btn
                Application.Quit();
                break;

        }
    }

    public void RandomStageRoad()
    {
        SaveManager.instance.Save();
        SceneManager.LoadScene(Scene[Random.Range(0, Scene.Length)]);
    }

    public void MainstageRoad()
    {
        SaveManager.instance.Save();
        SceneManager.LoadScene("Maintown");
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


    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private int fullScreen = 0 ; //0 전체화면 , 1 창모드

    List<Resolution> resolutions;
    private int resolutionNum;
    
    public void SetResolution()
    {
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.Reverse(); // 높은 것 부터 표시

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach(Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + " x " + item.height + "  " + item.refreshRateRatio + "hz";
            resolutionDropdown.options.Add(option);
            if(item.width == Screen.width && item.height == Screen.height)
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
    public void OpenInventory()
    {
        inventoryUI.SetCard();
        inventoryUI.SetItem();
        isInven = !isInven;
        inventoryUI.InvenUI.SetActive(isInven);
    }
    public void DropboxOptionChanged(int x)
    {
        resolutionNum = x; 
    }

    public void GraphicSetting()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, 
        resolutions[resolutionNum].height, 
        fullScreen == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }
    //public void EleSwap()
    //{
    //    Elements[swapUI.slot].SetActive(true);
    //    inventory.HavingElement[swapUI.slot] = ElementalManager.instance.AddElement((int)ObjData.WeaponType * 1000);
    //    player.SetEquipment();
    //    swapUI.SlotSwapUI.SetActive(false);
    //    TalkPanel.SetActive(false);
    //    isAction = false;
    //    isSelected = false;
    //    isSlotSwap = false;
    //}
    public void LogStat()
    {
        inventoryUI.Stat.text = "힘: " + player.GetComponent<PlayerStatus>().strength + "\n" + "민첩: " + player.GetComponent<PlayerStatus>().dexterity + "\n" + "운: " + player.GetComponent<PlayerStatus>().luck ;
    }

    public void SetNoneItem()
    {
        for (int i = 0; i < player.inventory.HasWeapon.Length; i++)
        {
            player.inventory.HavingElement[i] = Elemental.AddElement(0);
        }
        for (int i = 0; i < inventory.HavingItem.Length; i++)
        {
            player.inventory.HavingItem[i] = Item.AddItem(0);
        }
    }
}