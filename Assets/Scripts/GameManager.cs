using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Setting")]
    public bool CanCameraShake;


    [Header("System Panel")]
    [SerializeField] private GameObject SystemPanel;
    [SerializeField] private TalkManager talkManager;
    
    [Header("Setting Panel")]
    [SerializeField] private GameObject SettingPanel;



    [Header("Graphic Setting")]
    [SerializeField] private TMP_Text CameraShakeTxt;


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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {  
        SetResolution();
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            SystemPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }



    public void init()
    {
        CameraShakeTxt.text = CanCameraShake ? "켜짐" : "꺼짐";
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
        SceneManager.LoadScene(Scene[Random.Range(0, Scene.Length)]);
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
                // Active(ObjData.ID);
                isAction = false;
                // 플레이어 상태 변경 해야함
                TalkPanel.SetActive(false);
                break;

            case 3:
                isAction = false;
                // 플레이어 상태 변경 해야함
                TalkPanel.SetActive(false);
                break;
        }
    }
    









    [SerializeField] bool is16v9;
    [SerializeField] bool hasHz;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;

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

    public void DropboxOptionChanged(int x)
    {
        resolutionNum = x;
        Screen.SetResolution(resolutions[resolutionNum].width, 
        resolutions[resolutionNum].height, 
        FullScreenMode.FullScreenWindow);
    }
}