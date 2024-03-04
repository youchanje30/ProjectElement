using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleForm : MonoBehaviour
{
    
    void OnApplicationQuit()
    {
        Application.OpenURL("https://forms.gle/beA6y7k26jtsBzYh6");
    }
}
