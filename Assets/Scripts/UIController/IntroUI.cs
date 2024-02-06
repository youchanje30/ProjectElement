using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class IntroUI : MonoBehaviour
{
    public GameObject LoadBtn;
    public GameObject OptionUI;

    void Start()
    {
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            LoadBtn.SetActive(true);
        }
        else
        {
            LoadBtn.SetActive(false);
        }
    }


    void Update()
    {
        
    }


    public void IntroBtn(int num)
    {
        switch (num)
        {
            case 0:
                if (File.Exists(Application.dataPath + "/DataXML.xml"))
                {
                    File.Delete(Application.dataPath + "/DataXML.xml");
                }
                SceneManager.LoadScene("Main Scene");
                break;
            case 1:
                SceneManager.LoadScene("Main Scene");
                break;
            case 2:
                OptionUI.SetActive(true);
                break;
            case 3:
                PlayerPrefs.Save();
                SaveManager.instance.Save();
                Application.Quit();
                break;
        }
    }
}
