using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Objname;
    public TextMeshProUGUI Info;
    public Image EleImg;
    void Update()
    {
        
    }
    
    public void SetupTooltip(string elename, string eleInfo, Sprite image)
    {
        Objname.text = elename;
        Info.text = eleInfo;
        EleImg.sprite = image;
    }
    public void SetupItemTooltip(string elename, string eleInfo, Image image)
    {
        Objname.text = elename;
        Info.text = eleInfo;
        EleImg = image;
    }
}
