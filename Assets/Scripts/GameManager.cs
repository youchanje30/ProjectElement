using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{


    [Header("System Panel")]
    [SerializeField] private GameObject SystemPanel;
    [SerializeField] private TalkManager talkManager;
    

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

    void Start()
    {
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.)
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            SystemPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }


    public void SystemBtn(int BtnNum)
    {
        switch (BtnNum)
        {
            case 1: //Resume Btn
                SystemPanel.SetActive(false);
                Time.timeScale = 1f;
                break;

            case 2: //Exit Btn
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
            
            case 2:
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
    

}