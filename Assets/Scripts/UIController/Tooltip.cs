using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Elementalname;
    public TextMeshProUGUI ElementalInfo;
    void Update()
    {
        
    }
    
    public void SetupTooltip(string elename, string eleInfo)
    {
        Elementalname.text = elename;
        ElementalInfo.text = eleInfo;
    }
}
