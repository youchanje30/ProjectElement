using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Objname;
    public TextMeshProUGUI Info;
    void Update()
    {
        
    }
    
    public void SetupTooltip(string elename, string eleInfo)
    {
        Objname.text = elename;
        Info.text = eleInfo;
    }
}
